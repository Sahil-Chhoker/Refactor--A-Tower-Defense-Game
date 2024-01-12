using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CaptureTower : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    
    [Header("Attributes")]
    [SerializeField] private float targetingRange = 4f;
    [SerializeField] private float aps = 0.25f; //Attacks Per Second
    [SerializeField] private int captureSize = 5;

    private GameObject capturingParent;
    // private int enemiesCaptured = 0;
    private float timeUntilFire;

    private void Start()
    {
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

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
