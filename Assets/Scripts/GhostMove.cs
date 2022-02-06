using System;
using Astar2DPathFinding.Mika;
using UnityEngine;
using Random = UnityEngine.Random;

public class GhostMove : MonoBehaviour
{
    [SerializeField] public SoundManager soundManager;
    public GhostSpawner ghostSpawner;
    private float deltaTime = 5f;
    private float spawnTime = 5f;
    public void SetGhostSpawner(GhostSpawner ghostSpawner)
    {
        this.ghostSpawner = ghostSpawner;
    }

    public void FixedUpdate()
    {
        if (deltaTime >= spawnTime && ghostSpawner)
        {
            deltaTime = 0f;
            SetRandomCornerTarget();
        }
        deltaTime += Time.fixedDeltaTime;
    }

    public void SetRandomCornerTarget()
    {
        int randomCorner = Random.Range(0, 4);
        
        GetComponent<SeekerController>().AddTarget(ghostSpawner.Corners[randomCorner].position);
    }
    
    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.GameOver();
        }
        
    }
}