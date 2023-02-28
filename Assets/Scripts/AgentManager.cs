using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 
    AgentManager : MonoBehaviour
{
    public static AgentManager AM;

    [Header ("Agent Settings")]
    [Range(0.5f, 5.0f)]
    public float speedMultiplier = 1.0f;

    public AgentState IdleState = new AgentState("idle", (0.0f, 0.0f));
    public AgentState RoamingState = new AgentState("roaming", (0.5f, 3.5f));
    public AgentState EvacuatingState = new AgentState("evacuating", (1.5f, 2.5f));
    public AgentState PanicingState = new AgentState("panicing", (2.0f, 5.0f));

    void Start()
    {
        IdleState.SpeedRange = (0.0f * speedMultiplier, 0.0f * speedMultiplier);
        RoamingState.SpeedRange = (0.5f * speedMultiplier, 3.5f * speedMultiplier);
        EvacuatingState.SpeedRange = (1.5f * speedMultiplier, 2.5f * speedMultiplier);
        PanicingState.SpeedRange = (2.0f * speedMultiplier, 5.0f * speedMultiplier);

        AM = this;

    }

    void Update()
    {
        
    }
}
