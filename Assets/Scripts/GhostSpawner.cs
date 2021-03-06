using Astar2DPathFinding.Mika;
using UnityEngine;

public class GhostSpawner : BaseSpawner
{
    [SerializeField] private PathfindingGrid grid;
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
            Vector2 pos = transform.position;
            pos.x = pos.x + (i - 2) * 6f;
            var ghost = Instantiate(randomGhost, pos, Quaternion.identity, transform).GetComponent<GhostMove>();
            ghost.SetGhostSpawner(this, grid, i*5f);
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