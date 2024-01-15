using UnityEngine;
using UnityEngine.AI;

public class PersonalForceTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform summonPosition;
    [SerializeField] private KeyCode triggerKey = KeyCode.RightAlt;

    [Header("Attributes")]
    [SerializeField] private float timeBetweenSummons = 20.0f;
    [SerializeField] private int damgeOfPersonalArmy = 1;

    private float timeUntilFire;
    private GameObject target;

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

        if (target == null && GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
        {
            target = FindTarget();
            target.GetComponentInChildren<SpriteRenderer>().color = Color.black;
        }
        else if(target == null)
            Debug.Log("No Target");

        // AttackEnemyArmy();
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
            agent.SetDestination(target.transform.position);
        }
        //Remove the enemy from your arsenal
        PersonalForce.main.RemoveFromPersonalForce();
    }

    private GameObject FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            return null; // No enemies in the scene, return null
        }

        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    private void AttackEnemyArmy()
    {
        Debug.Log("Attacking");
    }
}
