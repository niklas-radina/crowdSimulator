using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using ACHF = AgentControlHelperFunctions;

public class AgentControl : MonoBehaviour
{
    private readonly float _detectionRadius = 20.0f;

    private NavMeshAgent _agent;
    private Renderer _agentRenderer;
    private AgentManager _am;
    private bool _didInititalize;


    // float fleeRadius = 10.0f;
    private bool _isRoaming;
    [CanBeNull] private AgentState _nextState;
    private SceneObjectManager _sceneOM;
    private ScriptObjectManager _scriptOM;
    private AgentState _state;


    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agentRenderer = GetComponent<Renderer>();

        _agent.angularSpeed = 120.0f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_nextState != null)
        {
            NewState(_nextState);
            _nextState = null;
        }

        if (_agent.remainingDistance < 1.0f)
        {
            if (_isRoaming)
                ACHF.ContinueRoaming(_agent, _scriptOM, _sceneOM);
            else
                _agent.speed = 0;
        }
    }

    public void SetObjectManagerAndInitialize(SceneObjectManager sceneOM, ScriptObjectManager scriptOM)

    {
        _sceneOM = sceneOM;
        _scriptOM = scriptOM;
        if (_nextState == null)
        {
            _state = _scriptOM.IdleState;
            _isRoaming = false;
        }
        else
        {
            NewState(_nextState);
        }
    }

    public void SetStateForNextFrame(AgentState state)
    {
        if (_state == state) return;
        _nextState = state;
    }

    private void NewState(AgentState state)
    {
        _state = state;
        switch (state.StateName)
        {
            case "idle":
                ACHF.BeIdle(_agent, transform.position, _agentRenderer, _sceneOM);
                _isRoaming = false;
                break;
            case "roaming":
                ACHF.StartRoaming(_agent, _agentRenderer, _scriptOM, _sceneOM);
                _isRoaming = true;
                break;
            case "evacuating":
                ACHF.StartEvacuating(_agent, _agentRenderer, transform.position, _sceneOM, _scriptOM);
                _isRoaming = false;
                break;
            case "panicking":
                _isRoaming = false;
                break;
            default:
                Debug.LogException(new Exception($"Agent was provided with unknown AgentState: {state}"));
                break;
        }
    }

    public void DetectNewObstacle(Vector3 position)
    {
        if (Vector3.Distance(position, transform.position) < _detectionRadius)
            NewState(_scriptOM.EvacuatingState);
    }
}