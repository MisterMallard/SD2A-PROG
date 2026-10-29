using UnityEngine;

public class TowerSelectionUI : MonoBehaviour
{
	private static GameObject selectedTowerPrefab;

	// READ & WRITE (TowerPlacer)
	public static GameObject SelectedTowerPrefab
	{
		get { return selectedTowerPrefab; }
		set { selectedTowerPrefab = value; }
	}

	public void SelectTower(GameObject towerPrefab)
	{
		if(towerPrefab == selectedTowerPrefab){
			selectedTowerPrefab = null;
			return;
		}

		if (towerPrefab.GetComponent<Tower>().TowerPrice <= CoinManager.Instance.Coins)
		{
			selectedTowerPrefab = towerPrefab;
		}
	}
}
