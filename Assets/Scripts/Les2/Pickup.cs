using System;
using UnityEngine;

public class Pickup : MonoBehaviour
{
	public static event Action<int> OnPickupCollected;

	[SerializeField] private int scoreValue = 50;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			OnPickupCollected?.Invoke(scoreValue);
			Destroy(gameObject);
		}
	}
}
