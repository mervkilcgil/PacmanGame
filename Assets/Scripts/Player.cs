using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MoveController
{
    [SerializeField] protected Rigidbody2D rigidbody;
    [SerializeField] protected Animator animator;
    private Vector2 moveDir;
    private Vector2 startPos;
    [SerializeField] private Transform rightTunnel, leftTunnel;
    [SerializeField] private List<Transform> corners;
    private Vector2 tunnelDir;
    private Dictionary<Direction, Tuple<Vector2, Vector2>> eatRange;

    protected void Start()
    {
        OnStart();
    }
    protected override void OnStart()
    {
        GameManager.Instance.Player = this;
        startPos = transform.position;
        ResetPlayer();
        GameManager.Instance.OnRestartGame += ResetPlayer;
        eatRange = new Dictionary<Direction, Tuple<Vector2, Vector2>>();
        Vector2 v1 = ((Vector2)corners[0].position - startPos).normalized;
        Vector2 v2 = ((Vector2)corners[1].position - startPos).normalized;
        Vector2 v3 = ((Vector2)corners[2].position - startPos).normalized;
        Vector2 v4 = ((Vector2)corners[3].position - startPos).normalized;
        eatRange.Add(Direction.Up, new Tuple<Vector2, Vector2>(v1, v2));
        eatRange.Add(Direction.Down, new Tuple<Vector2, Vector2>(v3, v4));
        eatRange.Add(Direction.Right, new Tuple<Vector2, Vector2>(v1, v4));
        eatRange.Add(Direction.Left, new Tuple<Vector2, Vector2>(v2, v3));
        
    }
    
    private void ResetPlayer()
    {
        tunnelDir = Vector2.zero;
        destination = startPos;
        transform.rotation = Quaternion.Euler(0,0,0);
        animator.SetFloat("DirX", 0);
        animator.SetFloat("DirY", 0);
        animator.Play("right");
        transform.position = startPos;
        direction = Vector2.zero;
        dirEnum = Direction.None;
    }

    protected override void OnUpdate()
    {
        if (GameManager.Instance.GameState == GameState.Playing)
        {
            currPos = transform.position;
            Vector2 p = Vector2.MoveTowards(currPos, destination, speed);
            rigidbody.MovePosition(p);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public bool CanEat(Vector2 pos)
    {
        var range = eatRange[dirEnum];
        Vector2 p1 = range.Item1.normalized;
        Vector2 p2 = range.Item2.normalized;
        pos = pos - (Vector2)transform.position;
        bool inRange1 = pos.normalized.IsInBetween(p1, direction.normalized);
        bool inRange2 = pos.normalized.IsInBetween(p2, direction.normalized);
        return inRange1 || inRange2;
    }
    
    
    
    protected override void Move(Vector2 direction)
    {
        if(CanGo(direction))
        {
            moveDir = direction * speed;
            destination = currPos + moveDir;
            animator.SetFloat("DirX", moveDir.x);
            animator.SetFloat("DirY", moveDir.y);
        }
        else
        {
            Debug.Log("Can't go there");
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (direction == tunnelDir) return;
        if (other.gameObject.CompareTag("RightTunnel"))
        {
            transform.position = leftTunnel.position;
            tunnelDir = direction;
        }
        else if(other.gameObject.CompareTag("LeftTunnel"))
        {
            transform.position = rightTunnel.position;
            tunnelDir = direction;
        }
    }
    
}