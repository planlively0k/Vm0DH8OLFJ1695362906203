using UnityEngine;
using UnityEngine.UI;
public class SongManager : MonoBehaviour {
	public static SongManager instance;
	[SerializeField] private GameObject songHolderPrefab;
	[SerializeField] private Transform contentHolder;
	[SerializeField] private Song[] songs;
	private int[] songLockeds;
	[HideInInspector] public int songCount;
	private int songUnlockedCount;
	public int mnumb;
	public GameObject warnSong;
	public GameObject rewardSuccess;
	private void Awake () { instance = this; }
	private void Start () {
		// songs = Resources.LoadAll<Song> ("Songs");
		var mySOng = CustomManagerEA.instance.listSong;
		Debug.Log("mySOng: "+ mySOng.Count);
		if(mySOng.Count > 0) {
			foreach (SongJson song in mySOng) {
				GameObject songObject = Instantiate (songHolderPrefab, contentHolder);
				// songObject.name = "Song" + songObject.transform.GetSiblingIndex ();
				songObject.name = "Song " + song.title;
				var newSong = new Song();
				newSong.name = song.title;
				newSong.BPM = song.bpm;
				songObject.GetComponent<SongHolder> ().SetSong (newSong);
				songObject.GetComponent<SongHolder> ().btnPlay.onClick.AddListener(() => {
					songObject.GetComponent<SongHolder> ().PlaySong(song.urlSong, true);
				});
				songObject.transform.GetChild (5).GetComponent<Button> ().onClick.AddListener (() => BuyThisSong (songObject.transform.GetSiblingIndex () - 1));
			}
			songLockeds = new int[mySOng.Count];
			songCount = mySOng.Count;
		} else {
			JSONData myDataLocal = CustomEAUtils.GetJsonDataFromLocal();
			Debug.Log("myDataLocal: "+myDataLocal.ListSong[0].title);
			mySOng = myDataLocal.ListSong;
			foreach (SongJson song in mySOng) {
				GameObject songObject = Instantiate (songHolderPrefab, contentHolder);
				// songObject.name = "Song" + songObject.transform.GetSiblingIndex ();
				songObject.name = "Song " + song.title;
				var newSong = new Song();
				newSong.name = song.title;
				newSong.BPM = song.bpm;
				songObject.GetComponent<SongHolder> ().SetSong (newSong);
				songObject.GetComponent<SongHolder> ().btnPlay.onClick.AddListener(() => {
					songObject.GetComponent<SongHolder> ().PlaySong(song.urlSong, false);
				});
				songObject.transform.GetChild (5).GetComponent<Button> ().onClick.AddListener (() => BuyThisSong (songObject.transform.GetSiblingIndex () - 1));
			}
			songLockeds = new int[mySOng.Count];
			songCount = mySOng.Count;
		}
		
		if (PlayerPrefs.GetInt ("FirstTimeSong") == 0) {
			PlayerPrefs.SetInt ("SongUnlocked0", 1);
			PlayerPrefs.SetInt ("SongUnlockedCount", 1);
			PlayerPrefs.SetInt ("FirstTimeSong", 1);
		}
	}
	private void Update () {
		songUnlockedCount = PlayerPrefs.GetInt ("SongUnlockedCount");
		for (int i = 0; i < songLockeds.Length; i++) {
			songLockeds[i] = PlayerPrefs.GetInt ("SongUnlocked" + i);
			if (songLockeds[i] == 0) {
				contentHolder.GetChild (i + 1).transform.GetChild (4).gameObject.SetActive (false);
				contentHolder.GetChild (i + 1).transform.GetChild (5).gameObject.SetActive (true);
			} else {
				contentHolder.GetChild (i + 1).transform.GetChild (4).gameObject.SetActive (true);
				contentHolder.GetChild (i + 1).transform.GetChild (5).gameObject.SetActive (false);
			}
		}
	}
	public void BuyThisSong (int numb) {
		Debug.Log("hallo: "+numb);
		mnumb = numb;
		if (HomeUi.instance.coins >= 30) {
			HomeUi.instance.coins -= 30;
			PlayerPrefs.SetInt ("Coins", HomeUi.instance.coins);
			PlayerPrefs.SetInt ("SongUnlocked" + numb, 1);
			songUnlockedCount++;
			PlayerPrefs.SetInt ("SongUnlockedCount", songUnlockedCount);
		} else {
			Debug.Log ("diamond tidak cukup");
			if (!warnSong.activeSelf) {
				warnSong.SetActive (true);
				// Invoke ("CloseWarn", 5f);
			}
		}
	}

	public void unlockByAds () {
		PlayerPrefs.SetInt ("SongUnlocked" + mnumb, 1);
		songUnlockedCount++;
		PlayerPrefs.SetInt ("SongUnlockedCount", songUnlockedCount);
		Debug.Log ("Song " + mnumb + " is unlocked");
		warnSong.SetActive (false);
		rewardSuccess.SetActive (true);
	}
	private void CloseWarn () {
		warnSong.SetActive (false);
	}
}