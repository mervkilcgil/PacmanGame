using UnityEngine;

public class Player : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Ghost"))
        {
            GameManager.Instance.GameOver();
        }
        
    }
}