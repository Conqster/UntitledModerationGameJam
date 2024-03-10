using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_ApproachDanceFloor : StateMachine
{

    private float updateRowdinessTimer = 0.0f;


    public SM_ApproachDanceFloor(SM_BrainInput input, SM_BrainOutput output) : base(input, output)
    {
        sm_name = "Approaching Dance Floor";
        sm_state = SM_State.ApproachingDanceFloor;
    }


    protected override void Enter()
    {
        updateRowdinessTimer = 0.0f;
        MoveTo(sm_input.barDanceLocation.position);
        sm_dancingState = SM_DanceFloorState.Approaching;
        base.Enter();
    }

    protected override void Update()
    {

        updateRowdinessTimer += Time.deltaTime;

        if(updateRowdinessTimer > 1.0f)
        {
            updateRowdinessTimer = 0.0f;
            sm_input.rowdyUtility.ReduceRowdiness(ref sm_input.rowdinessBehaviour, sm_input.rowdinessTendency);
        }

        if (sm_input.rowdinessBehaviour < 0.2f)
            TriggerExit(new SM_ApproachCounter(sm_input, sm_output));

        switch(sm_dancingState)
        {
            case SM_DanceFloorState.Neutral:

                break;
            case SM_DanceFloorState.Approaching:

                if(Vector3.Distance(sm_input.self.position, sm_input.barDanceLocation.position) < sm_input.danceFloorTreshold)
                {
                    sm_input.navAgent.isStopped = true;
                    //sm_output.danceFloorState = SM_DanceFloorState.FindingASpot;
                    sm_dancingState = SM_DanceFloorState.FindingASpot;
                }

                break;
            case SM_DanceFloorState.FindingASpot:

                if (NPC_BarManager.Instance != null)
                {
                    if(NPC_BarManager.Instance.GetDanceEQS.QueryAPost(sm_input.npc, out Vector3 post))
                    {
                        MoveTo(post);
                        sm_dancingState = SM_DanceFloorState.ApproachingSpot;
                    }
                }

                break;

            case SM_DanceFloorState.ApproachingSpot:

                if (sm_input.navAgent.remainingDistance < 0.2f)
                {
                    //Rotate towards the center
                    if(NPC_BarManager.Instance != null)
                    {
                        Vector3 target = NPC_BarManager.Instance.GetDanceEQS.GetCenterLocation;
                        sm_input.self.LookAt(target);
                    }

                    sm_dancingState = SM_DanceFloorState.DancingInSpot;
                }

                break;
            case SM_DanceFloorState.DancingInSpot:
                break;

        }




        base.Update(); 
    }

    protected override void Exit()
    {
        sm_dancingState = SM_DanceFloorState.Neutral;

        if (NPC_BarManager.Instance != null)
            NPC_BarManager.Instance.GetDanceEQS.ReleaseAPost(sm_input.npc);

        base.Exit();
    }
}
