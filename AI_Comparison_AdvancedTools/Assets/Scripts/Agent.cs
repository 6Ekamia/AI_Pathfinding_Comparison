using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    [SerializeField] Transform destination;
    [SerializeField] NavMeshAgent agent;

    void Start()
    {
        agent.SetDestination(destination.position);
    }

    void Update()
    {
        
    }
}
