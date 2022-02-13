using UnityEngine;
using Random = UnityEngine.Random;

public class GhostSpawner : BaseSpawner
{
    [SerializeField] public Transform rightTunnel, leftTunnel;
    private int maxGhostCount = 4;
    private int ghostIndex;

    protected override void StartSpawning()
    {
        SpawnGhost();
    }
    private void SpawnGhost()
    {
        for (int i = 0; i < maxGhostCount; i++)
        {
            var ghostPrefabs = GetGhostPrefabs();
            GameObject randomGhost = ghostPrefabs[i];
            var ghost = Instantiate(randomGhost, transform.position, Quaternion.identity, transform).GetComponent<GhostMove>();
            ghost.SetGhostSpawner(this);
        }
        
    }
    private void FixedUpdate()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Space))
        {
            if(Input.GetKeyDown(KeyCode.D))
                GameManager.Instance.OnDeath();
        }
        
#endif
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}