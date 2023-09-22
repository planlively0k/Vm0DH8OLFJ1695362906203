using System.Collections.Generic;
using UnityEngine;
public class SpectrumEffects : MonoBehaviour
{
	[Tooltip("12 objects")]
	public List<GameObject> cubeList = new List<GameObject>();
	public float height;
	public float width;
	private void Start()
	{
		AudioProcessor audioProcessor = UnityEngine.Object.FindObjectOfType<AudioProcessor>();
		audioProcessor.onBeat.AddListener(onOnbeatDetected);
		audioProcessor.onSpectrum.AddListener(onSpectrum);
	}
	private void onOnbeatDetected()
	{
		UnityEngine.Debug.Log("Beat!!!");
	}
	private void onSpectrum(float[] spectrum)
	{
		for (int i = 0; i < spectrum.Length; i++) cubeList[i].transform.localScale = new Vector3(width, Mathf.Lerp(cubeList[i].transform.localScale.y, spectrum[i] * height, 0.2f), width); 
	}
}
