using System;
using System.Threading.Tasks;
using Astar2DPathFinding.Mika;
using UnityEngine;
using Random = UnityEngine.Random;

public class GhostMove : MonoBehaviour, Pathfinding
{
    [SerializeField] public SoundManager soundManager;
    public GhostSpawner ghostSpawner;
    private float deltaTime = 5f;
    private float spawnTime = 5f;
    private Vector2 endPos;
    private Vector2[] pathArray;
    private int currentPathIndex;
    private float movespeed = 1;
    private Vector2 endPosition;

    public void SetGhostSpawner(GhostSpawner ghostSpawner)
    {
        this.ghostSpawner = ghostSpawner;
        SetRandomCornerTarget();
        InvokeRepeating(nameof(Move), 0f, 1f);
    }
    
    public void AddTarget(Vector2 target)
    {
        FindPath(transform, target);
    }
    private void Move()
    {
        if (pathArray == null)
        {
            return;
        }

        if (currentPathIndex < pathArray.Length - 1)
        {
            Vector2 target_pos = endPos;
            Vector2 my_pos = transform.position;
            target_pos.x = target_pos.x - my_pos.x;
            target_pos.y = target_pos.y - my_pos.y;
            float angle = Mathf.Atan2(target_pos.y, target_pos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            transform.position =
                Vector2.MoveTowards(my_pos, pathArray[currentPathIndex], Time.deltaTime*movespeed);
            currentPathIndex++;
        }
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

    public async void FindPath(Transform _seeker, Vector2 _endPos)
    {
        endPos = _endPos;

        if (_endPos != endPosition)
        {
            endPosition = _endPos;
            await SearchPathRequest(this, _seeker.position, endPosition);
        }
    }

    public void SetRandomCornerTarget()
    {
        int randomCorner = Random.Range(0, 4);

        AddTarget(ghostSpawner.Corners[randomCorner]);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.GameOver();
        }
    }

    public void OnPathFound(Vector2[] newPath)
    {
        pathArray = newPath;
    }
}