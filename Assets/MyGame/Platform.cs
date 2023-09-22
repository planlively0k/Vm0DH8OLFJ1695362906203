using UnityEngine;
public class Platform : MonoBehaviour
{
	[SerializeField] private Animator anim; 
	public void Hit() { anim.SetTrigger("Hit"); }
}
