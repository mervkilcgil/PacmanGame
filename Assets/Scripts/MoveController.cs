using UnityEngine;

public class MoveController : MonoBehaviour
{
    
    protected int speed = 2;
    protected Vector2 destination;
    protected Vector2 currPos;

    protected void Start()
    {
        OnStart();
    }

    protected virtual void OnStart()
    {
    }
    protected void FixedUpdate() 
    {
        OnUpdate();
        MoveForward();
    }

    
    protected virtual void OnUpdate()
    {

    }

    protected void MoveForward()
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

    }
    protected bool CanGo(Vector2 direction)
    {
        if (GameManager.Instance.GameState != GameState.Playing) 
            return false;
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