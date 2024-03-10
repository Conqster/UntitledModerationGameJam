using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public enum SM_Event
{
    Enter,
    Update,
    Exit
}


public enum SM_State
{
    Idle,
    MoveToEntrance,
    ApproachingDanceFloor,
    ApproachingCounter
    //MoveTo,
    //WaitInQuene
}

public enum SM_BarAccessState
{
    Neutral,
    WaitInQuene,
    Granted,
    Declined
}

public enum SM_GettingDrinkState
{
    //None,
    Neutral, 
    HaveASlot,
    WaitingForDrink,
    GotADrink,
}

public enum SM_DanceFloorState
{
    Neutral,
    Approaching,
    FindingASpot,
    ApproachingSpot,
    DancingInSpot
    //I can have extra state, that could then extranally kick Npc out this state 
}

[System.Serializable]
public struct StateMachineData
{
    public string stateName;
    public SM_Event stateEvent;
    public float stateDuration;
    public SM_State state;

    [Header("Mini-States")]
    public SM_BarAccessState barAccessState;
    public SM_GettingDrinkState gettingDrinkState;
    public SM_DanceFloorState dancingState;
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
    protected SM_BarAccessState sm_barAccessState;
    protected SM_GettingDrinkState sm_gettingDrinkState;
    protected SM_DanceFloorState sm_dancingState;
    protected SM_BrainInput sm_input;
    protected SM_BrainOutput sm_output;


    public StateMachine(SM_BrainInput input, SM_BrainOutput output)
    {
        sm_name = "Base State";
        sm_event = SM_Event.Enter;
        sm_duration = 0.0f;
        sm_input = input;   
        sm_output = output;  

        sm_barAccessState = output.barAccessState;
        sm_gettingDrinkState = output.gettingDrinkState;
        sm_dancingState = output.danceFloorState;
    }


    public StateMachine Process()
    {
        sm_stateData.stateName = sm_name;
        sm_stateData.stateDuration = sm_duration;
        sm_stateData.stateEvent = sm_event;
        sm_stateData.state = sm_state;
        sm_stateData.barAccessState = sm_barAccessState;
        sm_stateData.gettingDrinkState = sm_gettingDrinkState;
        sm_stateData.dancingState = sm_dancingState;


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

    public SM_BarAccessState GetSM_BarAccessState => sm_barAccessState;

    public void ChangeBarAccessState(SM_BarAccessState state) 
    { 
        sm_output.barAccessState = state;
        sm_barAccessState = state; 
    }




    public StateMachineData GetStateMachineData() => sm_stateData;

    private StateMachine GetStateMachine() => this;


    protected void MoveTo(Vector3 location)
    {
        //sm_input.navAgent.Move(location);
        if(sm_input.navAgent.isStopped)
            sm_input.navAgent.isStopped = false;

        sm_input.navAgent.SetDestination(location);
    }

}
