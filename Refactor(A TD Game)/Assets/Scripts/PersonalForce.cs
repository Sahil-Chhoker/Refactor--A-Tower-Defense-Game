using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class PersonalForce : MonoBehaviour
{
    public static PersonalForce main;

    private int captureSize;
    public List<GameObject> personalForce; // Use List instead of an array

    private void Awake()
    {
        main = this;
        personalForce = new List<GameObject>();
    }

    public void AddToPersonalForce(GameObject _prefab)
    {
        personalForce.Add(_prefab);
    }

    public bool IsInPersonalForce(GameObject _prefab)
    {
        return personalForce.Contains(_prefab);
    }

    public void SetSize(int _captureSize)
    {
        captureSize = _captureSize;
    }
}
