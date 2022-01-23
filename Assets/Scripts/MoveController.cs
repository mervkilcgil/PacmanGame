using System;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rigidbody;
    [SerializeField] protected Animator animator;
    protected int speed = 1;
    protected Vector2 destination;
    protected Vector2 currPos;
    
    protected void Start()
    {
        OnStart();
    }

    protected virtual void OnStart()
    {
        destination = transform.position;
        transform.rotation = Quaternion.Euler(0,0,0);
        animator.SetFloat("DirX", 0);
        animator.SetFloat("DirY", 0);
    }
    protected void FixedUpdate() 
    {
        currPos = transform.position;
        Vector2 p = Vector2.MoveTowards(currPos, destination, speed);
        rigidbody.MovePosition(p);
        transform.rotation = Quaternion.Euler(0,0,0);
        OnUpdate();
    }

    protected  void OnUpdate()
    {
        //if ((Vector2)transform.position == destination)
        {
            switch (GetDirection())
            {
                case Direction.Up:
                    Move(Vector2.up);
                    break;
                case Direction.Down: 
                    Move(-Vector2.up);
                    break;
                case Direction.Right:
                    Move(Vector2.right);
                    break;
                case Direction.Left:
                    Move(-Vector2.right);
                    break;
            }
        }
    }

    protected virtual Direction GetDirection()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            return Direction.Up;
        else if (Input.GetKey(KeyCode.RightArrow))
            return Direction.Right;
        else if (Input.GetKey(KeyCode.DownArrow))
            return Direction.Down;
        else if (Input.GetKey(KeyCode.LeftArrow))
            return Direction.Left;
        else
            return Direction.None;
    }

    protected virtual void Move(Vector2 direction)
    {
       if(CanGo(direction))
       {
           Vector2 moveDir = direction * speed;
           destination = currPos + moveDir;
           animator.SetFloat("DirX", moveDir.x);
           animator.SetFloat("DirY", moveDir.y);
       }
    }
    protected bool CanGo(Vector2 direction) 
    {
        currPos = transform.position;
        var newPos = currPos + direction*speed;
        RaycastHit2D hit = Physics2D.Linecast(newPos, currPos);
        if(hit.collider == null)
            return true;
        return !hit.collider.CompareTag("Wall");
    }
    
}

public enum Direction
{
    Up,
    Right,
    Down,
    Left,
    None
}