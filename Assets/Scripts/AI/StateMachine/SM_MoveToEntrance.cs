using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_MoveToEntrance : StateMachine
{
    public SM_MoveToEntrance(SM_BrainInput input, SM_BrainOutput output) : base(input, output)
    {
        sm_name = "MoveToEntrance State";
        sm_state = SM_State.MoveToEntrance;
    }


    protected override void Enter()
    {
        MoveTo(sm_input.barEntranceLocation.position);
        base.Enter();
    }

    protected override void Update()
    {
        if(Vector3.Distance(sm_input.self.position, sm_input.barEntranceLocation.position) < sm_input.reachedEntranceTreashold)
        {
            Debug.Log("At Location!!!!!!!!!");
            sm_output.barAccessState = SM_BarAccessState.WaitInQuene;
            if(sm_input.StopOnReachTreshold)
            {
                sm_input.navAgent.isStopped = true;
            }

            TriggerExit(new SM_Idle(sm_input, sm_output));
        }



        base.Update();
    }


    protected void OnExit()
    {
        base.Exit();
    }
}
