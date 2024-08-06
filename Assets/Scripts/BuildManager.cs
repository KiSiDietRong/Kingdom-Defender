using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Main;
    [Header("References")]
    [SerializeField] private GameObject[] towerPrefabs;
    [SerializeField] private MoneySetting moneySetting;

    private int selectedTower = 0;

    private void Awake()
    {
        Main = this;
    }
    public GameObject GetSelectedTower()
    {
        return towerPrefabs[selectedTower];
    }
    public bool TrySpendMoney(int amount)
    {
        return moneySetting.TrySpendMoney(amount);
    }
}
