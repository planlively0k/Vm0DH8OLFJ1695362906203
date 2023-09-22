using UnityEngine;
using UnityEngine.UI;
public class BallManager : MonoBehaviour
{
    public static BallManager instance;
    public Renderer ballMat;
    public Texture[] ballTexs;
    public Button[] ballButtons;
    private int[] ballLockeds;
    private int lastBall;
    private int ballChoosen;
    public Button buyButton;
    public GameObject warnText;
    private int ballUnlockedCount;
    private void Awake() { instance = this; }
    private void Start()
    {
        ballLockeds = new int[ballButtons.Length];
        if (PlayerPrefs.GetInt("FirstTimeBall") == 0)
        {
            PlayerPrefs.SetInt("BallUnlocked0", 1);
            PlayerPrefs.SetInt("BallChoosen", 0);
            ballMat.material.mainTexture = ballTexs[PlayerPrefs.GetInt("BallChoosen")];
            PlayerPrefs.SetInt("BallUnlockedCount", 1);
            PlayerPrefs.SetInt("FirstTimeBall", 1);
        }
        ballChoosen = PlayerPrefs.GetInt("BallChoosen");
        lastBall = ballChoosen;
        ballButtons[lastBall].transform.GetChild(1).gameObject.SetActive(true);
    }
    private void Update()
    {
        ballUnlockedCount = PlayerPrefs.GetInt("BallUnlockedCount");
        for (int i = 0; i < ballLockeds.Length; i++)
        {
            ballLockeds[i] = PlayerPrefs.GetInt("BallUnlocked" + i);
            if (ballLockeds[i] == 0) ballButtons[i].transform.GetChild(2).gameObject.SetActive(true);
            else ballButtons[i].transform.GetChild(2).gameObject.SetActive(false);
        }
        ballChoosen = PlayerPrefs.GetInt("BallChoosen");
        ballMat.material.mainTexture = ballTexs[ballChoosen];
    }
    public void ChooseThisTexture(int numb)
    {
        ballButtons[lastBall].transform.GetChild(1).gameObject.SetActive(false);
        if (ballLockeds[numb] == 1)
        {
            PlayerPrefs.SetInt("BallChoosen", numb);
            buyButton.gameObject.SetActive(false);
            ballButtons[numb].transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            ballButtons[numb].transform.GetChild(1).gameObject.SetActive(true);
            buyButton.gameObject.SetActive(true);
        }
        lastBall = numb;
    }
    public void BuyThisBall()
    {
        if (HomeUi.instance.coins >= 20)
        {
            HomeUi.instance.coins -= 20;
            PlayerPrefs.SetInt("Coins", HomeUi.instance.coins);
            PlayerPrefs.SetInt("BallUnlocked" + lastBall, 1);
            ballUnlockedCount++;
            PlayerPrefs.SetInt("BallUnlockedCount", ballUnlockedCount);
            Invoke("ChooseAfterBuy", 0.1f);
        }
        else
        {
            if (!warnText.activeSelf)
            {
                warnText.SetActive(true);
                Invoke("CloseWarn", 1f);
            }
        }
    }
    private void ChooseAfterBuy()
    {
        ChooseThisTexture(lastBall);
    }
    private void CloseWarn()
    {
        warnText.SetActive(false);
    }
    public void CloseBallManager()
    {
        ballButtons[lastBall].transform.GetChild(1).gameObject.SetActive(false);
        lastBall = ballChoosen;
        ballButtons[lastBall].transform.GetChild(1).gameObject.SetActive(true);
        buyButton.gameObject.SetActive(false);
    }
}
