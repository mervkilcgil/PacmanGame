using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private List<GameObject> ghostPrefabs;
    [SerializeField] private List<Transform> corners;
    private float ghostSpawnInterval = 100f;
    
    public List<Transform> Corners => corners;
    
    public void Start()
    {
        GameManager.Instance.OnStartGame += StartSpawning;
    }
    protected virtual void StartSpawning()
    {
        
    }
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
        
        Vector2 spawnPosition = GetPath(GetFirstCorner(), GetThirdCorner());
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
    
    protected bool CanPut(Vector2 pos) 
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit.collider == null || hit.collider.CompareTag("Dot"))
            return true;
        return !hit.collider.CompareTag("Wall") && !hit.collider.CompareTag("Ghost") && !hit.collider.CompareTag("Player");
    }
    
    protected Vector2 GetPath(Vector2 p1, Vector2 p2)
    {
        Vector2 path = p2 - p1;
        
        return GetPathAlong(p1, path.normalized);
    }
    
    protected Vector2 GetPathAlong(Vector2 path, Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(path, dir.PerpendicularClockwise().normalized);
        Debug.DrawRay(path, dir.PerpendicularClockwise(), Color.red, Mathf.Infinity, false);
        Vector2 upperWall = hit ? hit.point : path;
        RaycastHit2D hit2 = Physics2D.Raycast(path, dir.PerpendicularCounterClockwise().normalized);
        Debug.DrawRay(path, dir.PerpendicularCounterClockwise(), Color.yellow, Mathf.Infinity, false);
        Vector2 lowerWall = hit2 ? hit2.point : path;
        
        return upperWall.GetMiddlePoint(lowerWall);
    }

    protected Vector2 GetClosestColliderPosition(Vector2 position, Vector2 direction)
    {
        float minDist = Mathf.Infinity;
        Vector2 minPoint = Vector2.zero;
        RaycastHit2D[] hits = new RaycastHit2D[100];
        Physics2D.RaycastNonAlloc(position, direction, hits, Mathf.Infinity);
        foreach (var hit in hits.ToList().FindAll(i=> (i.collider != null && i.collider.CompareTag("Wall"))))
        {
            float dist = Vector2.Distance(hit.point, position);
            if (dist < minDist)
            {
                minPoint = hit.point;
                minDist = dist;
            }
            
        }

        return minPoint;
    }
    
    
}