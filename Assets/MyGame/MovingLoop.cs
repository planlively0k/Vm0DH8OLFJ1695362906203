using UnityEngine;
public class MovingLoop : MonoBehaviour
{
	public float range = 3f;
	public float speed = 2f;
	private float xPos;
	private float offset;
	private Vector3 newPos;
	private void OnEnable()
	{
		offset = UnityEngine.Random.Range(0f, 1f) / speed;
		speed *= (((double)UnityEngine.Random.Range(0f, 1f) > 0.5) ? 1 : (-1));
	}
	private void Update()
	{
		float t = (Mathf.Cos((Time.time + offset) * speed) + 1f) / 2f;
		xPos = Mathf.Lerp(0f - range, range, t);
		newPos = new Vector3(xPos, base.transform.position.y, base.transform.position.z);
		base.transform.position = newPos;
	}
}
