using System.Collections.Generic;
using UnityEngine;

public class DotSpawner : MonoBehaviour
{
    [SerializeField] private GameConstants gameConstants;
    private float spawnInterval = 28f;

    private void Start()
    {
        GameManager.Instance.OnStartGame += StartSpawning;
    }

    private void StartSpawning()
    {
        Vector2 spawnPosition = GameConstants.Instance.GetFirstCorner();
        while(spawnPosition.y > GameConstants.Instance.GetThirdCorner().y && spawnPosition.y <= GameConstants.Instance.GetSecondCorner().y)
        {
            while (spawnPosition.x > GameConstants.Instance.GetSecondCorner().x && spawnPosition.x <= GameConstants.Instance.GetFirstCorner().x)
            {
                if(CanPut(spawnPosition))
                {
                    Dot dot = Instantiate(GameConstants.Instance.GetDotPrefab(), spawnPosition, Quaternion.identity, transform).GetComponent<Dot>();
                    dot.IncreaseScore += GameManager.Instance.IncreaseScore;
                }
                spawnPosition.x -=spawnInterval;
            }
            spawnPosition.y -= spawnInterval;
            spawnPosition.x = GameConstants.Instance.GetFirstCorner().x;
        }
    }
    
    private bool CanPut(Vector2 pos) 
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit.collider == null)
            return true;
        return !hit.collider.CompareTag("Wall") && !hit.collider.CompareTag("Dot") && !hit.collider.CompareTag("Player");
    }
}


