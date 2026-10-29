using UnityEngine;

[System.Serializable]
public class TowerUpgradeStage
{
    [SerializeField] private float range;
    [SerializeField] private float fireRate;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int price;

    // READ ONLY (Tower) - can't be negative
    public float Range
    {
        get { return Mathf.Abs(range); }
    }

    // READ ONLY (Tower) - can't be negative
    public float FireRate
    {
        get { return Mathf.Abs(fireRate); }
    }

    // READ ONLY (Tower)
    public Sprite Sprite
    {
        get { return sprite; }
    }

    // READ ONLY (Tower, TowerUpgradeUI) - can't be negative
    public int Price
    {
        get { return Mathf.Abs(price); }
    }
}
public class Tower : MonoBehaviour
{
    [SerializeField, Min(0f)] private float range = 3f;
    [SerializeField, Min(0f)] private float fireRate = 1f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private TowerUpgradeStage[] upgradeStages;
    private int upgradeStage = 0;
    private SpriteRenderer sr;
    [SerializeField] private GameObject towerUpgradeUIPrefab;
    private GameObject currentUI;

    [SerializeField] private GameObject cloudPS;

    [SerializeField] private int towerPrice = 1;

    private float fireCooldown = 0f;

    // READ ONLY (TowerUpgradeUI)
    public TowerUpgradeStage[] UpgradeStages
    {
        get { return upgradeStages; }
    }

    // READ ONLY (TowerUpgradeUI) - can't be negative
    public int UpgradeStage
    {
        get { return Mathf.Abs(upgradeStage); }
    }

    // READ ONLY (TowerPlacer, TowerSelectionUI) - can't be negative
    public int TowerPrice
    {
        get { return Mathf.Abs(towerPrice); }
    }

	private void Awake()
	{
		sr= GetComponent<SpriteRenderer>();
        Instantiate(cloudPS, transform.position, Quaternion.identity);
	}
	void Update()
    {
        fireCooldown -= Time.deltaTime;

        Enemy target = FindBestTarget();

        if (target != null && fireCooldown <= 0f){
            Shoot(target);
            fireCooldown = 1f / fireRate;
        }
    }

    Enemy FindBestTarget()
    {
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();

        Enemy best = null;
        float bestProgress = -1f;

        foreach (Enemy e in enemies)
        {
            float dist = Vector2.Distance(transform.position, e.transform.position);

            if (dist <= range) {
                if (e.CurrentWayPoint > bestProgress) {
                    bestProgress = e.CurrentWayPoint;
                    best = e;
                }
            }
        }
        return best;
    }

    void Shoot(Enemy target)
    {
           GameObject p = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
           Projectile pr = p.GetComponent<Projectile>();
        pr.Target = target.transform;
    }

    public void Upgrade()
    {
        TowerUpgradeStage currentUpgradeStage = upgradeStages[upgradeStage];

        range = currentUpgradeStage.Range;
        fireRate = currentUpgradeStage.FireRate;
        sr.sprite = currentUpgradeStage.Sprite;
        CoinManager.Instance.UpdateCoins(-currentUpgradeStage.Price);
        upgradeStage += 1;
		Instantiate(cloudPS, transform.position, Quaternion.identity);

	}

	private void OnMouseDown()
	{
		if(currentUI == null)
        {
            currentUI = Instantiate(towerUpgradeUIPrefab, FindObjectOfType<Canvas>().transform);
        }

        TowerUpgradeUI currentUpgradeUI = currentUI.GetComponent<TowerUpgradeUI>();
        currentUpgradeUI.Tower = this;

        currentUI.transform.position = Input.mousePosition + new Vector3(50, -50);

        if (upgradeStage >= upgradeStages.Length) return;
        currentUpgradeUI.PriceTxt.text = upgradeStages[upgradeStage].Price.ToString();

	}
}
