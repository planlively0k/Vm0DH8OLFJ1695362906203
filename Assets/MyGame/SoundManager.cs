using UnityEngine;
using UnityEngine.Audio; 
public class SoundManager : Singleton<SoundManager>
{
	[SerializeField] private AudioSource SFX;
	[SerializeField] private AudioSource track; 
	[SerializeField] private AudioMixer mainMixer; 
	[SerializeField] private AudioClip blip; 
	[SerializeField] private AudioClip blop; 
	private int bitCount; 
	private void Start()
	{
		Sounds((PlayerPrefs.GetInt("Sound", 1) == 1) ? true : false);
	} 
	public void BitSound()
	{
		if (bitCount == 0) SFX.PlayOneShot(blip);
		else SFX.PlayOneShot(blop);
		bitCount++;
		if (bitCount > 3) bitCount = 0;
	}
	private void Update()
	{
        if(mainMixer) mainMixer.SetFloat("Music Pitch", Singleton<GameManager>.Instance.GameSpeed);
		Singleton<GameManager>.Instance.UpdateSongProgress(track.time / track.clip.length);
	}
	public void PlayMusic()
	{
		track.clip = Singleton<LevelGenerator>.Instance.currentSong.song;
		track.Play();
	}
	public void PlayMusicFromBeat(int beat)
	{
		track.clip = Singleton<LevelGenerator>.Instance.currentSong.song;
		track.time = Singleton<LevelGenerator>.Instance.currentSong.TimeFromBeat(beat - 1);
		track.Play();
	}
	public void StopTrack()
	{
		track.Pause();
	}
	public void Sounds(bool on)
	{
		if (mainMixer) mainMixer.SetFloat("Music Volume", (!on) ? (-80) : 0);
	}
}
