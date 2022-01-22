using System;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody;
    private int speed = 1;
    private Vector2 destination;

    private void FixedUpdate() 
    {
        Vector2 p = Vector2.MoveTowards(transform.position, destination, speed);
        rigidbody.MovePosition(p);
        Vector2 currPos = transform.position;
        if (currPos == destination)
        {
            if (Input.GetKey(KeyCode.UpArrow))
                Move(Vector2.up);
            else if (Input.GetKey(KeyCode.RightArrow))
                Move(Vector2.right);
            else if (Input.GetKey(KeyCode.DownArrow))
                Move(-Vector2.up);
            else if (Input.GetKey(KeyCode.LeftArrow))
                Move(-Vector2.right);
        }
    }

    private void Move(Vector2 direction)
    {
       if(CanGo(direction))
       {
           Vector2 currPos = transform.position;
           destination = currPos + direction*speed;
       }
    }
    private bool CanGo(Vector2 direction) 
    {
        Vector2 currPos = transform.position;
        var newPos = currPos + direction*speed;
        RaycastHit2D hit = Physics2D.Linecast(newPos, currPos);
        return hit.collider == GetComponent<Collider2D>();
    }
}