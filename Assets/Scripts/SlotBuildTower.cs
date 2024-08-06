using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotBuildTower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;
    [SerializeField] private int towerCost = 70;

    private GameObject tower;
    private Color startColor;

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

    private void OnMouseDown()
    {
        if (tower != null) return;

        if (BuildManager.Main.TrySpendMoney(towerCost))
        {
            GameObject towerToBuild = BuildManager.Main.GetSelectedTower();
            tower = Instantiate(towerToBuild, transform.position, Quaternion.identity);
        }
        else
        {
            StartCoroutine(FlashInsufficientFunds());
        }
    }
    private IEnumerator FlashInsufficientFunds()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        sr.color = startColor;
    }
}
