using UnityEngine;

public class Player : MoveController
{
    [SerializeField] protected Rigidbody2D rigidbody;
    [SerializeField] protected Animator animator;

    protected override void OnStart()
    {
        destination = transform.position;
        transform.rotation = Quaternion.Euler(0,0,0);
        animator.SetFloat("DirX", 0);
        animator.SetFloat("DirY", 0);
    }
    
    protected override void OnUpdate()
    {
        currPos = transform.position;
        Vector2 p = Vector2.MoveTowards(currPos, destination, speed);
        rigidbody.MovePosition(p);
        transform.rotation = Quaternion.Euler(0,0,0);
    }
    
    protected override void Move(Vector2 direction)
    {
        if(CanGo(direction))
        {
            Vector2 moveDir = direction * speed;
            destination = currPos + moveDir;
            animator.SetFloat("DirX", moveDir.x);
            animator.SetFloat("DirY", moveDir.y);
        }
    }
}