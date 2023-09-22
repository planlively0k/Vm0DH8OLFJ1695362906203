using System;
using UnityEngine;
public class Player : MonoBehaviour
{
	public float speed = 10f;
	public int platformHitCount = 1;
	[SerializeField] private float turnSpeed = 1f;
	[SerializeField] private float jumpHeigh = 5f;
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private LayerMask platformLayer;
	[SerializeField] private Transform jumpingPart;
	[SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private float baseSize = 1f;
	[SerializeField] private float scaleMultiplyer = 1f;
    private float d;
	private float p;
	private float t;
	private float turnAmount;
	private float fallZPos;
	private bool isTurning;
	private bool canMove = true;
	private Vector3 velocity;
	private Vector3 lastPos;
	private Vector3 playerOffset;
	private Transform platformA;
	private Transform platformB;
	private Rigidbody jumpingPartRB;
	private Rigidbody _rb;
	private Camera mainCamera;
	public float Speed => speed * ((Singleton<GameManager>.Instance == null) ? 1f : Singleton<GameManager>.Instance.GameSpeed);
	private void Awake()
	{
		_rb = GetComponentInChildren<Rigidbody>();
		jumpingPartRB = jumpingPart.GetComponent<Rigidbody>();
		mainCamera = Camera.main;
		canMove = false;
		platformHitCount = 1;
	}
	private void Update()
	{
		HandleInput();
		if (canMove)
		{
			Movement();
			Jumping();
			VelocityScale();
		}
	}
	private void VelocityScale()
	{
		velocity = jumpingPart.position - lastPos;
		lastPos = jumpingPart.position;
		if (velocity.magnitude != 0f) jumpingPart.forward = velocity;
		float value = baseSize - velocity.magnitude * scaleMultiplyer;
		value = Mathf.Clamp(value, 0.4f, 2f);
		float value2 = baseSize + velocity.magnitude * scaleMultiplyer;
		value2 = Mathf.Clamp(value2, 0.4f, 3f);
		jumpingPart.localScale = new Vector3(value, value, value2);
	}
	private void HandleInput()
	{
		if (Physics.Raycast(mainCamera.ScreenPointToRay(UnityEngine.Input.mousePosition), out RaycastHit hitInfo, 100f, groundLayer))
		{
			if (Input.GetMouseButtonDown(0))
			{
				isTurning = true;
				playerOffset = jumpingPart.position - hitInfo.point;
			}
			if (Input.GetMouseButtonUp(0))
			{
				isTurning = false;
			}
			if (isTurning)
			{
				turnAmount = Mathf.Lerp(turnAmount, hitInfo.point.x * turnSpeed + playerOffset.x, 0.2f);
			}
		}
	}
	private void Movement()
	{
		base.transform.position += Vector3.forward * Speed * Time.deltaTime;
	}
	private void Jumping()
	{
		if (platformA == null)
		{
			platformA = Singleton<LevelGenerator>.Instance.GetSpecificPlatform(0);
			return;
		}
		if (platformB == null)
		{
			platformB = Singleton<LevelGenerator>.Instance.GetSpecificPlatform(1);
			return;
		}
		d = Vector3.Distance(platformA.position, platformB.position);
		p = (base.transform.position.z - platformA.position.z) / (platformB.position.z - platformA.position.z);
		p = Mathf.Clamp(p, 0f, 1f);
		t = Mathf.Abs(Mathf.Cos((p - 0.5f) * (float)Math.PI));
		float value = Mathf.Lerp(0f, 1f, t) * jumpHeigh * d * 0.1f;
		value = Mathf.Clamp(value, 0f, 10f);
		Vector3 localPosition = new Vector3(turnAmount, value, 0f);
		jumpingPart.localPosition = localPosition;
		if (p >= 1f) CheckPlatform();
	}
	private void CheckPlatform()
	{
		if (Physics.CheckSphere(jumpingPart.position, 0.5f, platformLayer))
		{
			if (platformB.name == "Start")
			{
				Singleton<SoundManager>.Instance.PlayMusicFromBeat(1);
				Singleton<GameManager>.Instance.IncreaseGameSpeed();
				platformHitCount = 0;
			}
			hitEffect.transform.position = platformB.position;
			hitEffect.Play();
            
			bool perfect = Vector3.Distance(jumpingPart.position, platformB.position + Vector3.up * 0.3f) < 0.6f;
			platformB.GetComponent<Platform>().Hit();
			platformA = platformB;
			platformB = Singleton<LevelGenerator>.Instance.GetNextPlatform;
			platformHitCount++;
			Singleton<GameManager>.Instance.AddScore(perfect);
		}
		else if (canMove)
		{
			StopMoving();
			Singleton<GameManager>.Instance.PlayerFailed();
		}
	}
	private void StopMoving()
	{
		if (canMove)
		{
			canMove = false;
			jumpingPartRB.isKinematic = false;
			jumpingPartRB.velocity = jumpingPart.forward * Speed;
		}
	}
	public void Revive()
	{
		canMove = false;
		jumpingPartRB.isKinematic = true;
		Vector3 position = new Vector3(base.transform.position.x, base.transform.position.y, platformA.position.z);
		base.transform.position = position;
		turnAmount = 0f;
		jumpingPart.localPosition = new Vector3(platformA.position.x, 0f, 0f);
		MovingLoop component = platformA.gameObject.GetComponent<MovingLoop>();
		if (component != null) component.enabled = false;
	}
	public void StartMoving()
	{
		canMove = true;
	}
	public void StopAtPlatform(Transform platform)
	{
		canMove = false;
		base.transform.position = Vector3.forward * platform.position.z;
		Singleton<SoundManager>.Instance.StopTrack();
	}
	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(jumpingPart.position, 0.5f);
	}
}
