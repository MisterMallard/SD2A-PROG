using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class WaveData
{
	[SerializeField] private float duration = 10f;

	[SerializeField] private int easyEnemies = 5;
	[SerializeField] private int hardEnemies = 2;
	[SerializeField] private int horseEnemies = 0;
	[SerializeField] private int bossEnemy = 0;

	// READ ONLY (WaveManager) - none of these can be negative
	public float Duration
	{
		get { return Mathf.Abs(duration); }
	}

	public int EasyEnemies
	{
		get { return Mathf.Abs(easyEnemies); }
	}

	public int HardEnemies
	{
		get { return Mathf.Abs(hardEnemies); }
	}

	public int HorseEnemies
	{
		get { return Mathf.Abs(horseEnemies); }
	}

	public int BossEnemy
	{
		get { return Mathf.Abs(bossEnemy); }
	}
}

public class WaveManager : MonoBehaviour
{
	[Header("Wave Settings")]
	[SerializeField] private WaveData[] waves;
	[SerializeField] private Button startWaveButton;
	[SerializeField] private TextMeshProUGUI waveTxt;

	[Header("Enemy Prefabs")]
	[SerializeField] private GameObject easyEnemyPrefab;
	[SerializeField] private GameObject hardEnemyPrefab;
	[SerializeField] private GameObject horseEnemyPrefab;
	[SerializeField] private GameObject bossEnemyPrefab;

	[Header("Waypoints")]
	[SerializeField] private Transform[] wayPoints;

	[Header("Win Screen")]
	[SerializeField] private GameObject winScreen;

	private int currentWaveIndex = 0;
	private bool waveRunning = false;
	private bool hasWon = false;

	// Tracks enemies that have spawned but haven't been destroyed.
	private HashSet<GameObject> activeEnemies = new HashSet<GameObject>();

	private void Start()
	{
		Time.timeScale = 1f;

		if (startWaveButton != null)
		{
			startWaveButton.onClick.AddListener(StartWave);
		}

		if (winScreen != null)
		{
			winScreen.SetActive(false);
		}

		UpdateWaveText();
	}

	private void Update()
	{
		// Remove enemies that have been destroyed.
		List<GameObject> deadEnemies = new List<GameObject>();

		foreach (GameObject enemy in activeEnemies)
		{
			if (enemy == null)
			{
				deadEnemies.Add(enemy);
			}
		}

		foreach (GameObject enemy in deadEnemies)
		{
			activeEnemies.Remove(enemy);
		}

		// Win only after every wave has spawned
		// and every enemy has been defeated.
		if (!hasWon &&
			!waveRunning &&
			currentWaveIndex >= waves.Length &&
			activeEnemies.Count == 0 &&
			Time.timeScale > 0f)
		{
			WinGame();
		}
	}

	public void StartWave()
	{
		if (waveRunning || hasWon)
		{
			return;
		}

		if (currentWaveIndex >= waves.Length)
		{
			return;
		}

		if (waves.Length == 0)
		{
			Debug.LogWarning("No waves have been configured!");
			return;
		}

		if (wayPoints == null || wayPoints.Length == 0)
		{
			Debug.LogError("No waypoints have been assigned!");
			return;
		}

		StartCoroutine(RunWave());
	}

	private IEnumerator RunWave()
	{
		waveRunning = true;

		if (startWaveButton != null)
		{
			startWaveButton.interactable = false;
		}

		WaveData wave = waves[currentWaveIndex];

		Debug.Log("Starting Wave " + (currentWaveIndex + 1));

		UpdateWaveText();

		// Calculate the total number of enemies in this wave.
		int totalEnemies =
			wave.EasyEnemies +
			wave.HardEnemies +
			wave.HorseEnemies +
			wave.BossEnemy;

		// Spawn enemies over approximately two-thirds
		// of the wave duration.
		float spawnInterval = totalEnemies > 0
			? (wave.Duration * 2f / 3f) / totalEnemies
			: 0f;

		// Spawn easy enemies.
		for (int i = 0; i < wave.EasyEnemies; i++)
		{
			SpawnEnemy(easyEnemyPrefab);

			yield return new WaitForSeconds(spawnInterval);
		}

		// Spawn hard enemies.
		for (int i = 0; i < wave.HardEnemies; i++)
		{
			SpawnEnemy(hardEnemyPrefab);

			yield return new WaitForSeconds(spawnInterval);
		}

		// Spawn horse enemies.
		for (int i = 0; i < wave.HorseEnemies; i++)
		{
			SpawnEnemy(horseEnemyPrefab);

			yield return new WaitForSeconds(spawnInterval);
		}

		// Spawn boss enemies.
		for (int i = 0; i < wave.BossEnemy; i++)
		{
			SpawnEnemy(bossEnemyPrefab);

			yield return new WaitForSeconds(spawnInterval);
		}

		// Finish the wave's remaining duration.
		float remainingDuration = wave.Duration / 3f;

		yield return new WaitForSeconds(remainingDuration);

		// Mark this wave as finished spawning.
		waveRunning = false;

		// Move to the next wave.
		currentWaveIndex++;

		UpdateWaveText();

		// Enable the button if more waves remain.
		if (startWaveButton != null)
		{
			startWaveButton.interactable =
				currentWaveIndex < waves.Length;
		}

		Debug.Log("Finished spawning Wave " + currentWaveIndex);

		// The Update method will handle victory when
		// all waves are finished and all enemies are gone.
	}

	private void SpawnEnemy(GameObject prefab)
	{
		if (prefab == null)
		{
			Debug.LogError("An enemy prefab has not been assigned!");
			return;
		}

		GameObject enemyObject = Instantiate(
			prefab,
			wayPoints[0].position,
			Quaternion.identity
		);

		// Track this enemy until it is destroyed.
		activeEnemies.Add(enemyObject);

		Enemy enemy = enemyObject.GetComponent<Enemy>();

		if (enemy != null)
		{
			enemy.Waypoints = wayPoints;
		}
		else
		{
			Debug.LogWarning(
				prefab.name + " does not have an Enemy component!"
			);
		}
	}

	private void UpdateWaveText()
	{
		if (waveTxt == null)
		{
			return;
		}

		if (waves == null || waves.Length == 0)
		{
			waveTxt.text = "No Waves";
		}
		else if (currentWaveIndex < waves.Length)
		{
			waveTxt.text =
				"Wave " + (currentWaveIndex + 1) +
				" / " + waves.Length;
		}
		else
		{
			waveTxt.text = "All Waves Completed!";
		}
	}

	private void WinGame()
	{
		if (hasWon)
		{
			return;
		}

		hasWon = true;

		Debug.Log("Congratulations! All waves completed!");

		if (winScreen != null)
		{
			winScreen.SetActive(true);
		}
		else
		{
			Debug.LogError(
				"Win Screen is not assigned in the Inspector!"
			);
		}

		if (startWaveButton != null)
		{
			startWaveButton.interactable = false;
		}

		// Pause the game.
		Time.timeScale = 0f;
	}

	// Connect this method to the Restart button
	// on your win screen.
	public void RestartGame()
	{
		Time.timeScale = 1f;

		SceneManager.LoadScene(
			SceneManager.GetActiveScene().buildIndex
		);
	}

	// Connect this method to the Quit button
	// on your win screen.
	public void QuitGame()
	{
		Time.timeScale = 1f;

		Debug.Log("Quitting game...");

		Application.Quit();
	}
}
