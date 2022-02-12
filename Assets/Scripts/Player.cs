using UnityEngine;

public class Player : MoveController
{
    [SerializeField] protected Rigidbody2D rigidbody;
    [SerializeField] protected Animator animator;
    private Vector2 moveDir;
    private Vector2 startPos;

    public void Start()
    {
        GameManager.Instance.Player = this;
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
        return false;
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

    public void EatGhost()
    {
        GameManager.Instance.EatGhost();
    }
}