using System;
using UnityEngine;
using UnityEngine.AI;

public class GhostMove : MonoBehaviour
{
    [SerializeField] public SoundManager soundManager;
    private NavMeshAgent agent;
    [SerializeField] private Transform player;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.Warp(transform.position);
    }

    public void SetPlayer(Transform player)
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        GetComponent<NavMeshSurface>().BuildNavMesh();
        agent.Warp(transform.position);
        this.player = player;
        InvokeRepeating(nameof(SetTarget), 0f, 3f);
    }
    private void SetTarget()
    {
        if (!agent.isOnNavMesh)
        {
            agent.ActivateCurrentOffMeshLink(true);
        }
        agent.destination =  player.position;
    }
    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.GameOver();
        }
        
    }
}