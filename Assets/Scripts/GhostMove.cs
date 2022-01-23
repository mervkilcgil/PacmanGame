using UnityEngine;

public class GhostMove : MoveController
{
    protected override void OnStart()
    {
        destination = transform.position;
        transform.rotation = Quaternion.Euler(0,0,0);
    }

    protected override void OnUpdate()
    {
        currPos = transform.position;
        Vector2 p = Vector2.MoveTowards(currPos, destination, speed);
        transform.position = p;
    }

    protected override void Move(Vector2 direction)
    {
        if(CanGo(direction))
        {
            Vector2 moveDir = direction * speed;
            destination = currPos + moveDir;
        }
    }

    protected override Direction GetDirection()
    {
        return (Direction)Random.Range(0, 4);
    }
    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.GameOver();
        }
        
    }
}