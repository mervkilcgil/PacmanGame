using UnityEngine;

public class DotSpawner : BaseSpawner
{
    private float spawnInterval = 28f;

    private void Start()
    {
        GameManager.Instance.OnStartGame += StartSpawning;
    }

    private void StartSpawning()
    {
        Vector2 spawnPosition = GetPath(GetFirstCorner(), GetThirdCorner());
        while(spawnPosition.y > GetThirdCorner().y && spawnPosition.y <= GetSecondCorner().y)
        {
            while (spawnPosition.x > GetSecondCorner().x && spawnPosition.x <= GetFirstCorner().x)
            {
                if(CanPut(spawnPosition))
                {
                    Dot dot = Instantiate(GetDotPrefab(), spawnPosition, Quaternion.identity, transform).GetComponent<Dot>();
                    dot.IncreaseScore += GameManager.Instance.IncreaseScore;
                    GameManager.Instance.DotCount++;
                }
                spawnPosition.x -=spawnInterval;
            }
            spawnPosition.y -= spawnInterval;
            spawnPosition.x = GetFirstCorner().x;
            spawnPosition = GetPathAlong(spawnPosition, spawnPosition.normalized);
        }
        Debug.Log(GameManager.Instance.DotCount);
    }
    
    private bool CanPut(Vector2 pos) 
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit.collider == null)
            return true;
        return !hit.collider.CompareTag("Wall") && !hit.collider.CompareTag("Dot") && !hit.collider.CompareTag("Player");
    }
    
}


