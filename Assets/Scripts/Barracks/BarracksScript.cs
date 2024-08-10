using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarracksScript : MonoBehaviour
{
    public GameObject soldierPrefab;
    public Transform[] spawnPoints;
    public int soldierCount = 3;

    private void Start()
    {
        StartCoroutine(SpawnSoldiers());
    }

    private IEnumerator SpawnSoldiers()
    {
        for (int i = 0; i < soldierCount; i++)
        {
            Transform spawnPoint = spawnPoints[i % spawnPoints.Length];
            GameObject soldier = Instantiate(soldierPrefab, spawnPoint.position, spawnPoint.rotation);
            SoldierScript soldierScript = soldier.GetComponent<SoldierScript>();
            soldierScript.SetSpawnPoint(spawnPoint.position);
            yield return new WaitForSeconds(2f);
        }
    }
}
