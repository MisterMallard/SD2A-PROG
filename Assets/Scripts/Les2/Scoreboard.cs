using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
	private int score = 0;
	private TMP_Text textField;

	private void Start()
	{
		textField = GetComponent<TMP_Text>();
		UpdateScore();

		Pickup.OnPickupCollected += AddPoints;
	}

	private void AddPoints(int points)
	{
		score += points;
		UpdateScore();
	}

	private void UpdateScore()
	{
		textField.text = "Score: " + score;
	}

	private void OnDestroy()
	{
		Pickup.OnPickupCollected -= AddPoints;
	}
}