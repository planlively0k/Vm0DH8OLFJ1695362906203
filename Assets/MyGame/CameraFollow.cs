using UnityEngine;
public class CameraFollow : MonoBehaviour
{
	[SerializeField] private Transform target;
	[SerializeField] private float smoothDamp = 0.5f;
	private Vector3 offset;
	private Vector3 velocity;
	private void Awake()
	{
		offset = base.transform.position - target.position;
	}
	private void LateUpdate()
	{
		base.transform.position = Vector3.SmoothDamp(base.transform.position, target.position + offset, ref velocity, smoothDamp);
	}
}
