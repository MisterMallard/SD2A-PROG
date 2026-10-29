using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("Movement")]
	[SerializeField, Min(0f)] private float speed = 2f;
	private Transform[] waypoints;
	private int currentWayPoint = 0;

	[Header("Health")]
	[SerializeField] private int health = 1;

	[Header("Damage to Player")]
	[SerializeField, Min(0)] private int damage = 10;

	private bool isDead = false;

	// WRITE ONLY (WaveManager)
	public Transform[] Waypoints
	{
		set { waypoints = value; }
	}

	// READ ONLY (Tower) - a waypoint index can never be negative.
	public int CurrentWayPoint
	{
		get { return Mathf.Abs(currentWayPoint); }
	}

	// READ & WRITE (Projectile, Deadeye)
	// No Mathf.Abs here: health is allowed to drop to 0 or below,
	// that is how the game detects that an enemy is dead.
	public int Health
	{
		get { return health; }
		set { health = value; }
	}

	void Update()
	{
		if (isDead || waypoints == null ||
			waypoints.Length == 0)
			return;

		if (currentWayPoint >= waypoints.Length)
		{
			ReachEnd();
			return;
		}

		Transform target = waypoints[currentWayPoint];

		if (target == null)
		{
			currentWayPoint++;
			return;
		}

		Vector3 dir =
			(target.position - transform.position).normalized;

		transform.position += dir * speed * Time.deltaTime;

		if (Vector3.Distance(
			transform.position, target.position) < 0.05f)
		{
			currentWayPoint++;

			if (currentWayPoint >= waypoints.Length)
			{
				ReachEnd();
			}
		}
	}

	public void TakeDamage(int damageAmount)
	{
		if (isDead || damageAmount <= 0)
			return;

		health -= damageAmount;

		Debug.Log(name + " took " + damageAmount +
				  " damage. Health: " + health);

		if (health <= 0)
		{
			Die();
		}
	}

	void Die()
	{
		if (isDead)
			return;

		isDead = true;

		Debug.Log(name + " was defeated!");

		Destroy(gameObject);
	}

	void ReachEnd()
	{
		if (isDead)
			return;

		isDead = true;

		if (HealthManager.Instance != null)
		{
			HealthManager.Instance.UpdateHealth(-damage);
		}

		Debug.Log(name + " reached the end and dealt " +
				  damage + " damage to the player!");

		// Destroy the enemy, not its waypoint.
		Destroy(gameObject);
	}
}
