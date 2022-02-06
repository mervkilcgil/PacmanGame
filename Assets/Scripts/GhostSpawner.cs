using UnityEngine;
using Random = UnityEngine.Random;

public class GhostSpawner : BaseSpawner
{
    
    [SerializeField] private float spawnTime = 5f;
    [SerializeField] private SoundManager soundManager;
    

    protected override void StartSpawning()
    {
        InvokeRepeating(nameof(SpawnGhost), spawnTime, spawnTime);

    }
    private void SpawnGhost()
    {
        var ghostPrefabs = GetGhostPrefabs();

        int randomIndex = Random.Range(0, ghostPrefabs.Count);
        GameObject randomGhost = ghostPrefabs[randomIndex];
        var ghost = Instantiate(randomGhost, GetValidRandomSpawnLocation(), Quaternion.identity).GetComponent<GhostMove>();
        ghost.soundManager = soundManager;
        ghost.SetGhostSpawner(this);
    }

    private Vector2 GetValidRandomSpawnLocation()
    {
        var spawnLocations = GetGhostSpawnLocations();
        int randomIndex = Random.Range(0, spawnLocations.Count);
        return spawnLocations[randomIndex];
    }
}