using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotBuildTower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;
    [SerializeField] private TowerSelectionUI towerSelectionUI;
    [SerializeField] private MoneySetting moneySetting;

    private bool isOccupied = false;
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
        if (!isOccupied)
        {
            towerSelectionUI.ShowTowerSelection();
            towerSelectionUI.OnTowerSelected += BuildTower;
        }
    }
    private void BuildTower(int towerIndex)
    {
        if (!isOccupied)
        {
            if (BuildManager.Main.TrySpendMoney(moneySetting.GetTowerCost(towerIndex)))
            {
                GameObject towerToBuild = BuildManager.Main.GetSelectedTower(towerIndex);
                tower = Instantiate(towerToBuild, transform.position, Quaternion.identity);
                towerSelectionUI.HideTowerSelection();
                isOccupied = true;
                moneySetting.ResetMoneyTextColor();
            }
            else
            {
                StartCoroutine(FlashInsufficientFunds());
            }
        }
    }
    private IEnumerator FlashInsufficientFunds()
    {
        moneySetting.SetMoneyTextColor(Color.red);
        sr.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        sr.color = startColor;

        moneySetting.ResetMoneyTextColor();
    }
}
