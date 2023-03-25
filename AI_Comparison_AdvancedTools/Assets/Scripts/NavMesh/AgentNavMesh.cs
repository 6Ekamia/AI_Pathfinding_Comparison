using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentNavMesh : MonoBehaviour
{
    [SerializeField] Transform destination;
    [SerializeField] NavMeshAgent agent;

    bool startCounting = false;
    float time = 0;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            agent.SetDestination(destination.position);
            startCounting = true;
        }

        if (startCounting) time += Time.deltaTime;

        if (agent.hasPath)
        {
            startCounting = false;
            Debug.Log("Time took to find path: " + time);
        }
    }
}
