using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC_BarManager : MonoBehaviour
{
    public static NPC_BarManager Instance;
    [SerializeField] IDDisplayManager idDisplayManager;
    [SerializeField] private List<NPC> m_AllNPCs;
    [SerializeField] private List<NPC> m_NPCWaitInQuene;
    [SerializeField, Range(0, 20)] private int m_numOfQueneLocationSlot = 10;
    
    [SerializeField] private Transform queneMainEntrance;
    [SerializeField] private Vector3 queneDirection = Vector3.forward;
    [SerializeField, Range(0.0f, 10.0f)] private float queneOffset = 0.2f;

    [Header("Dev Debugger")]
    [SerializeField] private bool alwayUseLastQuene = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }


    /// <summary>
    /// Test testing
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(m_NPCWaitInQuene.Count > 0)
            {
                m_NPCWaitInQuene[0].GrantAccessToBar();
            }
            UpdateNPCsPos();
        }

        if(Input.GetKey(KeyCode.R))
            UpdateNPCsPos();
    }


    public void GrantAccessFirstNPCInQuene()
    {
        if (m_NPCWaitInQuene.Count > 0)
        {
            m_NPCWaitInQuene[0].GrantAccessToBar();
            DisplayNextNPCInfoOrClear();
            UpdateNPCsPos();
        }
    }

    public void DeclineAccessToFirstNPC()
    {
        if (m_NPCWaitInQuene.Count > 0)
        {
            m_NPCWaitInQuene[0].DeclineAccessToBar();
            UpdateNPCsPos();
            DisplayNextNPCInfoOrClear();
        }
    }


    private void DisplayNextNPCInfoOrClear()
    {
        if (m_NPCWaitInQuene.Count > 0)
        {
            UpdateIDCardWithNPCData(m_NPCWaitInQuene[0]);
        }
        else
        {
            idDisplayManager.ClearIDInfo();
        }
    }
    public void AddToCollection(NPC npc)
    {
        if(!m_AllNPCs.Contains(npc))
            m_AllNPCs.Add(npc);
    }


    public void AddToQuene(NPC npc)
    {
        if(!m_NPCWaitInQuene.Contains(npc))
        {
            m_NPCWaitInQuene.Add(npc);
            LastPosQueneEntrance();
        }
        if (m_NPCWaitInQuene.Count == 1)
        {
            UpdateIDCardWithNPCData(npc);
        }
    }

    public void RemoveFromQuene(NPC npc)
    {
        if(m_NPCWaitInQuene.Contains(npc))
        {
            m_NPCWaitInQuene.Remove(npc);
            LastPosQueneEntrance();
        }
    }

    private void LastPosQueneEntrance()
    {
        if (alwayUseLastQuene)
            foreach (var npc in m_AllNPCs)
                npc.UpdateEntranceBasedOnQuene(queneMainEntrance.position + Vector3.up + (queneDirection * (m_NPCWaitInQuene.Count) * queneOffset));
    }


    private void OnDrawGizmos()
    {

        if (queneMainEntrance == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(queneMainEntrance.position, 0.2f);

        Gizmos.color = Color.red;
        for(int i = 1; i < m_numOfQueneLocationSlot; i++)
        {
            Gizmos.DrawWireSphere(queneMainEntrance.position + (queneDirection * i * queneOffset), 0.2f);
        }


        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(queneMainEntrance.position + (queneDirection * (m_numOfQueneLocationSlot) * queneOffset), 0.2f);


        //current open slot
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(queneMainEntrance.position + Vector3.up + (queneDirection * (m_NPCWaitInQuene.Count) * queneOffset), 0.2f);
    }
    //public void CreateNPC(GameObject spawnObject, Transform spawnLocation)
    //{
    //    GameObject spawnObj = Instantiate(spawnObject, spawnLocation);
    //}
    private void UpdateIDCardWithNPCData(NPC npc)
    {
        var idHolder = npc.GetComponent<IdHolder>();
        if (idHolder != null)
        {
            // Set the data on the ID card
            idDisplayManager.SetIDInfo(idHolder.image, idHolder.expiryDate, idHolder.dateOfBirth);
        }
        else
        {
            Debug.LogError("NPC does not have an IdHolder component.");
        }
    }
    private void UpdateNPCsPos()
    {
        if (m_NPCWaitInQuene.Count <= 0)
            return;

        for(int i = 0; i < m_NPCWaitInQuene.Count; ++i)
        {
            if(i > m_numOfQueneLocationSlot)
            {
                m_NPCWaitInQuene[i].UpdateMoveTo(queneMainEntrance.position + (queneDirection * m_numOfQueneLocationSlot * queneOffset));
                continue;
            }

            m_NPCWaitInQuene[i].UpdateMoveTo(queneMainEntrance.position + (queneDirection * i * queneOffset));


        }
    }
}
