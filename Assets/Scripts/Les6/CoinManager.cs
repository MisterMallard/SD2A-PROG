using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
	private static CoinManager instance;

	[SerializeField] private int coins;
	[SerializeField] private TextMeshProUGUI coinTxt;

	// READ ONLY (Projectile, Tower, TowerUpgradeUI, TowerPlacer, TowerSelectionUI)
	public static CoinManager Instance
	{
		get { return instance; }
	}

	// READ ONLY (TowerUpgradeUI, TowerSelectionUI)
	// Coins can't be negative, so the getter always returns a positive value.
	public int Coins
	{
		get { return Mathf.Abs(coins); }
	}

	private void Awake(){
		instance = this;
		UpdateCoins(0);
	}

	public void UpdateCoins(int changeAmount){
		coins += changeAmount;

		coinTxt.text = coins.ToString();
	}
}
