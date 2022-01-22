using System;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Animator animator;
    private int speed = 1;
    private Vector2 destination;
    private Vector2 currPos;
    
    private void Start()
    {
        destination = transform.position;
        transform.rotation = Quaternion.Euler(0,0,0);
        animator.SetFloat("DirX", 0);
        animator.SetFloat("DirY", 0);
    }
    private void FixedUpdate() 
    {
        currPos = transform.position;
        Vector2 p = Vector2.MoveTowards(currPos, destination, speed);
        rigidbody.MovePosition(p);
        transform.rotation = Quaternion.Euler(0,0,0);
        //if ((Vector2)transform.position == destination)
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
           Vector2 moveDir = direction * speed;
           destination = currPos + moveDir;
           animator.SetFloat("DirX", moveDir.x);
           animator.SetFloat("DirY", moveDir.y);
       }
    }
    private bool CanGo(Vector2 direction) 
    {
        currPos = transform.position;
        var newPos = currPos + direction*speed;
        RaycastHit2D hit = Physics2D.Linecast(newPos, currPos);
        if(hit.collider == null)
            return true;
        return !hit.collider.CompareTag("Wall");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //destination = transform.position;
    }
}