using UnityEngine;
public class CameraShake : Singleton<CameraShake>
{
	[SerializeField] private Transform camTransform;
	[SerializeField] private float shakeDuration;
	[SerializeField] private float shakeAmount = 0.7f;
	[SerializeField] private float decreaseFactor = 1f;
	private Vector3 originalPos;
	protected override void Awake()
	{
		base.Awake();
		if (camTransform == null)
		{
			UnityEngine.Debug.LogWarning("[CameraShake] camTransform no assigned! Will be taken the MainCamera's transform.");
			camTransform = Camera.main.transform;
		}
	}
	private void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}
	private void Update()
	{
		if (shakeDuration > 0f)
		{
			camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, originalPos + Random.insideUnitSphere * shakeAmount * shakeDuration, 0.3f);
			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shakeDuration = 0f;
			camTransform.localPosition = originalPos;
		}
	}
	public void Shake(float duration)
	{
		shakeDuration = duration;
	}
}
