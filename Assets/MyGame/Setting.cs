using UnityEngine; 
public class Setting : MonoBehaviour
{
	[SerializeField] private string settingName; 
	[SerializeField] private GameObject on; 
	[SerializeField] private GameObject off; 
	private bool isOn; 
	private void Awake()
	{
		isOn = (PlayerPrefs.GetInt(settingName, 1) == 1);
		on.SetActive(isOn);
		off.SetActive(!isOn);
	} 
	public void ChangeSetting()
	{
		isOn = !isOn;
		on.SetActive(isOn);
		off.SetActive(!isOn);
		PlayerPrefs.SetInt(settingName, isOn ? 1 : 0);
		Singleton<SoundManager>.Instance.Sounds(isOn);
	}
}
