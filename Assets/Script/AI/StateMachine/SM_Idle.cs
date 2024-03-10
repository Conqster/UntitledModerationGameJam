using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_Idle : StateMachine
{
    

    public SM_Idle() : base()
    {
        sm_name = "Idle State";
    }


    protected override void Enter()
    {
        base.Enter();
    }

    protected override void Update()
    {
        base.Update();
    }


    protected void OnExit()
    {
        base.Exit();
    }
}
