using System;
using System.Collections.Generic;
using UnityEngine;

public class DotSpawner : MonoBehaviour
{
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private List<Transform> corners;
    private float spawnInterval = 28f;

    private void Start()
    {
        Vector2 spawnPosition = corners[0].position;
        while(spawnPosition.y > corners[2].position.y && spawnPosition.y <= corners[1].position.y)
        {
            while (spawnPosition.x > corners[1].position.x && spawnPosition.x <= corners[0].position.x)
            {
                if(CanPut(spawnPosition))
                {
                    Dot dot = Instantiate(dotPrefab, spawnPosition, Quaternion.identity, transform).GetComponent<Dot>();
                    dot.IncreaseScore += GameManager.Instance.IncreaseScore;
                }
                spawnPosition.x -=spawnInterval;
            }
            spawnPosition.y -= spawnInterval;
            spawnPosition.x = corners[0].position.x;
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


