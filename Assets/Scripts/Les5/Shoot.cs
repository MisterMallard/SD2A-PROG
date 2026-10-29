using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
	public GameObject projectilePrefab;
	private Plane floor;
	void Start()
	{
		floor = new Plane(Vector3.up, 0);
	}
	void Update()
	{
		if (Mouse.current.leftButton.wasPressedThisFrame)
		{
		float dist;
		Vector2 mousePosition = Mouse.current.position.ReadValue();
		Ray ray = Camera.main.ScreenPointToRay(mousePosition);
		if (floor.Raycast(ray, out dist))
		{
		GameObject p = Instantiate(projectilePrefab, transform.position, transform.rotation);
				Vector3 tPos = ray.GetPoint(dist);
		p.transform.LookAt(tPos);
		p.AddComponent<MoveProj>();
		Destroy(p, 5f);
			}
		}
	}
}
public class MoveProj : MonoBehaviour
{
	private float moveSpeed = 20f;
	void Update()
	{
	
	
transform.position += transform.forward * moveSpeed * Time.deltaTime;
}
}