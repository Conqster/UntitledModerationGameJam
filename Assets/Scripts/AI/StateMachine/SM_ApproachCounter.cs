using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_ApproachCounter : StateMachine
{
    
    public SM_ApproachCounter(SM_BrainInput input, SM_BrainOutput output) : base(input, output)
    {
        sm_name = "Approaching Counter";
        sm_state = SM_State.ApproachingCounter;
    }

    protected override void Enter()
    {
        MoveTo(sm_input.barCounterLocation.position);
        base.Enter();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Exit()
    {
        base.Exit();
    }
}
