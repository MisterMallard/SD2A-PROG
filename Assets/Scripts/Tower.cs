using UnityEngine;

public class Tower : MonoBehaviour
{
	void Start()
	{
		float x = Random.Range(0.7f, 1.5f);
		float y = Random.Range(0.8f, 2f);
		float z = Random.Range(0.7f, 1.5f);

		transform.localScale = new Vector3(x, y, z);
	}
}