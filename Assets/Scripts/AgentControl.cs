using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentControl : MonoBehaviour
{
    public Material evacuatingMaterial;
    public Material panicMaterial;

    GameObject[] exitLocations;
    GameObject[] roamingTargets;
    NavMeshAgent agent;
    AgentState state;
    Renderer agentRenderer;


    float detectionRadius = 20.0f;
    // float fleeRadius = 10.0f;
    bool isRoaming;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agentRenderer = GetComponent<Renderer>();
        exitLocations = GameObject.FindGameObjectsWithTag("exit");
        roamingTargets = GameObject.FindGameObjectsWithTag("roamingTarget");
        
        agent.angularSpeed = 120.0f;
        if(agent.speed > 0) {
            state = AgentManager.AM.roamingState;
            isRoaming = false;
            
            SetRandomSpeedAndRoamingTarget();
        } else {
            state = AgentManager.AM.idleState;
            isRoaming = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance < 1.0f)
        {
            if (isRoaming)
            {
                SetRandomSpeedAndRoamingTarget();
            }
            else
            {
                agent.speed = 0;
            }

        }
    }

    public void DetectNewObstacle(Vector3 position)
    {
        if (Vector3.Distance(position, this.transform.position) < detectionRadius)
        {
            isRoaming = false;
            state = AgentManager.AM.evacuatingState;
            agent.SetDestination(this.GetClosestExit().transform.position);
            SetRandomSpeedInSpeedRange();
            agentRenderer.material = evacuatingMaterial;
        }
    }

    GameObject GetClosestExit()
    {
        float currentDistance = 99999.0f;
        GameObject closestExit = this.exitLocations[0];
        foreach (GameObject exit in this.exitLocations)
        {
            float distance = Vector3.Distance(exit.transform.position, this.transform.position);
            if (distance < currentDistance)
            {
                closestExit = exit;
                currentDistance = distance;
            }
        }

        return closestExit;
    }

    void SetRandomSpeedAndRoamingTarget()
    {
        float speedMult = Random.Range(0.5f, 1.5f);
        agent.speed *= speedMult;
        if (agent.speed > state.SpeedRange.max) {
            agent.speed = state.SpeedRange.max;
        } else if (agent.speed < state.SpeedRange.min) {
            agent.speed = state.SpeedRange.min;
        }

        int i = Random.Range(0, roamingTargets.Length);
        agent.SetDestination(roamingTargets[i].transform.position);
    }

    void SetRandomSpeedInSpeedRange()
    {
        agent.speed = Random.Range(state.SpeedRange.min, state.SpeedRange.max);
    }

}

