using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianTower : MonoBehaviour
{
    public GameObject soldierPrefab; // Prefab của lính phòng thủ
    public Transform[] spawnPoints; // Các vị trí sinh ra lính phòng thủ
    [SerializeField] private int soldierCount; // Số lượng lính phòng thủ cần sinh ra

    private void Start()
    {
        StartCoroutine(SpawnSoldiers());
    }

    private IEnumerator SpawnSoldiers()
    {
        for (int i = 0; i < soldierCount; i++)
        {
            // Chọn một vị trí sinh ra ngẫu nhiên từ mảng spawnPoints
            Transform spawnPoint = spawnPoints[i % spawnPoints.Length];
            Instantiate(soldierPrefab, spawnPoint.position, spawnPoint.rotation);
            yield return new WaitForSeconds(2f); // Thời gian giữa các lần sinh ra lính
        }
    }
}
