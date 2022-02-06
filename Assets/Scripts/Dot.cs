using System;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public Action IncreaseScore;
    public LayerMask[] walkableMask;
    private void Start()
    {
        Collider2D[] hits = null;
        Collider2D[] hitWalls = Physics2D.OverlapCircleAll(transform.position, 3, walkableMask[0]);
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(transform.position, 3, walkableMask[1]);
        hits = new Collider2D[hitWalls.Length + hitPlayer.Length];
        hitWalls?.CopyTo(hits, 0);
        hitPlayer?.CopyTo(hits, hitWalls.Length);
        if (hits == null)
            return;
        if (hits.Length > 0) Destroy(gameObject);
        IncreaseScore += ()=>GameManager.Instance.IncreaseScore(1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SoundManager.Instance.PlayChomp();
            IncreaseScore?.Invoke();
            Destroy(gameObject);
            GameManager.Instance.DotCount--;
        }
        else if (other.gameObject.CompareTag("Ghost"))
        {
            Destroy(gameObject);
            GameManager.Instance.DotCount--;
        }
    }

    private void OnDestroy()
    {
        
    }
}