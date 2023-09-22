using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class SongHolder : MonoBehaviour
{
    [SerializeField] private Song song;
    [SerializeField] private Text songName;
    [SerializeField] private Image[] stars = new Image[3];
    [SerializeField] private Color activeStars;
    [SerializeField] private Color inactiveStars;
    public Button btnPlay;
    public void SetSong(Song newSong)
    {
        song = newSong;
        UpdateInfo();
    }
    public void UpdateInfo()
    {
        song.LoadData();
        for (int i = 0; i < 3; i++) stars[i].color = ((i < song.stars) ? activeStars : inactiveStars);
        songName.text = song.name;
    }
    public async void PlaySong(string urlAudio, bool isOnlie)
    { 
        if (isOnlie)
        {
			Singleton<UIManager>.Instance.loadingPlay.SetActive(true);
        	var loadingPlay = Singleton<UIManager>.Instance.loadingPlay.GetComponent<LoadingPlay>();
            var audioClip = await CustomEAUtils.LoadAudioFromUrl(urlAudio, (progress) =>
            {
                loadingPlay.UpdateLoading(progress);
            });
            song.song = audioClip;
			loadingPlay.gameObject.SetActive(false);
        } else {
			var audioClip = CustomEAUtils.LoadAudioFromLocal(urlAudio, (progress) =>
            {
                // loadingPlay.UpdateLoading(progress);
            });
            song.song = audioClip;
		}
        Singleton<LevelGenerator>.Instance.currentSong = song;
        Singleton<LevelGenerator>.Instance.StartWithSong();
        Singleton<UIManager>.Instance.CloseMenu();
    }
}