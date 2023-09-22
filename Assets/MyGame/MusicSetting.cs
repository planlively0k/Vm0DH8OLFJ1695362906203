using UnityEngine;
public class MusicSetting : MonoBehaviour
{
    private AudioSource music;
    private void Start() {  music = GetComponent<AudioSource>();  }
    private void Update()
    {
        if (PlayerPrefs.GetInt("Music") == 0) music.mute = true;
        else music.mute = false;
    }
}
