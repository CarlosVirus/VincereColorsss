using UnityEngine;
using UnityEngine.UI;

public class UnitSpawner : MonoBehaviour
{
    public GameObject unitPrefab;
    public Transform spawnPoint;
    public Button spawnButton;

    public float spawnDelay = 1f;
    private bool canSpawn = true;

    private void Start()
    {
        spawnButton.onClick.AddListener(SpawnUnit);
    }

    void SpawnUnit()
    {
        if (!canSpawn) return;
        StartCoroutine(SpawnRoutine());
    }

    System.Collections.IEnumerator SpawnRoutine()
    {
        canSpawn = false;
        Instantiate(unitPrefab, spawnPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(spawnDelay);
        canSpawn = true;
    }
}
