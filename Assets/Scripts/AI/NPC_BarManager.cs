using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public struct DrinkSlot
{
    public bool inUse;
    public Vector3 position;
    public NPC user;
}

public class NPC_BarManager : MonoBehaviour
{
    public static NPC_BarManager Instance;

    [Header("General")]
    [SerializeField] private List<NPC> m_AllNPCs;

    [Header("Entrance Quene")]
    [SerializeField] private List<NPC> m_NPCWaitInQuene;
    [SerializeField, Range(0, 20)] private int m_numOfQueneLocationSlot = 10;
    [SerializeField] private Transform queneMainEntrance;
    [SerializeField] private Vector3 queneDirection = Vector3.forward;
    [SerializeField, Range(0.0f, 10.0f)] private float queneOffset = 0.2f;

    [Header("Dance EQS system")]
    [SerializeField] private DanceFloorEQS m_danceFloorEQS;

    public DanceFloorEQS GetDanceEQS
    {
        get { return m_danceFloorEQS; }
    }

    [Space]
    [Header("Bar Counter Quene, (USE ONLY IN TOOLSTIME)")]
    [Tooltip("Dont not change the Drinking Slot value on runtime, If so check Update slot system. But do it in your own digression, might affect NPCs behaviour")]
    [SerializeField] private List<DrinkSlot> m_NPCDrinkWaiting = new List<DrinkSlot>();
    [SerializeField, Range(0, 20)] private int m_numDrinkWaitSlots;
    [SerializeField] private Transform drinkStartWait;
    [SerializeField, Range(0.0f, 5.0f)] private float drinkWaitOffset;
    [SerializeField] private Vector3 drinkWaitStartDir = Vector3.right;
    [SerializeField, Range(0.0f, 20.0f)] private float lengthForNextDir = 5.0f;
    [SerializeField] private Vector3 drinkWaitNextDir = Vector3.forward;
    [SerializeField] private bool updateSlotSystem = false;

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

    private void Start()
    {
        m_danceFloorEQS.StartModule();
        UpdateDrinkSlotStation();
    }

    /// <summary>
    /// Test testing
    /// </summary>
    private void Update()
    {
        m_danceFloorEQS.UpdateModule();

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

        if (Input.GetKey(KeyCode.M))
            m_danceFloorEQS.UpdateNPCsUsingEQS();

        if (updateSlotSystem)
            UpdateDrinkSlotStation();
    }

    #region Entrance Manager


    public void GrantAccessFirstNPCInQuene()
    {
        if (m_NPCWaitInQuene.Count > 0)
        {
            m_NPCWaitInQuene[0].GrantAccessToBar();
            UpdateNPCsPos();
        }
    }

    public void DeclineAccessToFirstNPC()
    {
        if (m_NPCWaitInQuene.Count > 0)
        {
            m_NPCWaitInQuene[0].DeclineAccessToBar();
            UpdateNPCsPos();
        }
    }



    public void AddToCollection(NPC npc)
    {
        if (!m_AllNPCs.Contains(npc))
            m_AllNPCs.Add(npc);
    }


    public void AddToQuene(NPC npc)
    {
        if (!m_NPCWaitInQuene.Contains(npc))
        {
            m_NPCWaitInQuene.Add(npc);
            LastPosQueneEntrance();
        }
    }


    public void RemoveFromQuene(NPC npc)
    {
        if (m_NPCWaitInQuene.Contains(npc))
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


    private void UpdateNPCsPos()
    {
        if (m_NPCWaitInQuene.Count <= 0)
            return;

        for (int i = 0; i < m_NPCWaitInQuene.Count; ++i)
        {
            if (i > m_numOfQueneLocationSlot)
            {
                m_NPCWaitInQuene[i].UpdateMoveTo(queneMainEntrance.position + (queneDirection * m_numOfQueneLocationSlot * queneOffset));
                continue;
            }

            m_NPCWaitInQuene[i].UpdateMoveTo(queneMainEntrance.position + (queneDirection * i * queneOffset));


        }
    }
    #endregion


    public bool AddWaitingForDrinks(ref DrinkSlot drinkSlot)
    {

        foreach(DrinkSlot availableSlot in m_NPCDrinkWaiting)
            if(availableSlot.user == drinkSlot.user)
                return true;


        for (int i = 0; i < m_NPCDrinkWaiting.Count; i++)
        {
            if (!m_NPCDrinkWaiting[i].inUse)
            {

                DrinkSlot slot = new DrinkSlot();
                slot.inUse = true;
                slot.position = m_NPCDrinkWaiting[i].position;
                slot.user = drinkSlot.user;
                m_NPCDrinkWaiting[i] = slot;
                drinkSlot = slot;

                //drinkSlot = m_NPCDrinkWaiting[i];
                //m_NPCDrinkWaiting[i].inUse = true;
                return true;
            }
        }
        return false;
    }

    public void RemoveServedNPC(DrinkSlot drinkSlot)
    {

        for (int i = 0; i <= m_NPCDrinkWaiting.Count; i++)
        {
            if (m_NPCDrinkWaiting[i].user == drinkSlot.user)
            {
                DrinkSlot slot = new DrinkSlot();
                slot.inUse = false;
                slot.position = m_NPCDrinkWaiting[i].position;
                //slot.user = null;
                m_NPCDrinkWaiting[i] = slot;
                return;
            }
        }

    }

    public void UpdateDrinkSlotStation()
    {
        m_NPCDrinkWaiting.Clear();

        //First Slot Location
        DrinkSlot drinkSlot1 = new DrinkSlot();
        drinkSlot1.position = drinkStartWait.position;
        drinkSlot1.inUse = false;
        drinkSlot1.user = null;
        m_NPCDrinkWaiting.Add(drinkSlot1);

        //Other Extra Location Slot
        DrinkSlot drinkSlot = new DrinkSlot();
        drinkSlot.inUse = false;
        drinkSlot.user = null;
        int hackSwitchOffset = 1;
        for (int i = 1; i < m_numDrinkWaitSlots; i++)
        {
            Vector3 newPos = drinkStartWait.position + (drinkWaitStartDir * i * drinkWaitOffset);
            if (Vector3.Distance(drinkStartWait.position, newPos) > lengthForNextDir)
            {
                drinkSlot.position = drinkStartWait.position + (drinkWaitStartDir * lengthForNextDir) + (drinkWaitNextDir * hackSwitchOffset * drinkWaitOffset);
                hackSwitchOffset++;
            }
            else
                drinkSlot.position = newPos;

            m_NPCDrinkWaiting.Add(drinkSlot);
        }

        updateSlotSystem = false;
    }


    private void OnDrawGizmos()
    {
        m_danceFloorEQS.DrawDebugger();

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

        if (drinkStartWait == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(drinkStartWait.position, 0.2f);

        Gizmos.color = Color.red;
        int hackSwitchOffset = 1;
        for (int i = 1; i < m_numDrinkWaitSlots; i++)
        {
            Vector3 newPos = drinkStartWait.position + (drinkWaitStartDir * i * drinkWaitOffset);
            if (Vector3.Distance(drinkStartWait.position, newPos) > lengthForNextDir)
            {
                Gizmos.DrawWireSphere(drinkStartWait.position + (drinkWaitStartDir * lengthForNextDir) + (drinkWaitNextDir * hackSwitchOffset * drinkWaitOffset), 0.2f);
                hackSwitchOffset++;
            }
            else 
                Gizmos.DrawWireSphere(newPos, 0.2f);
        }


    }


}