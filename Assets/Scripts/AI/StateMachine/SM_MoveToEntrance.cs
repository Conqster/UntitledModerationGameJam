using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_MoveToEntrance : StateMachine
{

    private Vector3 targetEntrance = Vector3.zero;

    public SM_MoveToEntrance(SM_BrainInput input, SM_BrainOutput output) : base(input, output)
    {
        sm_name = "MoveToEntrance State";
        sm_state = SM_State.MoveToEntrance;
    }


    protected override void Enter()
    {
        targetEntrance = sm_input.barEntranceLocation.position;
        MoveTo(targetEntrance);
        base.Enter();
    }

    protected override void Update()
    {
        if(targetEntrance != sm_input.barEntranceLocation.position)
        {
            targetEntrance = sm_input.barEntranceLocation.position;
            MoveTo(targetEntrance);
        }    


        if(Vector3.Distance(sm_input.self.position, sm_input.barEntranceLocation.position) < sm_input.reachedEntranceTreashold)
        {
            Debug.Log("At Location!!!!!!!!!");
            sm_output.barAccessState = SM_BarAccessState.WaitInQuene;
            if(sm_input.StopOnReachTreshold)
            {
                sm_input.navAgent.isStopped = true;
            }

            if(NPC_BarManager.Instance != null)
                NPC_BarManager.Instance.AddToQuene(sm_input.npc);

            TriggerExit(new SM_Idle(sm_input, sm_output));
        }



        base.Update();
    }


    protected void OnExit()
    {
        base.Exit();
    }
}
