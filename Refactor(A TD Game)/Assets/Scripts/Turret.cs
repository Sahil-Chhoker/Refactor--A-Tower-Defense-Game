using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotatePoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 4f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bps = 1f; //Bullets Per Second
    [SerializeField] private int baseUpgradeCost = 100;
    [SerializeField] private float upgradeBPSScale;
    [SerializeField] private float upgradeRangeScale;
    [SerializeField] private float upgradeCostScale;

    private float bpsBase;
    private float targetingRangeBase;

    private Transform target;
    private float timeUntilFire;

    private int level = 1;

    private void Start()
    {
        bpsBase = bps;
        targetingRangeBase = targetingRange;

        upgradeButton.onClick.AddListener(Upgrade);
    }

    private void Update()
    {
        if (target == null || !target.gameObject.activeSelf)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if(!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;

            if(timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0.0f;
            }
        }
    }

    private void FindTarget()
    {   
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);

        foreach (Collider2D collider in colliders)
        {
            if (!collider.gameObject.CompareTag("CapturedArmy"))
            {
                target = collider.transform;
                return;
            }
        }

        // If no active targets found, set target to null
        target = null;
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
        turretRotatePoint.rotation = Quaternion.RotateTowards(turretRotatePoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    public void Upgrade()
    {
        if(CalculateCost() > LevelManager.main.currency) return;

        LevelManager.main.SpendCurrency(CalculateCost());
        level++;

        bps = CalculateBPS();
        targetingRange = CalculateRange();

        CloseUpgradeUI();
    }

    private float CalculateBPS()
    {
        return bps * Mathf.Pow(level, upgradeBPSScale);
    }

    private float CalculateRange()
    {
        return targetingRangeBase * Mathf.Pow(level, upgradeRangeScale);
    }

    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, upgradeCostScale));
    }

    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }

}
