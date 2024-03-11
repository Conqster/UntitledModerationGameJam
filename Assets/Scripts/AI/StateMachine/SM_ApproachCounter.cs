using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SM_ApproachCounter : StateMachine
{
    private float incTimerLongWait = 0.0f;
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

        if(incTimerLongWait > 1.0f)
        {
            incTimerLongWait = 0.0f;
            sm_input.rowdyUtility.IncreaseRowdinessNotGettingDrinks(ref sm_input.rowdinessBehaviour, sm_input.rowdinessTendency);
        }



        switch (sm_gettingDrinkState)
        {
            case SM_GettingDrinkState.Neutral:

                if(NPC_BarManager.Instance != null)
                {
                    if(NPC_BarManager.Instance.AddWaitingForDrinks(ref sm_input.myDrinkSlot))
                    {
                        MoveTo(sm_input.myDrinkSlot.position);
                        sm_gettingDrinkState = SM_GettingDrinkState.HaveASlot;
                    }
                    else
                    {
                        //sm_input.navAgent.isStopped = true;

                    }

                       
                }

                break;
            case SM_GettingDrinkState.HaveASlot:

                if (sm_input.navAgent.remainingDistance < 0.3f)
                {
                    sm_gettingDrinkState = SM_GettingDrinkState.WaitingForDrink;
                }

                if (sm_input.rowdinessBehaviour > 0.9f)
                {
                    if (NPC_BarManager.Instance != null)
                        NPC_BarManager.Instance.RemoveServedNPC(sm_input.myDrinkSlot);

                    TriggerExit(new SM_ApproachDanceFloor(sm_input, sm_output));
                }

                break;
            case SM_GettingDrinkState.WaitingForDrink:

                incTimerLongWait += Time.deltaTime;

                //Rotate towards the center
                if (NPC_BarManager.Instance != null)
                {
                    Vector3 target = NPC_BarManager.Instance.GetCurrentBarTenderPos;
                    target.y = sm_input.self.position.y;
                    sm_input.self.LookAt(target);
                }


                if (sm_input.rowdinessBehaviour > 0.9f)
                {
                    if(NPC_BarManager.Instance != null)
                        NPC_BarManager.Instance.RemoveServedNPC(sm_input.myDrinkSlot);

                    TriggerExit(new SM_ApproachDanceFloor(sm_input, sm_output));
                }


                break;
            case SM_GettingDrinkState.GotADrink:
                break;

        }





        base.Update();
    }

    protected override void Exit()
    {

        sm_gettingDrinkState = SM_GettingDrinkState.Neutral;
        base.Exit();
    }
}
