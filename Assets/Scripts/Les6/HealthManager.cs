using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
	private static HealthManager instance;

	[Header("Health")]
	[SerializeField, Min(0)] private int health = 100;
	[SerializeField] private TextMeshProUGUI healthText;

	[Header("Lose Screen")]
	[SerializeField] private GameObject loseScreen;

	private bool hasLost = false;

	// READ ONLY (Enemy)
	public static HealthManager Instance
	{
		get { return instance; }
	}

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		// Hide the lose screen when the game starts
		if (loseScreen != null)
		{
			loseScreen.SetActive(false);
		}

		// Display the starting health
		UpdateHealthText();
	}

	public void UpdateHealth(int changeAmount)
	{
		// Don't change health after losing
		if (hasLost)
		{
			return;
		}

		health += changeAmount;

		// Prevent health from going below zero
		health = Mathf.Max(0, health);

		UpdateHealthText();

		// Check if the player has lost
		if (health <= 0)
		{
			LoseGame();
		}
	}

	private void UpdateHealthText()
	{
		if (healthText != null)
		{
			healthText.text = health.ToString();
		}
	}

	private void LoseGame()
	{
		if (hasLost)
		{
			return;
		}

		hasLost = true;

		// Show the lose screen with its buttons
		if (loseScreen != null)
		{
			loseScreen.SetActive(true);
		}
		else
		{
			Debug.LogError("Lose Screen is not assigned in the Inspector!");
		}

		// Pause the game
		Time.timeScale = 0f;
	}

	// Connect this method to your Restart button
	public void RestartGame()
	{
		Time.timeScale = 1f;

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	// Connect this method to your Quit button
	public void QuitGame()
	{
		Time.timeScale = 1f;

		Debug.Log("Quitting game...");

		Application.Quit();
	}
}
