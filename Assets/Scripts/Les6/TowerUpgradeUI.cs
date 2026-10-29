using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerUpgradeUI : MonoBehaviour
{
	private Tower tower;

	[SerializeField] private Button upgradeButton;
	[SerializeField] private TextMeshProUGUI priceTxt;

	private bool justOpened = true;

	// WRITE ONLY (Tower)
	public Tower Tower
	{
		set { tower = value; }
	}

	// READ ONLY (Tower)
	public TextMeshProUGUI PriceTxt
	{
		get { return priceTxt; }
	}

	private void Awake()
	{
		upgradeButton.onClick.AddListener(TryUpgrade);
	}

	private void TryUpgrade()
	{
		if (CoinManager.Instance.Coins < tower.UpgradeStages[tower.UpgradeStage].Price) return;

		tower.Upgrade();

		Destroy(gameObject);
	}
	public void Update()
	{
		if(justOpened)
		{
			justOpened = false;
			return;
		}

		if(Input.GetMouseButtonDown(0))
		{
			if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
				return;

				Destroy(gameObject);
		}

		if (tower.UpgradeStage >= tower.UpgradeStages.Length)
		{
			Destroy(gameObject);
		}
	}
}
