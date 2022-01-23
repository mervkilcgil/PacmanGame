using UnityEngine;

public class Ghost : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.GameOver();
        }
        
    }
}