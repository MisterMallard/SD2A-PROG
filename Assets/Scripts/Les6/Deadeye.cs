
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class Deadeye : MonoBehaviour
{
	[Header("UI")]
	[SerializeField] private Button tacticalButton;
	[SerializeField] private Image yellowOverlay;
	[SerializeField] private Sprite overlaySprite;

	[Header("Timing")]
	[SerializeField, Min(0f)] private float duration = 3f;
	[SerializeField, Min(0f)] private float cooldown = 60f;

	[Range(0.05f, 1f)]
	[SerializeField] private float slowMotionScale = 0.3f;

	[Header("Damage")]
	[SerializeField, Min(0)] private int damagePerClick = 1;

	[Header("2D Enemy Detection")]
	[SerializeField] private LayerMask enemyLayers = ~0;

	[Header("Sound Effects")]
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip activationSound;
	[SerializeField] private AudioClip enemyClickSound;
	[SerializeField] private AudioClip cooldownReadySound;

	private bool tacticalMode;
	private bool coolingDown;

	private float originalTimeScale = 1f;

	private Coroutine tacticalRoutine;
	private Coroutine cooldownRoutine;

	void Start()
	{
		originalTimeScale = Time.timeScale;

		if (yellowOverlay != null)
		{
			if (overlaySprite != null)
				yellowOverlay.sprite = overlaySprite;

			yellowOverlay.color = new Color(1f, 0.85f, 0f, 0f);
			yellowOverlay.raycastTarget = false;
			yellowOverlay.gameObject.SetActive(false);
		}

		if (tacticalButton != null)
		{
			tacticalButton.onClick.AddListener(ActivateTacticalMode);
		}

		if (audioSource == null)
		{
			Debug.LogWarning(
				"Deadeye: No AudioSource assigned in the Inspector."
			);
		}
	}

	void Update()
	{
		if (!tacticalMode)
			return;

		if (Input.GetMouseButtonDown(0))
		{
			// Don't select enemies when clicking UI buttons.
			if (EventSystem.current != null &&
				EventSystem.current.IsPointerOverGameObject())
			{
				return;
			}

			DetectEnemy();
		}
	}

	public void ActivateTacticalMode()
	{
		if (tacticalMode || coolingDown)
			return;

		tacticalMode = true;
		coolingDown = true;

		Time.timeScale = slowMotionScale;

		if (yellowOverlay != null)
		{
			yellowOverlay.gameObject.SetActive(true);
			yellowOverlay.color = new Color(1f, 0.85f, 0f, 0.25f);
		}

		PlaySound(activationSound, "Activation");

		if (tacticalButton != null)
			tacticalButton.interactable = false;

		tacticalRoutine = StartCoroutine(TacticalTimer());
		cooldownRoutine = StartCoroutine(CooldownTimer());

		Debug.Log("Deadeye activated!");
	}

	IEnumerator TacticalTimer()
	{
		yield return new WaitForSecondsRealtime(duration);

		DeactivateTacticalMode();
	}

	void DeactivateTacticalMode()
	{
		tacticalMode = false;

		Time.timeScale = originalTimeScale;

		if (yellowOverlay != null)
		{
			yellowOverlay.color = new Color(1f, 0.85f, 0f, 0f);
			yellowOverlay.gameObject.SetActive(false);
		}

		tacticalRoutine = null;

		Debug.Log("Deadeye ended. Cooldown is still running.");
	}

	IEnumerator CooldownTimer()
	{
		yield return new WaitForSecondsRealtime(cooldown);

		coolingDown = false;

		if (tacticalButton != null)
			tacticalButton.interactable = true;

		PlaySound(cooldownReadySound, "Cooldown ready");

		cooldownRoutine = null;

		Debug.Log("Deadeye is ready again!");
	}

	void DetectEnemy()
	{
		Camera cam = Camera.main;

		if (cam == null)
		{
			Debug.LogError("Deadeye: No MainCamera found!");
			return;
		}

		// Cast a ray from the camera through the mouse position.
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);

		RaycastHit2D hit = Physics2D.GetRayIntersection(
			ray,
			Mathf.Infinity,
			enemyLayers
		);

		if (hit.collider == null)
		{
			Debug.Log("Deadeye: No 2D collider hit. Check enemy colliders and layers.");
			return;
		}

		// Find the EnemyTarget component on the clicked enemy or its parents.
		EnemyTarget target = hit.collider.GetComponentInParent<EnemyTarget>();

		if (target == null)
		{
			Debug.LogWarning(
				"Deadeye: Hit " + hit.collider.name +
				", but no EnemyTarget was found on it or its parents."
			);
			return;
		}

		// Find the health component belonging to THIS enemy.
		Enemy targetHealth = target.GetComponentInParent<Enemy>();

		if (targetHealth == null)
		{
			Debug.LogError(
				"Deadeye: " + target.name +
				" has no Enemy component on it or its parents."
			);
			return;
		}

		// Apply damage to the enemy that was clicked.
		targetHealth.Health -= damagePerClick;

		Debug.Log(
			"Hit " + target.name +
			" for " + damagePerClick +
			" damage. Health remaining: " + targetHealth.Health
		);

		// Highlight the enemy.
		target.Select();

		// Play the selection sound.
		PlaySound(enemyClickSound, "Enemy click");
	}

	void PlaySound(AudioClip clip, string soundName)
	{
		if (audioSource == null)
		{
			Debug.LogWarning(
				"Deadeye: Cannot play " + soundName +
				" because AudioSource is missing."
			);
			return;
		}

		if (clip == null)
		{
			Debug.LogWarning(
				"Deadeye: " + soundName +
				" sound clip has not been assigned."
			);
			return;
		}

		audioSource.PlayOneShot(clip);

		Debug.Log("Deadeye: Playing " + soundName + " sound.");
	}

	void OnDisable()
	{
		// Prevent slow motion from remaining active if this component
		// is disabled while tactical mode is running.
		if (tacticalMode)
		{
			tacticalMode = false;
			Time.timeScale = originalTimeScale;

			if (yellowOverlay != null)
				yellowOverlay.gameObject.SetActive(false);
		}
	}

	void OnDestroy()
	{
		if (tacticalButton != null)
			tacticalButton.onClick.RemoveListener(ActivateTacticalMode);

		// Don't leave the game slowed down if this component is destroyed.
		Time.timeScale = 1f;
	}
}
