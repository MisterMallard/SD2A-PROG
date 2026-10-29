using UnityEngine;

public class DeathWall : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		Destroy(collision.gameObject);
	}
}
