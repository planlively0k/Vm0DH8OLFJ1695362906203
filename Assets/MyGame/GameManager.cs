using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {
	public int score;

	public int bestScore;

	public int star;

	private int gameSpeed = 1;

	private GameState gameState;

	private Player player;

	private float songProgress;

	[SerializeField] private ParticleSystem bintang;
	[SerializeField] private ParticleSystem bintang2;
	[SerializeField] private ParticleSystem bintang3;

	[Header ("UI")]
	[SerializeField]
	private Image levelProgress;

	[SerializeField]
	private Text scoreText;

	//[SerializeField]
	//private Animator scoreAnim;

	[SerializeField]
	private Animator reviveAnim;

	[SerializeField]
	private GameObject revivePanel;

	[SerializeField]
	private GameObject playButton;

	[SerializeField]
	private GameObject watereffect;

	[Space]
	[SerializeField]
	private Text songName;

	[SerializeField]
	private Text levelScore;

	[SerializeField]
	private Image[] stars = new Image[3];

	[SerializeField]
	private Color activeStars;

	[SerializeField]
	private Color inactiveStars;

	public float GameSpeed => (float) (gameSpeed - 1) * 0.2f + 1f;

	public GameState CurrentGameState => gameState;

	protected override void Awake () {
		base.Awake ();
		player = Object.FindObjectOfType<Player> ();
		bestScore = PlayerPrefs.GetInt ("bestScore", 0);
	}

	private void Start () {
		scoreText.text = score.ToString ();
		//if (scoreAnim.isActiveAndEnabled)
		//{
		//	scoreAnim.SetTrigger("Up");
		//}
	}

	public void PlayerFailed () {
		gameState = GameState.Lost;
		Singleton<SoundManager>.Instance.StopTrack ();
		revivePanel.SetActive (value: true);
		Singleton<UIManager>.Instance.ShowHUD (value: false);
		if (Singleton<LevelGenerator>.Instance.currentSong.stars < star) {
			Singleton<LevelGenerator>.Instance.currentSong.stars = star;
			Singleton<LevelGenerator>.Instance.currentSong.SaveData ();
		}
		if (score > bestScore) {
			PlayerPrefs.SetInt ("bestScore", score);
		}
		ShowLevelProgress ();
		PlayerPrefs.Save ();

		int Ads_count = PlayerPrefs.GetInt ("HopTile_gameover", 0);
		Ads_count++;
		if (Ads_count >= 1) {
			Ads_count = 0;
			if(AdsManagerWrapper.INSTANCE)
			AdsManagerWrapper.INSTANCE.ShowInterstitial((onAdLoded) => {}, onAdFailedToLoad => {});
			// AdmobAd.instance.ShowInterstitial ();
		}
		PlayerPrefs.SetInt ("HopTile_gameover", Ads_count);
		PlayerPrefs.Save ();
	}

	private void ShowLevelProgress () {
		songName.text = Singleton<LevelGenerator>.Instance.currentSong.name;
		levelScore.text = score.ToString ();
		for (int i = 0; i < 3; i++) {
			if (i < star) {
				stars[i].color = activeStars;
			} else {
				stars[i].color = inactiveStars;
			}
		}
	}

	public void Revive () {
	
		// AdmobAd.instance.ShowRewardedRevive ();
		if(AdsManagerWrapper.INSTANCE)
		AdsManagerWrapper.INSTANCE.ShowRewards((onAdLoded) => {}, onAdFailedToLoad => {}, (irewards) => {
			ReviveSucceed(true);
		});
		
	}

	public void ReviveSucceed (bool completed) {
		if (completed) {
			player.Revive ();
			revivePanel.SetActive (value: false);
			playButton.SetActive (value: true);
		}
	}

	public void StartGame () {
		Debug.Log ("Start!");
		if (CurrentGameState == GameState.Menu) {
			star = 0;
			gameState = GameState.Gameplay;
			player.StartMoving ();
			Singleton<SoundManager>.Instance.PlayMusicFromBeat (player.platformHitCount);
			if (!bintang.isPlaying) {
				bintang.Play ();
			}
			if (!bintang2.isPlaying) {
				bintang2.Play ();
			}
			if (!bintang3.isPlaying) {
				bintang3.Play ();
			}
		} else if (CurrentGameState == GameState.Lost) {
			gameState = GameState.Gameplay;

			player.StartMoving ();
			Singleton<SoundManager>.Instance.PlayMusicFromBeat (player.platformHitCount);
			if (!bintang.isPlaying) {
				bintang.Play ();
			}
		}
		Singleton<UIManager>.Instance.ShowHUD (value: true);
		//AdsManager.Instance.HideBanner();

	}

	public void IncreaseGameSpeed () {
		if (gameSpeed < 5) {
			gameSpeed++;
		}
	}

	public void AddScore (bool perfect) {
		if (perfect) {
			score += 10;
		} else {
			score += 5;
		}
		scoreText.text = score.ToString ();
		//scoreAnim.SetTrigger("Up");
	}

	public void UpdateSongProgress (float value) {
		songProgress = value;
		levelProgress.fillAmount = Mathf.Lerp (levelProgress.fillAmount, value, 0.1f);
		if (songProgress >= 0.3 && songProgress <= 0.39 && star == 1) {
			watereffect.SetActive (true);
		} else if (songProgress >= 0.6 && songProgress <= 0.69 && star == 2) {
			watereffect.SetActive (true);
		} else if (songProgress >= 0.95) {
			watereffect.SetActive (true);
		} else {
			watereffect.SetActive (false);
		}

		//Debug.Log(songProgress);

	}

	public void NoThanks () {
		reviveAnim.SetTrigger ("No");

	}

	public void Menu () {
		SceneManager.LoadScene (0);
	}
}