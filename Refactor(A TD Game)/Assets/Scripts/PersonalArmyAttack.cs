using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class PersonalArmyAttack : MonoBehaviour
{

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public GameObject FindTarget(float detectionRange)
    {
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy") && collider.gameObject != gameObject)
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = collider.gameObject;
                }
            }
        }

        return closestEnemy;
    }

    private void AttackEnemyArmy()
    {
        Debug.Log("Attacking");
    }
}
