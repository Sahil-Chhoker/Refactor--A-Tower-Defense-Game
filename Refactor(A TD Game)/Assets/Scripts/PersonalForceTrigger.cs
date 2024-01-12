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
            Debug.Log("Can Summon");
            if(Input.GetKeyDown(triggerKey))
            {
                SummonArmy();
                timeUntilFire = 0.0f;
            }
        }
    }

    private void SummonArmy()
    {
        int sizeOfArmy = PersonalForce.main.GetLengthOfPersonalForce();

        foreach(GameObject _prefabToSummon in PersonalForce.main.personalForce)
        {
            //Reset thier positions as ours
            _prefabToSummon.transform.position = summonPosition.position;
            _prefabToSummon.transform.parent = summonPosition;

            _prefabToSummon.SetActive(true);
            Destroy(_prefabToSummon.GetComponent<EnemyManager>());
            // summonedPrefab.AddComponent<>
        }
        //Remove the enemy from your arsenal
        PersonalForce.main.RemoveFromPersonalForce();
    }

}
