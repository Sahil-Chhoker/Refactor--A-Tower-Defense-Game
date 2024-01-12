using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] public int hitPoints = 2;
    [SerializeField] private int currencyWorth = 50;

    [HideInInspector] public int baseHitPoints;
    private bool isDestroyed = false;

    private void Start()
    {
        baseHitPoints = hitPoints;
    }

    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;

        if(hitPoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(currencyWorth);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
}
