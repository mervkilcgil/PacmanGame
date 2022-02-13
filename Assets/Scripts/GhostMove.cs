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
    private float movespeed = 60;
    private Vector2 endPosition;
    private IEnumerator currentPath;
    private int currentTargetCorner = -1;
    private Vector2 tunnelDir;
    private Vector2 direction;
    private Vector2 startPos, currentPos;

    public void SetGhostSpawner(GhostSpawner ghostSpawner)
    {
        this.ghostSpawner = ghostSpawner;
        GameManager.Instance.OnEndGame += DestroyGhost;
        GameManager.Instance.OnRestartGame += ResetGhost;
        tunnelDir = Vector2.zero;
        startPos = new Vector2(transform.position.x, transform.position.y);
        currentPos = startPos;

        SetRandomCornerTarget();
    }

    private void ResetGhost()
    {
        transform.position = startPos;
        currentPos = startPos;
        gameObject.SetActive(true);
        StopCoroutine(currentPath);
        SetRandomCornerTarget();
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
                currentPos = new Vector2(transform.position.x, transform.position.y);
                var speed = Time.deltaTime * movespeed;
                direction = (target_pos - (Vector2)transform.position).normalized;
                transform.position = Vector2.MoveTowards(currentPos, target_pos, speed);
                transform.rotation = Quaternion.identity;
                yield return null;
            }
        }
        
        SetRandomCornerTarget();
    }

    private Task<Vector3[]> SearchPathRequest(Pathfinding requester, Vector2 _startPos, Vector2 endPos)
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
            await SearchPathRequest(this, startPos, endPosition);
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
                    DestroyGhost();
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

    public void DestroyGhost()
    {
        gameObject.SetActive(false);
        ResetGhost();
    }
    
    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (direction == tunnelDir) return;
        if (other.gameObject.CompareTag("RightTunnel"))
        {
            transform.position = ghostSpawner.leftTunnel.position;
            tunnelDir = direction;
        }
        else if(other.gameObject.CompareTag("LeftTunnel"))
        {
            transform.position = ghostSpawner.rightTunnel.position;
            tunnelDir = direction;
        }
        
    
    }*/
    private void FixedUpdate()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Space))
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                DestroyGhost();
            }
        } 
#endif
    }
}