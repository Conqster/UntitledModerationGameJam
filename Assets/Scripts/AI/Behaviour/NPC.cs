using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;


[RequireComponent(typeof(NavMeshAgent))]
public class NPC : MonoBehaviour
{
    private StateMachine NPC_SM;
    private uint id = 0;
    private NavMeshAgent agent;

    public uint Id
    {
        get { return id; }
        set { id = value; }
    }

    [SerializeField] private SM_BrainInput m_brainInput;
    [SerializeField] private SM_BrainOutput m_brainOutput;

    [Space]
    [Header("Dev Debugger")]
    [SerializeField] private StateMachineData SMData;

    // Start is called before the first frame update
    void Start()
    {
        m_brainInput.navAgent = GetComponent<NavMeshAgent>();
        agent = GetComponent<NavMeshAgent>();
        m_brainInput.npc = this;
        m_brainInput.self = transform;
        m_brainInput.myDrinkSlot.user = this;

        m_brainInput.barEntranceLocation = GameObject.FindGameObjectWithTag("BarEntrance").transform;
        m_brainInput.barCounterLocation = GameObject.FindGameObjectWithTag("BarCounter").transform;
        m_brainInput.barDanceLocation = GameObject.FindGameObjectWithTag("BarDanceFloor").transform;

        NPC_SM = new SM_Idle(m_brainInput, m_brainOutput);
    }

    // Update is called once per frame
    void Update()
    {
        NPC_SM = NPC_SM.Process();
        SMData = NPC_SM.GetStateMachineData();


        //Test
        //if (Input.GetKeyDown(KeyCode.Space))
        //    GrantAccessToBar();
    }




    /// <summary>
    /// change NPC Bar Access state to Granted 
    /// </summary>
    public void GrantAccessToBar()
    {
        NPC_BarManager.Instance.RemoveFromQuene(this);
        NPC_SM.ChangeBarAccessState(SM_BarAccessState.Granted);
    }


    /// <summary>
    /// Decline NPC access
    ///
    /// </summary>
    public void DeclineAccessToBar()
    {
        if (NPC_BarManager.Instance != null)
            NPC_BarManager.Instance.RemoveFromQuene(this);

        Destroy(this.gameObject);
    }


    public void UpdateRowdiness(float rate)
    {
        m_brainInput.rowdinessBehaviour = rate;
    }

    public void UpdateMoveTo(Vector3 pos)
    {
        if (agent.isStopped)
            agent.isStopped = false;

        agent.SetDestination(pos);
    }

    public void UpdateEntranceBasedOnQuene(Vector3 pos)
    {
        m_brainInput.barEntranceLocation.position = pos;
        //Not Ideal, but it works
        //UpdateMoveTo(pos);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, 0.5f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);



        //Debug Entrance Radius
        if(m_brainInput != null)
        {
            if (!m_brainInput.barEntranceLocation && !m_brainInput.barDanceLocation && !m_brainInput.barCounterLocation)
                return;

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(m_brainInput.barEntranceLocation.position, m_brainInput.reachedEntranceTreashold);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(m_brainInput.barDanceLocation.position, m_brainInput.danceFloorTreshold);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(m_brainInput.barCounterLocation.position, m_brainInput.reachedEntranceTreashold);



            Handles.color = Color.red;
            Handles.Label(transform.position + (4.0f * Vector3.up), m_brainInput.rowdinessBehaviour.ToString("0.0"));


        }
        
    }
}
