using System;
using UnityEngine;

public class LaneManager : MonoBehaviour
{
    public Lane[] lanes;
    private void Awake()
    {
        InitialiseLanes();
    }

    private void InitialiseLanes()
    {
        for (var i = 0; i < lanes.Length; i++)
        {
            lanes[i].Initialise(i);
        }
    }
    
    public Lane ReturnLaneToTheLeft(int currentLaneIndex)
    {
        if (currentLaneIndex <= 0)
        {
            #if UNITY_EDITOR
            EndlessRunnerGameManager.DisplayDebugMessage("You are in the right most lane, Lane: " + currentLaneIndex);
            #endif
            EndlessRunnerGameManager.instance.audioManager.PlayInvalidLaneNoise();
            return lanes[currentLaneIndex];
        }
        return lanes[currentLaneIndex - 1];
    }
    
    public Lane ReturnLaneToTheRight(int currentLaneIndex)
    {
        if (currentLaneIndex >= lanes.Length-1)
        {
            #if UNITY_EDITOR
            EndlessRunnerGameManager.DisplayDebugMessage("You are in the left most lane, Lane: " + currentLaneIndex);
            #endif
            EndlessRunnerGameManager.instance.audioManager.PlayInvalidLaneNoise();
            return lanes[currentLaneIndex];
        }
        return lanes[currentLaneIndex + 1];
    }
    
}

[Serializable]
public class Lane
{
    [HideInInspector]
    public int laneIndex;
    public int laneXPosition;

    public void Initialise(int newLaneIndex)
    {
        laneIndex = newLaneIndex;
    }
}
