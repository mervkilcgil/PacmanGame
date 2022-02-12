using UnityEngine;

public class DotSpawner : BaseSpawner
{
    private float spawnInterval = 20;
    [SerializeField] private BoxCollider2D[] paths;

    protected override void StartSpawning()
    {
        foreach (var path in paths)
        {
            Vector2 direction = path.size.x > path.size.y ? Vector2.right : Vector2.up;
            Vector2 pathVector = Vector2.zero;
            if(direction == Vector2.right)
            {
                float p1 = path.bounds.center.x + path.size.x / 2f;
                float p2 = path.bounds.center.x - path.size.x / 2f;
                while (p2 < p1)
                {
                    InstantiateDot(new Vector2(p2, path.bounds.center.y));
                    p2 += spawnInterval;
                }
            }
            else
            {
                float p1 = path.bounds.center.y + path.size.y / 2f;
                float p2 = path.bounds.center.y - path.size.y / 2f;
                while (p2 < p1)
                {
                    InstantiateDot(new Vector2(path.bounds.center.x, p2));
                    p2 += spawnInterval;
                }
            }

        }
    }
    
    private void InstantiateDot(Vector2 spawnPosition)
    {
        Dot dot = Instantiate(GetDotPrefab(), spawnPosition, Quaternion.identity, transform).GetComponent<Dot>();
        dot.IncreaseScore += ()=>GameManager.Instance.EatPellet();
        GameManager.Instance.DotCount++;
    }
    private bool CanPut(Vector2 pos) 
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit.collider == null)
            return true;
        return !hit.collider.CompareTag("Wall") && !hit.collider.CompareTag("Dot") && !hit.collider.CompareTag("Player");
    }
    
}


