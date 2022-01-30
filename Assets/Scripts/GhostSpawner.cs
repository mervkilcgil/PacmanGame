using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GhostSpawner : BaseSpawner
{
    
    [SerializeField] private float spawnTime = 5f;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private Transform player;

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
        var ghost = Instantiate(randomGhost, GetValidRandomSpawnLocation(), Quaternion.identity).GetComponent<GhostMove>();
        ghost.soundManager = soundManager;
        ghost.SetPlayer(player);
        
        deltaTime = 0f;
    }

    private Vector2 GetValidRandomSpawnLocation()
    {
        var spawnLocations = GetGhostSpawnLocations();
        int randomIndex = Random.Range(0, spawnLocations.Count);
        return spawnLocations[randomIndex];
    }
}