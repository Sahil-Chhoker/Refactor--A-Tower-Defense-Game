using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
        foreach(GameObject _prefabToSummon in PersonalForce.main.personalForce)
        {
            //Reset thier positions as ours
            _prefabToSummon.transform.position = summonPosition.position;
            _prefabToSummon.transform.parent = summonPosition;

            //Set them active
            _prefabToSummon.SetActive(true);
            Destroy(_prefabToSummon.GetComponent<EnemyManager>());

            //Reset thier health
            Health healthScript = _prefabToSummon.GetComponent<Health>();
            healthScript.hitPoints = healthScript.baseHitPoints;
        }
        //Remove the enemy from your arsenal
        PersonalForce.main.RemoveFromPersonalForce();
    }

}
