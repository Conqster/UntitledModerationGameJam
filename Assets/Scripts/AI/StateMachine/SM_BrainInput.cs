using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



[System.Serializable]
public class SM_BrainInput 
{
    [Header("General")]
    [SerializeField] public NavMeshAgent navAgent;
    [SerializeField] public Transform self;
    [SerializeField] public NPC npc;
    [SerializeField] public Transform barEntranceLocation;
    [SerializeField] public Transform barCounterLocation;
    [SerializeField] public Transform barDanceLocation;

    [Header("Behaviour")]
    [SerializeField, Range(0.0f, 1.0f)] public float rowdinessBehaviour;
    [SerializeField, Range(0.0f, 1.0f)] public float rowdinessTendency;
    [SerializeField] public RowdyUtility rowdyUtility = new RowdyUtility();

    [Header("Idle State")]
    [SerializeField, Range(0f, 5f)] public float waitDuration = 2.0f;

    [Header("Approaching Entrance")]
    [SerializeField, Range(0.0f, 5.0f)] public float reachedEntranceTreashold = 2.0f;
    [SerializeField] public bool StopOnReachTreshold = false;

    [Header("Getting Drinks")]
    [SerializeField] public DrinkSlot myDrinkSlot;

    [Header("Dance Floor")]
    [SerializeField, Range(0.0f, 10.0f)] public float danceFloorTreshold = 5.0f;



}


[System.Serializable]
public class SM_BrainOutput
{
    [Header("General")]
    [SerializeField] public SM_BarAccessState barAccessState;
    [SerializeField] public SM_GettingDrinkState gettingDrinkState;
    [SerializeField] public SM_DanceFloorState danceFloorState;
}
