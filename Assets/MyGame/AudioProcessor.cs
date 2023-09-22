using System;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(AudioSource))]
public class AudioProcessor : MonoBehaviour {
	[Serializable]
	public class OnBeatEventHandler : UnityEvent
	{
	}
	[Serializable]
	public class OnSpectrumEventHandler : UnityEvent<float[]>
	{
	}
	private class Autoco
	{
		private int del_length;
		private float decay;
		private float[] delays;
		private float[] outputs;
		private int indx;
		private float[] bpms;
		private float[] rweight;
		private float wmidbpm = 120f;
		private float woctavewidth;
		public Autoco(int len, float alpha, float framePeriod, float bandwidth)
		{
			woctavewidth = bandwidth;
			decay = alpha;
			del_length = len;
			delays = new float[del_length];
			outputs = new float[del_length];
			indx = 0;
			bpms = new float[del_length];
			rweight = new float[del_length];
			for (int i = 0; i < del_length; i++)
			{
				bpms[i] = 60f / (framePeriod * (float)i);
				rweight[i] = (float)Math.Exp(-0.5 * Math.Pow(Math.Log(bpms[i] / wmidbpm) / Math.Log(2.0) / (double)woctavewidth, 2.0));
			}
		}
		public void newVal(float val)
		{
			delays[indx] = val;
			for (int i = 0; i < del_length; i++)
			{
				int num = (indx - i + del_length) % del_length;
				outputs[i] += (1f - decay) * (delays[indx] * delays[num] - outputs[i]);
			}
			if (++indx == del_length) indx = 0;
		}
		public float autoco(int del)
		{
			return rweight[del] * outputs[del];
		}
		public float avgBpm()
		{
			float num = 0f;
			for (int i = 0; i < bpms.Length; i++) num += bpms[i];
			return num / (float)del_length;
		}
	}
	public AudioSource audioSource;
	private long lastT;
	private long nowT;
	private long diff;
	private long entries;
	private long sum;
	public int bufferSize = 1024;
	private int samplingRate = 44100;
	private int nBand = 12;
	public float gThresh = 0.1f;
	private int blipDelayLen = 16;
	private int[] blipDelay;
	private int sinceLast;
	private float framePeriod;
	private int colmax = 120;
	private float[] spectrum;
	private float[] averages;
	private float[] acVals;
	private float[] onsets;
	private float[] scorefun;
	private float[] dobeat;
	private int now;
	private float[] spec;
	private int maxlag = 100;
	private float decay = 0.997f;
	private Autoco auco;
	private float alph;
	[Header("Events")]
	public OnBeatEventHandler onBeat;
	public OnSpectrumEventHandler onSpectrum;
	private long getCurrentTimeMillis()
	{
		return DateTime.Now.Ticks / 10000;
	}
	private void initArrays()
	{
		blipDelay = new int[blipDelayLen];
		onsets = new float[colmax];
		scorefun = new float[colmax];
		dobeat = new float[colmax];
		spectrum = new float[bufferSize];
		averages = new float[12];
		acVals = new float[maxlag];
		alph = 100f * gThresh;
	}
	private void Start()
	{
		initArrays();
		audioSource = GetComponent<AudioSource>();
		samplingRate = audioSource.clip.frequency;
		framePeriod = (float)bufferSize / (float)samplingRate;
		spec = new float[nBand];
		for (int i = 0; i < nBand; i++) spec[i] = 100f;
		auco = new Autoco(maxlag, decay, framePeriod, getBandWidth());
		lastT = getCurrentTimeMillis();
	}
	public void tapTempo()
	{
		nowT = getCurrentTimeMillis();
		diff = nowT - lastT;
		lastT = nowT;
		sum += diff;
		entries++;
		int num = (int)(sum / entries);
		UnityEngine.Debug.Log("average = " + num);
	}
	private double[] toDoubleArray(float[] arr)
	{
		if (arr == null) return null;
		int num = arr.Length;
		double[] array = new double[num];
		for (int i = 0; i < num; i++) array[i] = arr[i];
		return array;
	}
	private void Update()
	{
		if (!audioSource.isPlaying) return;
		audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
		computeAverages(spectrum);
		onSpectrum.Invoke(averages);
		float num = 0f;
		for (int i = 0; i < nBand; i++)
		{
			float num2 = Math.Max(-100f, 20f * (float)Math.Log10(averages[i]) + 160f);
			num2 *= 0.025f;
			float num3 = num2 - spec[i];
			spec[i] = num2;
			num += num3;
		}
		onsets[now] = num;
		auco.newVal(num);
		float num4 = 0f;
		int num5 = 0;
		for (int j = 0; j < maxlag; j++)
		{
			float num6 = (float)Math.Sqrt(auco.autoco(j));
			if (num6 > num4)
			{
				num4 = num6;
				num5 = j;
			}
			acVals[maxlag - 1 - j] = num6;
		}
		float num7 = -999999f;
		int num8 = 0;
		alph = 100f * gThresh;
		for (int k = num5 / 2; k < Math.Min(colmax, 2 * num5); k++)
		{
			float num9 = num + scorefun[(now - k + colmax) % colmax] - alph * (float)Math.Pow(Math.Log((float)k / (float)num5), 2.0);
			if (num9 > num7)
			{
				num7 = num9;
				num8 = k;
			}
		}
		scorefun[now] = num7;
		float num10 = scorefun[0];
		for (int l = 0; l < colmax; l++)
		{
			if (scorefun[l] < num10)
			{
				num10 = scorefun[l];
			}
		}
		for (int m = 0; m < colmax; m++)
		{
			scorefun[m] -= num10;
		}
		num7 = scorefun[0];
		num8 = 0;
		for (int n = 0; n < colmax; n++)
		{
			if (scorefun[n] > num7)
			{
				num7 = scorefun[n];
				num8 = n;
			}
		}
		dobeat[now] = 0f;
		sinceLast++;
		if (num8 == now && sinceLast > num5 / 4)
		{
			onBeat.Invoke();
			blipDelay[0] = 1;
			dobeat[now] = 1f;
			sinceLast = 0;
		}
		if (++now == colmax)
		{
			now = 0;
		}
	}
	public void changeCameraColor()
	{
		float r = UnityEngine.Random.Range(0f, 1f);
		float g = UnityEngine.Random.Range(0f, 1f);
		float b = UnityEngine.Random.Range(0f, 1f);
		Color backgroundColor = new Color(r, g, b);
		GetComponent<Camera>().clearFlags = CameraClearFlags.Color;
		Camera.main.backgroundColor = backgroundColor;
	}
	public float getBandWidth()
	{
		return 2f / (float)bufferSize * ((float)samplingRate / 2f);
	}
	public int freqToIndex(int freq)
	{
		if ((float)freq < getBandWidth() / 2f) return 0;
		if ((float)freq > (float)(samplingRate / 2) - getBandWidth() / 2f) return bufferSize / 2;
		float num = (float)freq / (float)samplingRate;
		return (int)Math.Round((float)bufferSize * num);
	}
	public void computeAverages(float[] data)
	{
		for (int i = 0; i < 12; i++)
		{
			float num = 0f;
			int freq = (i != 0) ? ((int)((float)(samplingRate / 2) / (float)Math.Pow(2.0, 12 - i))) : 0;
			int freq2 = (int)((float)(samplingRate / 2) / (float)Math.Pow(2.0, 11 - i));
			int num2 = freqToIndex(freq);
			int num3 = freqToIndex(freq2);
			for (int j = num2; j <= num3; j++) num += data[j];
			num /= (float)(num3 - num2 + 1);
			averages[i] = num;
		}
	}
	private float map(float s, float a1, float a2, float b1, float b2)
	{
		return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
	}
	public float constrain(float value, float inclusiveMinimum, float inlusiveMaximum)
	{
		if (value >= inclusiveMinimum)
		{
			if (value <= inlusiveMaximum) return value;
			return inlusiveMaximum;
		}
		return inclusiveMinimum;
	}
}
