using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_ApproachDanceFloor : StateMachine
{
    
    public SM_ApproachDanceFloor(SM_BrainInput input, SM_BrainOutput output) : base(input, output)
    {
        sm_name = "Approaching Dance Floor";
        sm_state = SM_State.ApproachingDanceFloor;
    }


    protected override void Enter()
    {
        MoveTo(sm_input.barDanceLocation.position);
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
