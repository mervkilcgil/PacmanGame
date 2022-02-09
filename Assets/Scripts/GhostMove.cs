using System;
using System.Threading.Tasks;
using Astar2DPathFinding.Mika;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;

public class GhostMove : MonoBehaviour, Pathfinding
{
    public GhostSpawner ghostSpawner;
    private float deltaTime = 5f;
    private float spawnTime = 5f;
    private float movespeed = 25;
    private Vector2 endPosition;
    private IEnumerator currentPath;
    private int currentTargetCorner = -1;

    public void SetGhostSpawner(GhostSpawner ghostSpawner)
    {
        this.ghostSpawner = ghostSpawner;
        GameManager.Instance.OnRestartGame += ResetGhost;
        SetRandomCornerTarget();
    }

    private void ResetGhost()
    {
        Destroy(this.gameObject);
    }

    private void AddTarget(Vector2 target)
    {
        FindPath(transform, target);
    }

    private IEnumerator MovePath(Vector2[] pathArray)
    {
        if (pathArray == null || GameManager.Instance.GameState != GameState.Playing)
        {
            yield break;
        }
        
        for (int j = 0; j < pathArray.Length - 1; j++)
        {
            Debug.DrawLine(pathArray[j], pathArray[j + 1], Color.white,
                10);
        }
        for (int i = 0; i < pathArray.Length; i++)
        {
            while ((Vector2)transform.position != pathArray[i])
            {
                Vector2 target_pos = pathArray[i];
                var speed = Time.deltaTime * movespeed;
                transform.position = Vector2.MoveTowards(transform.position, target_pos, speed);
                transform.rotation = Quaternion.identity;
                yield return null;
            }
        }
        SetRandomCornerTarget();
    }

    private Task<Vector3[]> SearchPathRequest(Pathfinding requester, Vector2 startPos, Vector2 endPos)
    {
        var taskCompletionSource = new TaskCompletionSource<Vector3[]>();
        Node start = PathfindingGrid.Instance.NodeFromWorldPoint(startPos);
        Node end = PathfindingGrid.Instance.ClosestNodeFromWorldPoint(endPos, start.gridAreaID);
        Vector2[] newPath = null;
        newPath = AStar.FindPath(start, end);
        requester.OnPathFound(newPath);
        return taskCompletionSource.Task;
    }

    private async void FindPath(Transform _seeker, Vector2 _endPos)
    {
        if (_endPos != endPosition)
        {
            endPosition = _endPos;
            await SearchPathRequest(this, _seeker.position, endPosition);
        }
    }

    private void SetRandomCornerTarget()
    {
        List<Vector2> targets = new List<Vector2>(ghostSpawner.Corners.Count + 1);
        ghostSpawner.Corners.ForEach(i=> targets.Add(i));
        targets.Add(GameManager.Instance.PlayerPosition);
        int randomCorner = Random.Range(1, 5);
        while (currentTargetCorner == randomCorner)
        {
            randomCorner = Random.Range(1, 5);
        }
        currentTargetCorner = randomCorner;
        AddTarget(targets[currentTargetCorner]);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (GameManager.Instance.GameState == GameState.Playing)
        {

            if (other.gameObject.CompareTag("Player"))
            {
                var player = other.gameObject.GetComponent<Player>();
                if (player.CanEat(transform.position))
                {
                    SoundManager.Instance.PlayEatGhost();
                    Destroy(gameObject);
                }
                else
                {
                    GameManager.Instance.OnDeath();
                }
            }
        }
    }

    public void OnPathFound(Vector2[] newPath)
    {
        if (currentPath != null)
        {
            StopCoroutine(currentPath);
        }

        currentPath = MovePath(newPath);
        StartCoroutine(currentPath);
    }

    public void OnDestroy()
    {
        GameManager.Instance.OnRestartGame -= ResetGhost;
    }
}