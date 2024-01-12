using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class PersonalForce : MonoBehaviour
{
    public static PersonalForce main;
    public List<GameObject> personalForce;

    public int enemiesCaptured;

    private void Awake()
    {
        main = this;
        personalForce = new List<GameObject>();
    }

    public void AddToPersonalForce(GameObject _prefab)
    {
        personalForce.Add(_prefab);
        enemiesCaptured++;
    }

    public bool IsInPersonalForce(GameObject _prefab)
    {
        return personalForce.Contains(_prefab);
    }

    public int GetLengthOfPersonalForce()
    {
        return personalForce.Count;
    }

    public void RemoveFromPersonalForce()
    {
        personalForce.Clear();
        enemiesCaptured = 0;
    }
}
