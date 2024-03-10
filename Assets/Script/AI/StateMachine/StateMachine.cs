using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum SM_Event
{
    Enter,
    Update,
    Exit
}


public enum SM_State
{
    Idle,
    MoveTo,
    WaitInQuene
}

[System.Serializable]
public struct StateMachineData
{
    public string stateName;
    public SM_Event stateEvent;
    public float stateDuration;
    public SM_State state;
}



public class StateMachine
{
    private StateMachineData sm_stateData;
    protected StateMachine sm_transitTo;
    protected bool sm_transitionTriggered = false;  //triggered for transition
    protected SM_Event sm_event;   //current event in state 
    protected string sm_name;             // state name 
    protected float sm_duration;

    protected SM_State sm_state;


    public StateMachine()
    {
        sm_name = "Base State";
        sm_event = SM_Event.Enter;
        sm_duration = 0.0f;
    }


    public StateMachine Process()
    {
        sm_stateData.stateName = sm_name;
        sm_stateData.stateDuration = sm_duration;
        sm_stateData.stateEvent = sm_event;
        sm_stateData.state = sm_state;


        switch (sm_event)
        {
            case SM_Event.Enter:
                Enter();
                break;
            case SM_Event.Update:
                Update();
                break;
            case SM_Event.Exit:
                Debug.Log("Prepare to exit");
                Exit();
                return sm_transitTo;
        }


        return this;
    }



    protected virtual void Enter()
    {
        sm_duration = 0.0f;



        //after all logic as be performed change state event to update
        sm_event = SM_Event.Update;
    }



    /// <summary>
    /// what specific state do on it event update every frame
    /// </summary>
    protected virtual void Update()
    {
        sm_duration += Time.deltaTime;




        if (sm_transitionTriggered)
            return;

        sm_event = SM_Event.Update;
    }


    /// <summary>
    /// what state should do before transiting to next state 
    /// </summary>
    protected virtual void Exit()
    {
        //sm_settings.child.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        sm_event = SM_Event.Exit;
    }


    protected virtual void TriggerExit(StateMachine transition)
    {
        sm_transitTo = transition;
        sm_transitionTriggered = true;
        sm_event = SM_Event.Exit;
    }


    public string GetSM_Name() => sm_name;
    public SM_Event GetSM_Event() => sm_event;

    public StateMachineData GetStateMachineData() => sm_stateData;

    private StateMachine GetStateMachine() => this;

}
