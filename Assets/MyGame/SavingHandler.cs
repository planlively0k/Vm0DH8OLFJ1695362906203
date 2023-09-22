using UnityEngine;
public class SavingHandler : Singleton<SavingHandler>
{
	[HideInInspector] public int bestScore;
	protected override void Awake()
	{
		base.Awake();
		LoadData();
	}
	public void LoadData() { bestScore = PlayerPrefs.GetInt("bestScore", 0); }
	public void SaveData() { PlayerPrefs.GetInt("bestScore", bestScore); }
}
