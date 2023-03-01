using UnityEngine;

public class ScriptObjectManager : MonoBehaviour
{
    public AgentState EvacuatingState = new("evacuating", (1.5f, 2.5f));
    public AgentState IdleState = new("idle", (0.0f, 0.0f));
    public AgentState PanicingState = new("panicking", (2.0f, 5.0f));
    public AgentState RoamingState = new("roaming", (0.5f, 3.5f));
}