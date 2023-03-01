using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class
    AgentManager : MonoBehaviour
{
    public static AgentManager AM;

    [Header("Agent Settings")] [Range(0.5f, 5.0f)]
    public float speedMultiplier = 1.0f;


    public List<GameObject> agents = new();
    public GameObject sceneObjectManagerGameObject;
    public GameObject scriptObjectManagerGameObject;

    public SceneObjectManager sceneOM;
    public ScriptObjectManager scriptOM;

    private void Start()
    {
        AM = this;

        sceneOM = sceneObjectManagerGameObject.GetComponent<SceneObjectManager>();
        scriptOM = scriptObjectManagerGameObject.GetComponent<ScriptObjectManager>();


        scriptOM.IdleState.SpeedRange = (0.0f * speedMultiplier, 0.0f * speedMultiplier);
        scriptOM.RoamingState.SpeedRange = (0.5f * speedMultiplier, 3.5f * speedMultiplier);
        scriptOM.EvacuatingState.SpeedRange = (1.5f * speedMultiplier, 2.5f * speedMultiplier);
        scriptOM.PanicingState.SpeedRange = (2.0f * speedMultiplier, 5.0f * speedMultiplier);

        GenerateAgents();
    }

    private void GenerateAgents()
    {
        Debug.Log("AgentManager generate agents");
        var idleAgents = GameObject.FindGameObjectsWithTag("idleAgent");
        var roamingAgents = GameObject.FindGameObjectsWithTag("roamingAgent");
        agents.AddRange(ReplaceWithAgents(idleAgents, scriptOM.IdleState));
        agents.AddRange(ReplaceWithAgents(roamingAgents, scriptOM.RoamingState));
    }

    private GameObject[] ReplaceWithAgents(GameObject[] objects, AgentState state)
    {
        return objects.Select(o =>
        {
            var agent = Instantiate(sceneOM.agentPrefab, o.transform.position, Quaternion.identity);
            Destroy(o);
            var ac = agent.GetComponent<AgentControl>();
            ac.SetObjectManagerAndInitialize(sceneOM, scriptOM);
            ac.SetStateForNextFrame(state);
            return agent;
        }).ToArray();
    }
}