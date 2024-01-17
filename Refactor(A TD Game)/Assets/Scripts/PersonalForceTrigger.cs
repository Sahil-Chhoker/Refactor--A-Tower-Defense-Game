using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class PersonalForceTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PersonalArmyAttack _personalArmyScript;
    [SerializeField] private Transform summonPosition;
    [SerializeField] private KeyCode triggerKey = KeyCode.RightAlt;

    [Header("Attributes")]
    [SerializeField] private float timeBetweenSummons = 20.0f;
    [SerializeField] private int damgeOfPersonalArmy = 1;
    [SerializeField] private float detectionRange = 10f;

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
            target = _personalArmyScript.FindTarget(detectionRange);
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

            //Give the Logic to attack
            summonedPrefab.AddComponent<PersonalArmyAttack>();
        }
        //Remove the enemy from your arsenal
        PersonalForce.main.RemoveFromPersonalForce();
    }

    
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, detectionRange);
    }

}
