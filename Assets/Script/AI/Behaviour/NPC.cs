using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class NPC : MonoBehaviour
{
    private StateMachine NPC_SM;

    [SerializeField] private StateMachineData SMData;

    // Start is called before the first frame update
    void Start()
    {
        NPC_SM = new SM_Idle();
    }

    // Update is called once per frame
    void Update()
    {
        NPC_SM = NPC_SM.Process();
        SMData = NPC_SM.GetStateMachineData();
    }
}
