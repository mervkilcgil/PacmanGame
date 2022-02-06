using System;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public Action IncreaseScore;
    private void Start()
    {
        
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
    }

    private void OnDestroy()
    {
        
    }
}