using UnityEngine;
using UnityEngine.AI;

public static class AgentControlHelperFunctions
{
    public static void StartEvacuating(
        NavMeshAgent agent,
        Renderer agentRenderer,
        Vector3 agentPosition,
        SceneObjectManager sceneOM,
        ScriptObjectManager scriptOM)
    {
        agent.SetDestination(GetClosestExit(agentPosition, sceneOM).transform.position);
        SetRandomSpeedInSpeedRange(agent, scriptOM.EvacuatingState);
        agentRenderer.material = sceneOM.evacuatingMaterial;
    }

    public static void ContinueRoaming(NavMeshAgent agent, ScriptObjectManager scriptOM, SceneObjectManager sceneOM)
    {
        var speedMult = Random.Range(0.5f, 1.5f);
        agent.speed *= speedMult;
        if (agent.speed > scriptOM.RoamingState.SpeedRange.max) agent.speed = scriptOM.RoamingState.SpeedRange.max;
        else if (agent.speed < scriptOM.RoamingState.SpeedRange.min) agent.speed = scriptOM.RoamingState.SpeedRange.min;

        var i = Random.Range(0, sceneOM.roamingLocations.Length);
        agent.SetDestination(sceneOM.roamingLocations[i].transform.position);
    }

    public static void StartRoaming(
        NavMeshAgent agent,
        Renderer agentRenderer,
        ScriptObjectManager scriptOM,
        SceneObjectManager sceneOM)
    {
        SetRandomSpeedInSpeedRange(agent, scriptOM.RoamingState);

        var i = Random.Range(0, sceneOM.roamingLocations.Length);
        agent.SetDestination(sceneOM.roamingLocations[i].transform.position);

        agentRenderer.material = sceneOM.calmMaterial;
    }

    public static void BeIdle(
        NavMeshAgent agent,
        Vector3 agentPosition,
        Renderer agentRenderer,
        SceneObjectManager sceneOM)
    {
        agent.speed = 0;

        agent.SetDestination(agentPosition);

        agentRenderer.material = sceneOM.calmMaterial;
    }


    private static GameObject GetClosestExit(Vector3 position, SceneObjectManager sceneOM)
    {
        var currentDistance = 99999.0f;
        var closestExit = sceneOM.exitLocations[0];
        foreach (var exit in sceneOM.exitLocations)
        {
            var distance = Vector3.Distance(exit.transform.position, position);
            if (distance < currentDistance)
            {
                closestExit = exit;
                currentDistance = distance;
            }
        }

        return closestExit;
    }

    private static void SetRandomSpeedInSpeedRange(NavMeshAgent agent, AgentState state)
    {
        agent.speed = Random.Range(state.SpeedRange.min, state.SpeedRange.max);
    }
}