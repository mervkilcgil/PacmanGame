using System;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D other)
    {
        GameManager.Instance.GameOver();
    }
}