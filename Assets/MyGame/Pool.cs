using System.Collections.Generic;
using UnityEngine;
public class Pool : MonoBehaviour
{
	[SerializeField] private int startAmount = 1;
	[SerializeField] private GameObject itemPrefab; 
	private Queue<GameObject> poolQueue = new Queue<GameObject>();
	public GameObject GetItem
	{
		get
		{
			if (poolQueue.Count < 1) AddToPool(1);
			GameObject gameObject = poolQueue.Dequeue();
			gameObject.SetActive(value: true);
			return gameObject;
		}
	}
	private void Awake()
	{
		if (startAmount > 0)
		{
			for (int i = 0; i < startAmount; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(itemPrefab, base.transform);
				gameObject.SetActive(value: false);
				poolQueue.Enqueue(gameObject);
			}
		}
	}
	private void AddToPool(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(itemPrefab, base.transform);
			gameObject.SetActive(value: false);
			poolQueue.Enqueue(gameObject);
		}
	}
	public void ReturnItem(GameObject item)
	{
		item.name = itemPrefab.name;
		item.GetComponent<Animator>().ResetTrigger("Perfect");
		poolQueue.Enqueue(item);
	}
}
