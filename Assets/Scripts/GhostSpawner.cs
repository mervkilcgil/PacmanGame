using UnityEngine;
using Random = UnityEngine.Random;

public class GhostSpawner : BaseSpawner
{
    
    [SerializeField] private float spawnTime = 5f;
    [SerializeField] private SoundManager soundManager;
    

    protected override void StartSpawning()
    {
        InvokeRepeating(nameof(SpawnGhost), 0, spawnTime);

    }
    private void SpawnGhost()
    {
        var ghostPrefabs = GetGhostPrefabs();

        int randomIndex = Random.Range(0, ghostPrefabs.Count);
        GameObject randomGhost = ghostPrefabs[randomIndex];
        var ghost = Instantiate(randomGhost, transform.position, Quaternion.identity, transform).GetComponent<GhostMove>();
        ghost.soundManager = soundManager;
        ghost.SetGhostSpawner(this);
    }
}