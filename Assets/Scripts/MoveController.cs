using UnityEngine;

public class MoveController : MonoBehaviour
{
    protected float speed = 3.5f;
    protected Vector2 destination;
    protected Vector2 currPos;
    protected Vector2 direction;
    protected Direction dirEnum;
    

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
            direction = GetDirection();
            Move(direction);
        }
    }

    protected virtual Vector2 GetDirection()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            dirEnum = Direction.Up;
            return Vector2.up;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            dirEnum = Direction.Right;
            return Vector2.right;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            dirEnum = Direction.Down;
            return -Vector2.up;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            dirEnum = Direction.Left;
            return -Vector2.right;
        }
        else return direction;

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
    Left
}
