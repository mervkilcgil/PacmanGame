using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private List<GameObject> ghostPrefabs;
    [SerializeField] private List<Transform> corners;
    private float ghostSpawnInterval = 100f;
    
    public List<Transform> Corners => corners;

    public Vector2 GetFirstCorner()
    {
        return corners[0].position;
    }
    
    public Vector2 GetSecondCorner()
    {
        return corners[1].position;
    }
    
    public Vector2 GetThirdCorner()
    {
        return corners[2].position;
    }
    
    public Vector2 GetFourthCorner()
    {
        return corners[3].position;
    }
    
    public GameObject GetDotPrefab()
    {
        return dotPrefab;
    }
    
    public List<GameObject> GetGhostPrefabs()
    {
        return ghostPrefabs;
    }

    public List<Vector2> GetGhostSpawnLocations()
    {
        List<Vector2> ghostSpawnLocations = new List<Vector2>();
        Vector2 spawnPosition = GetFirstCorner();
        while(spawnPosition.y > GetThirdCorner().y && spawnPosition.y <= GetSecondCorner().y)
        {
            while (spawnPosition.x > GetSecondCorner().x && spawnPosition.x <= GetFirstCorner().x)
            {
                if(CanPut(spawnPosition))
                {
                    ghostSpawnLocations.Add(spawnPosition);
                }
                spawnPosition.x -=ghostSpawnInterval;
            }
            spawnPosition.y -= ghostSpawnInterval;
            spawnPosition.x = GetFirstCorner().x;
        }

        return ghostSpawnLocations;
    }
    
    private bool CanPut(Vector2 pos) 
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit.collider == null || hit.collider.CompareTag("Dot"))
            return true;
        return !hit.collider.CompareTag("Wall") && !hit.collider.CompareTag("Ghost") && !hit.collider.CompareTag("Player");
    }
}