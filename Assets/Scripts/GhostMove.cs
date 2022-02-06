using System.Threading.Tasks;
using Astar2DPathFinding.Mika;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;

public class GhostMove : MonoBehaviour, Pathfinding
{
    [SerializeField] public SoundManager soundManager;
    public GhostSpawner ghostSpawner;
    private float deltaTime = 5f;
    private float spawnTime = 5f;
    private Vector2 endPos;
    private Vector2[] pathArray;
    private int currentPathIndex;
    private float movespeed = 10;
    private Vector2 endPosition;
    private IEnumerator currentPath;

    public void SetGhostSpawner(GhostSpawner ghostSpawner)
    {
        this.ghostSpawner = ghostSpawner;
        SetRandomCornerTarget();
    }

    private void AddTarget(Vector2 target)
    {
        FindPath(transform, target);
    }

    private IEnumerator MovePath(Vector2[] pathArray)
    {
        if (pathArray == null)
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
                yield return null;
            }
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

    private async void FindPath(Transform _seeker, Vector2 _endPos)
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
        if (currentPath != null)
        {
            StopCoroutine(currentPath);
        }

        currentPath = MovePath(newPath);
        pathArray = newPath;
        StartCoroutine(currentPath);
    }
}