using System;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public Action IncreaseScore;
    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IncreaseScore?.Invoke();
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        
    }
}