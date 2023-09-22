using System.Collections.Generic;
using UnityEngine;
public class LevelGenerator : Singleton<LevelGenerator>
{
	public Song currentSong;
	[HideInInspector] public int platformsPassed;
	[SerializeField] private int platformsDrawn = 7;
	[SerializeField] private float platformTurnStep = 0.5f;
	[SerializeField] private float levelWidth = 10f;
	[Range(0f, 1f)]
	[SerializeField] private float movingPlatformChance = 0.2f;
	[SerializeField] private Player player;
	[SerializeField]private Pool platformPool;
	[SerializeField] private Pool movingPlatformPool;
	[SerializeField] private GameObject diamondPrefab;
	private int[] diamondIDs = new int[3];
	private int songLevel;
	private int platformCount;
	private bool nextPlatformIsStart;
	private float lastPlatformZ;
	private List<GameObject> platformList = new List<GameObject>();
	public int beatPerSong => (int)(currentSong.song.length / 60f * (float)currentSong.BPM);
	private float distanceBetweenPlatforms => currentSong.song.length / (float)beatPerSong * ((player == null) ? 10f : player.speed);
	public bool PathIsValid => platformList.Count > 2;
	private float TurnStep
	{
		get
		{
			float num = 0f;
			if (Random.Range(0f, 1f) > 0.1f) return Random.Range(platformTurnStep / 2f, platformTurnStep);
			return platformTurnStep * 2f;
		}
	}
	public Transform GetNextPlatform
	{
		get
		{
			GameObject gameObject = platformList[2];
			if (platformList[0].tag == "Moving") movingPlatformPool.ReturnItem(platformList[0]);
			else platformPool.ReturnItem(platformList[0]);
			platformList.RemoveAt(0);
			GameObject gameObject2 = null;
			gameObject2 = ((!(Random.Range(0f, 1f) <= movingPlatformChance) || platformCount == beatPerSong - 1) ? platformPool.GetItem : movingPlatformPool.GetItem);
			gameObject2.GetComponent<Animator>().SetTrigger("Spawn");
			if (nextPlatformIsStart)
			{
				nextPlatformIsStart = false;
				gameObject2.name = "Start";
			}
			Reposition(gameObject2, platformCount);
			platformList.Add(gameObject2);
			platformCount++;
			platformsPassed++;
			if (platformsPassed >= beatPerSong) IncreaseDificulty();
			return gameObject.transform;
		}
	}
	public Transform GetSpecificPlatform(int id)
	{
		return platformList[id].transform;
	}
	public void StartWithSong()
	{
		platformCount = 0;
		SetStarIDs();
		for (int i = 0; i < platformsDrawn; i++)
		{
			GameObject getItem = platformPool.GetItem;
			Reposition(getItem, platformCount);
			platformList.Add(getItem);
			platformCount++;
			platformsPassed++;
		}
	}
	private void SetStarIDs()
	{
		diamondIDs[0] = currentSong.BeatFromTime(0.3f);
		diamondIDs[1] = currentSong.BeatFromTime(0.64f);
		diamondIDs[2] = currentSong.BeatFromTime(0.99f);
	}
	private void IncreaseDificulty()
	{
		platformsPassed = 0;
		lastPlatformZ += 40f;
		nextPlatformIsStart = true;
	}
	private bool CheckForStar()
	{
		for (int i = 0; i < 3; i++)
		{
			if (platformCount == diamondIDs[i]) return true;
		}
		return false;
	}
	public void Reposition(GameObject platform, int id)
	{
		float x = (id > 3) ? Random.Range(0f - levelWidth, levelWidth) : 0f;
		platform.transform.position = new Vector3(x, platform.transform.position.y, lastPlatformZ);
		lastPlatformZ += distanceBetweenPlatforms;
		if (CheckForStar()) Object.Instantiate(diamondPrefab, platform.transform);
	}
	private void OnDrawGizmos()
	{
		if (!(currentSong == null) && !(currentSong.song == null))
		{
			for (int i = 0; i < beatPerSong; i++) Gizmos.DrawSphere(new Vector3(0f, 0f, distanceBetweenPlatforms * (float)i), 0.5f);
		}
	}
}
