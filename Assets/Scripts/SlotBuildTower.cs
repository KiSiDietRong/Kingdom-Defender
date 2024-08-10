using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotBuildTower : MonoBehaviour
{
    /*[Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject tower;
    private Color startColor;
    public GameObject Shop;
    private void Start()
    {
        startColor = sr.color;
    }

    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    //private void OnMouseDown()
    {
        if (Shop == true)
        {
            Shop.SetActive(false);
        }
        
            Shop.SetActive(true);
        
        // towerToBuild = BuildManager.Main.GetSelectedTower();
        //tower = Instantiate(towerToBuild, transform.position, Quaternion.identity);

    }

    private void OnMouseDown()
    {
        // Toggle the active state of the Shop
        if (Shop.activeSelf)
        {
            Shop.SetActive(false); // Turn off the shop if it is on
        }
        else
        {
            Shop.SetActive(true); // Turn on the shop if it is off
        }

        // Optionally, you can turn off the SpriteRenderer (circle) if needed
        // sr.enabled = false;

        // towerToBuild = BuildManager.Main.GetSelectedTower();
        // tower = Instantiate(towerToBuild, transform.position, Quaternion.identity);
    }

}*/
    public GameObject selectionCanvas;

    void OnMouseDown()
    {
        selectionCanvas.SetActive(true);
        turretSelection.instance.SetBuildSpot(this);
    }

    public void BuildTurret(GameObject turretPrefab)
    {
        Instantiate(turretPrefab, transform.position, Quaternion.identity);
        selectionCanvas.SetActive(false);
    }
}
