using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_Idle : StateMachine
{
    public SM_Idle(SM_BrainInput input, SM_BrainOutput output) : base(input, output)
    {
        sm_name = "Idle State";
        sm_state = SM_State.Idle;
    }


    protected override void Enter()
    {
        base.Enter();
    }

    protected override void Update()
    {


        switch(sm_barAccessState)
        {
            case SM_BarAccessState.Neutral:
                if (sm_duration > sm_input.waitDuration)
                    TriggerExit(new SM_MoveToEntrance(sm_input, sm_output));
                break;
            case SM_BarAccessState.WaitInQuene:

                //if (sm_duration > 1.0f)
                //    ChangeBarAccessState(SM_BarAccessState.Granted);

                break;
            case SM_BarAccessState.Granted:

                if(sm_input.rowdinessBehaviour > 0.5f)
                    TriggerExit(new SM_ApproachDanceFloor(sm_input, sm_output));
                else
                    TriggerExit(new SM_ApproachCounter(sm_input, sm_output));   



                break;
            case SM_BarAccessState.Declined: 


                break;
        }





        base.Update();
    }


    protected void OnExit()
    {
        base.Exit();
    }



}
