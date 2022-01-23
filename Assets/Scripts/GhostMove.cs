using UnityEngine;

public class GhostMove : MoveController
{
    protected override void OnStart()
    {
        destination = transform.position;
        transform.rotation = Quaternion.Euler(0,0,0);
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
}