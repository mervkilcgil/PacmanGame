using System;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public Action IncreaseScore;
    public LayerMask walkableMask;
    private void Start()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 3, walkableMask);
        if (hits == null)
            return;
        if (hits.Length > 0) Destroy(gameObject);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IncreaseScore?.Invoke();
            Destroy(gameObject);
            GameManager.Instance.DotCount--;
        }
        else if (other.gameObject.CompareTag("Ghost"))
        {
            Destroy(gameObject);
            GameManager.Instance.DotCount--;
        }
        else
        {
            Destroy(gameObject);
            GameManager.Instance.DotCount--;
        }
    }

    private void OnDestroy()
    {
        
    }
}