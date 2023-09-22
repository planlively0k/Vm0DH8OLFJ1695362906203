using UnityEngine;
using UnityEngine.UI;
public class UIManager : Singleton<UIManager>
{
	public GameObject HUDPanel;
	[SerializeField] private Animator menuAnim;
	[SerializeField] private GameObject gameBg;
	[SerializeField] private GameObject gameUI;
	[SerializeField] private Text bestScoreText; 
	public GameObject loadingPlay;
	public void CloseMenu()
	{
		menuAnim.SetTrigger("Close");
		gameBg.SetActive(value: true);
		gameBg.transform.GetChild(ChooseTheme.instance.chooseTheme - 1).gameObject.SetActive(true);
		gameUI.SetActive(value: true);
	}
	private void OnEnable()
	{
		bestScoreText.text = PlayerPrefs.GetInt("bestScore", 0).ToString();
	}
	public void ShowHUD(bool value)
	{
		HUDPanel.SetActive(value);
	}
}
