using UnityEngine;
using Random = UnityEngine.Random;

public class GhostSpawner : BaseSpawner
{
    
    [SerializeField] private float spawnTime = 5f;

    private float deltaTime;
    private void Start()
    {
        deltaTime = 0f;
    }

    private void FixedUpdate()
    {
        if(GameManager.Instance.GameState == GameState.Playing)
        {
            deltaTime += Time.fixedDeltaTime;
            if (deltaTime >= spawnTime)
            {
                deltaTime = 0f;
                SpawnGhost();
            }
        }
    }

    private void SpawnGhost()
    {
        var ghostPrefabs = GetGhostPrefabs();

        int randomIndex = Random.Range(0, ghostPrefabs.Count);
        GameObject randomGhost = ghostPrefabs[randomIndex];
        Instantiate(randomGhost, GetValidRandomSpawnLocation(), Quaternion.identity);
        
        deltaTime = 0f;
    }

    private Vector2 GetValidRandomSpawnLocation()
    {
        var spawnLocations =GetGhostSpawnLocations();
        int randomIndex = Random.Range(0, spawnLocations.Count);
        return spawnLocations[randomIndex];
    }
}