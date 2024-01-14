using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class CaptureTower : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;
    
    [Header("Attributes")]
    [SerializeField] private float targetingRange = 4f;
    [SerializeField] private float aps = 0.25f; //Attacks Per Second
    [SerializeField] private int captureSize = 5;
    [SerializeField] private int baseUpgradeCost = 100;
    [SerializeField] private float upgradeAPSScale;
    [SerializeField] private float upgradeRangeScale;
    [SerializeField] private float upgradeCostScale;
    [SerializeField] private float upgradeCaptureSizeScale;

    private float apsBase;
    private float targetingRangeBase;
    private float captureSizeBase;

    private GameObject capturingParent;
    private float timeUntilFire;

    private int level = 1;

    private void Start()
    {
        apsBase = aps;
        targetingRangeBase = targetingRange;
        captureSizeBase = captureSize;
        
        upgradeButton.onClick.AddListener(Upgrade);
        
        capturingParent = GameObject.FindGameObjectWithTag("CapturedArmy");
    }

    private void Update()
    {
        timeUntilFire += Time.deltaTime;

        if(timeUntilFire >= 1.0f / aps)
        {
            CaptureEnemies();
            timeUntilFire = 0.0f;
        }
    }

    private void CaptureEnemies()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);

        foreach (RaycastHit2D hit in hits)
        {
            if (PersonalForce.main.enemiesCaptured >= captureSize)
            {
                break; // Break if the required number of enemies are already captured
            }

            Health healthScript = hit.transform.GetComponent<Health>();
            if (healthScript != null && healthScript.hitPoints > 1)
            {
                continue; // Skip if the enemy has more than 1 hit point
            }

            GameObject currEnemy = hit.transform.gameObject;

            // Check if the enemy is already captured
            if (PersonalForce.main.IsInPersonalForce(currEnemy))
            {
                continue;
            }

            // Set the capturing parent as the parent of the captured enemy
            currEnemy.transform.parent = capturingParent.transform;
            currEnemy.tag = "CapturedArmy";

            // Deactivate the enemy immediately after capturing
            currEnemy.SetActive(false);

            EnemySpawner.onEnemyDestroy.Invoke();

            PersonalForce.main.AddToPersonalForce(currEnemy);
        }
    }

    public void Upgrade()
    {
        if(CalculateCost() > LevelManager.main.currency) return;

        LevelManager.main.SpendCurrency(CalculateCost());
        level++;

        aps = CalculateAPS();
        targetingRange = CalculateRange();
        captureSize = CalculateCaptureSize();

        CloseUpgradeUI();
    }

    private float CalculateAPS()
    {
        return aps * Mathf.Pow(level, upgradeAPSScale);
    }

    private float CalculateRange()
    {
        return targetingRangeBase * Mathf.Pow(level, upgradeRangeScale);
    }

    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, upgradeCostScale));
    }

    private int CalculateCaptureSize()
    {
        return Mathf.RoundToInt(captureSizeBase * Mathf.Pow(level, upgradeCaptureSizeScale));
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
