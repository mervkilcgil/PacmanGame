using UnityEngine;

public class MoveController : MonoBehaviour
{
    
    protected int speed = 2;
    protected Vector2 destination;
    protected Vector2 currPos;
    protected Vector2 direction;

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
            direction = GetDirection();
            Move(direction);
        }
    }

    protected virtual Vector2 GetDirection()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            return Vector2.up;
        else if (Input.GetKey(KeyCode.RightArrow))
            return Vector2.right;
        else if (Input.GetKey(KeyCode.DownArrow))
            return -Vector2.up;
        else if (Input.GetKey(KeyCode.LeftArrow))
            return -Vector2.right;
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
