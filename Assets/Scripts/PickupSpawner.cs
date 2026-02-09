using UnityEngine;
using UnityEngine.AI;
public class PickupSpawner : MonoBehaviour
{
    public GameObject pickupPrefab;
    public int spawnCount = 8; 
    public float spawnRange = 20f; 

    void Start()
    {
        SpawnPickups();
    }

    void SpawnPickups()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * spawnRange;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                Instantiate(pickupPrefab, hit.position, Quaternion.identity);
            }
            else
            {
                i--;
            }
        }
    }
}