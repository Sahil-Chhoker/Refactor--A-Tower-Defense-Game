using System.Text.RegularExpressions;
using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    public GameObject towerObj;
    public Turret _turretScript;
    public CaptureTower _captureTowerScript;
    private Color startColor;


    private void Start()
    {
        startColor = sr.color;
    }

    private void OnMouseEnter()
    {
        if(UIManager.main.IsHoveringUI())   return;
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        if(UIManager.main.IsHoveringUI()) return;
        
        if(towerObj != null && towerObj.CompareTag("NormalTurret"))
        {
            _turretScript.OpenUpgradeUI();
            return;
        }
        else if(towerObj != null && towerObj.CompareTag("CaptureTurret"))
        {
            _captureTowerScript.OpenUpgradeUI();
            return;
        }

        Tower towerToBuild = BuildManager.main.GetSelectedTower();

        if(towerToBuild.cost > LevelManager.main.currency)
        {
            Debug.Log("Not Enough Money");
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);
        towerObj = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
        _turretScript = towerObj.GetComponent<Turret>();
        _captureTowerScript = towerObj.GetComponent<CaptureTower>();
    }

}
