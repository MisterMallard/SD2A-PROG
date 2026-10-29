using UnityEngine;

public class Enemy : MonoBehaviour
{
	protected float speed = 5f;
	protected int Hp = 60;
	protected CapsuleCollider capsuleCollider;
	protected void Start()
	{
		capsuleCollider = GetComponent<CapsuleCollider>();

	}

	protected virtual void Update() 
	{


	transform.position += Vector3.right * (speed * Time.deltaTime);

	}

	protected void OnCollisionEnter(Collision collision)
	{
	if (collision.gameObject.CompareTag("Damage"))
	{


			TakeDamage();
	}

	}

	protected private void Death()
	{
	capsuleCollider.enabled = false;
	Destroy(gameObject);
	}

	protected private void TakeDamage()
	{
	Hp -= 20;
		if (Hp <= 0)
		{
			Death();
		}
	}
}