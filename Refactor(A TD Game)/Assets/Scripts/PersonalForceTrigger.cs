using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PersonalForceTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform summonPosition;
    [SerializeField] private KeyCode triggerKey = KeyCode.RightAlt;

    [Header("Attributes")]
    [SerializeField] private float timeBetweenSummons = 20.0f;

    private float timeUntilFire;

    private void Update()
    {
        timeUntilFire += Time.deltaTime;

        if(timeUntilFire >= timeBetweenSummons)
        {
            if(Input.GetKeyDown(triggerKey))
            {
                SummonArmy();
                timeUntilFire = 0.0f;
            }
        }
    }

    private void SummonArmy()
    {
        foreach(GameObject summonedPrefab in PersonalForce.main.personalForce)
        {
            //Reset thier positions as ours
            summonedPrefab.transform.position = summonPosition.position;
            summonedPrefab.transform.parent = summonPosition;

            //Set them active
            summonedPrefab.SetActive(true);
            Destroy(summonedPrefab.GetComponent<EnemyManager>());

            //Reset thier health
            Health healthScript = summonedPrefab.GetComponent<Health>();
            healthScript.hitPoints = healthScript.baseHitPoints;

            //Add a new Logic
            NavMeshAgent agent = summonedPrefab.AddComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.SetDestination(FindTarget());
        }
        //Remove the enemy from your arsenal
        PersonalForce.main.RemoveFromPersonalForce();
    }

    private Vector3 FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach(GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy.transform.position;
    }

}
