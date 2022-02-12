using UnityEngine;

public class Player : MoveController
{
    [SerializeField] protected Rigidbody2D rigidbody;
    [SerializeField] protected Animator animator;
    private Vector2 moveDir;
    private Vector2 startPos;
    [SerializeField] private Transform rightTunnel, leftTunnel;
    private Vector2 tunnelDir;

    public void Start()
    {
        GameManager.Instance.Player = this;
        tunnelDir = Vector2.zero;
    }
    protected override void OnStart()
    {
        startPos = transform.position;
        destination = transform.position;
        transform.rotation = Quaternion.Euler(0,0,0);
        animator.SetFloat("DirX", 0);
        animator.SetFloat("DirY", 0);
        GameManager.Instance.OnRestartGame += OnRestartGame;
    }

    private void OnRestartGame()
    {
        transform.position = startPos;
        destination = transform.position;
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
        Vector2 p1 = direction.PerpendicularClockwise().Rotate(45f).normalized;
        Vector2 p2 = direction.PerpendicularCounterClockwise().Rotate(45f).normalized;
        return pos.normalized.IsInBetween(p1,direction) || pos.normalized.IsInBetween(p2, direction);
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