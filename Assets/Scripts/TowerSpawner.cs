using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
	public GameObject towerPrefab;
	public Collider ground;

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			float x = Random.Range(
				ground.bounds.min.x,
				ground.bounds.max.x
			);

			float z = Random.Range(
				ground.bounds.min.z,
				ground.bounds.max.z
			);

			float y = ground.bounds.max.y;

			Vector3 position = new Vector3(x, y, z);

			Instantiate(
				towerPrefab,
				position,
				Quaternion.identity
			);
		}
	}
}