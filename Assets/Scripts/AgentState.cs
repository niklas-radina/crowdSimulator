using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentState
{
    public string StateName { get; set; }
    public (float min, float max) SpeedRange { get; set; }

    public AgentState(string stateName, (float min, float max) speedRange)
    {
        StateName = stateName;
        SpeedRange = speedRange;
    }
}