using UnityEngine;
public class Star : MonoBehaviour
{
	[SerializeField] private GameObject collectedEffect;
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate(collectedEffect, base.transform.position, Quaternion.Euler(-90f, 0f, 0f)), 3f);
			Singleton<GameManager>.Instance.star++;
			UnityEngine.Object.Destroy(base.gameObject);
            //efekair.SetActive(true);

		}
	}
}
