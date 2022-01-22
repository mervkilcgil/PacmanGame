using System;
using System.Collections.Generic;
using UnityEngine;

public class DotSpawner : MonoBehaviour
{
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private List<Transform> corners;
    [SerializeField] private float spawnInterval;

    private void Start()
    {
        var startCorner = corners[0];
        var endCorner = corners[1];
        var line = (Vector2)endCorner.position - (Vector2)startCorner.position;
        Vector2 spawnPosition = startCorner.position;
        while(spawnPosition.y > corners[2].position.y)
        {
            while (spawnPosition.x < endCorner.position.x)
            {
                spawnPosition.x -=spawnInterval;
                if(CanPut(spawnPosition))
                    Instantiate(dotPrefab, spawnPosition, Quaternion.identity);
            }
            spawnPosition.y += spawnInterval;
            spawnPosition.x = startCorner.position.x;
        }
    }
    
    private bool CanPut(Vector2 pos) 
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        return !hit.collider.CompareTag("Wall") && !hit.collider.CompareTag("Dot") && !hit.collider.CompareTag("Player");
    }
}


