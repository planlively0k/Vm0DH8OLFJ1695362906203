using UnityEngine;
// [CreateAssetMenu(fileName = "New Song", menuName = "Create Song Asset")]
public class Song : ScriptableObject
{
	public AudioClip song;
	public int BPM;
	public int stars;
	public float TimeFromBeat(int beat) { return song.length / (song.length / 60f * (float)BPM) * (float)beat; }
	public int BeatFromTime(float time) { return (int)(time * song.length / 60f * (float)BPM); }
	public void LoadData() { stars = PlayerPrefs.GetInt(base.name + "Stars", 0); }
	public void SaveData() { PlayerPrefs.SetInt(base.name + "Stars", stars); }
}
