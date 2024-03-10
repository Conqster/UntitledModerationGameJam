using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[System.Serializable]
public class BarUtility 
{
    [SerializeField] private List<NPC> m_AllNpcs = new List<NPC>();
    [SerializeField] private List<NPC> m_NpcAtEntrance = new List<NPC>();
    [SerializeField] private List<NPC> m_NpcAtDrinkStation = new List<NPC>();
    [SerializeField] private List<NPC> m_NpcOnDanceFloor = new List<NPC>();


    [Header("Current Utility values")]
    [SerializeField] private float m_entranceRowdiness;
    [SerializeField] private float m_danceFloorRowdiness;
    [SerializeField] private float m_drinkStationRowdiness;
    [SerializeField] private float m_barRodiness;

    [Header("On start Utility Assign")]
    [SerializeField, Range(0.0f, 1.0f)] private float d_entranceRowdiness;
    [SerializeField, Range(0.0f, 1.0f)] private float d_danceFloorRowdiness;
    [SerializeField, Range(0.0f, 1.0f)] private float d_drinkStationRowdiness;


    [Header("Object Locations")]
    [SerializeField] private Transform entrance;
    [SerializeField] private Transform drinkStation;
    [SerializeField] private Transform danceFloor;
    [SerializeField] private Transform barCenter;


    public void OnStartModule()
    {
        m_entranceRowdiness = d_entranceRowdiness;
        m_danceFloorRowdiness = d_danceFloorRowdiness;
        m_drinkStationRowdiness = d_drinkStationRowdiness;


        m_barRodiness = (m_entranceRowdiness + m_danceFloorRowdiness + m_drinkStationRowdiness) / 3.0f;
    }


    public void OnUpdateModule()
    {
        m_entranceRowdiness = Mathf.Clamp01(m_entranceRowdiness);
        m_danceFloorRowdiness =  Mathf.Clamp01(m_danceFloorRowdiness);
        m_drinkStationRowdiness = Mathf.Clamp01(m_drinkStationRowdiness);

        m_barRodiness = Mathf.Clamp01(m_barRodiness);
    }



    public void UpdateDrinkStation(NPC npc)
    {
        if(!m_NpcAtDrinkStation.Contains(npc))
        {
            m_NpcAtDrinkStation.Add(npc);
        }
        else
            m_NpcAtDrinkStation.Remove(npc);

        AverageDrinkRowdiness();
    }

    public void UpdateNPCAtEntrance(NPC npc)
    {
        if(!m_NpcAtEntrance.Contains(npc))
            m_NpcAtEntrance.Add(npc);
        else
            m_NpcAtEntrance.Remove(npc);

        AverageEntranceRowdindess();
    }

    public void UpdateNPCsOnDance(NPC npc)
    {
        if(!m_NpcOnDanceFloor.Contains(npc))
            m_NpcOnDanceFloor.Add(npc);
        else
            m_NpcOnDanceFloor.Remove(npc);

        AverageDanceFloorRowdiness();
    }


    private void AverageEntranceRowdindess()
    {
        float total = 0;
        foreach (NPC npc in m_NpcAtEntrance)
            total += npc.GetBrainInput.rowdinessBehaviour;

        m_entranceRowdiness = total / m_NpcAtEntrance.Count;
        m_barRodiness = (m_entranceRowdiness + m_danceFloorRowdiness + m_drinkStationRowdiness) / 3.0f;
    }

    private void AverageDrinkRowdiness()
    {
        float total = 0;
        foreach (NPC npc in m_NpcAtDrinkStation)
            total += npc.GetBrainInput.rowdinessBehaviour;

        m_drinkStationRowdiness = total / m_NpcAtDrinkStation.Count;
        m_barRodiness = (m_entranceRowdiness + m_danceFloorRowdiness + m_drinkStationRowdiness) / 3.0f;
    }


    private void AverageDanceFloorRowdiness()
    {
        float total = 0;
        foreach (NPC npc in m_NpcOnDanceFloor)
            total += npc.GetBrainInput.rowdinessBehaviour;

        m_danceFloorRowdiness = total / m_NpcOnDanceFloor.Count;
        m_barRodiness = (m_entranceRowdiness + m_danceFloorRowdiness + m_drinkStationRowdiness) / 3.0f;
    }

    public void AdjustEntranceRowdy(int value)
    {
        m_entranceRowdiness += value;
        m_barRodiness = (m_entranceRowdiness + m_danceFloorRowdiness + m_drinkStationRowdiness) / 3.0f;
    }

    public void AdjustDanceFloor(float value)
    {
        m_danceFloorRowdiness += value;
        m_barRodiness = (m_entranceRowdiness + m_danceFloorRowdiness + m_drinkStationRowdiness) / 3.0f;
    }

    public void AdjustDrinkStation(float value)
    {
        m_drinkStationRowdiness += value;
        m_barRodiness = (m_entranceRowdiness + m_danceFloorRowdiness + m_drinkStationRowdiness) / 3.0f;
    }


    public void OnDebugGizmos()
    {
        Handles.Label(entrance.position, m_entranceRowdiness.ToString("0.0"));
        Handles.Label(drinkStation.position, m_drinkStationRowdiness.ToString("0.0"));
        Handles.Label(danceFloor.position, m_danceFloorRowdiness.ToString("0.0"));
        Handles.Label(barCenter.position, m_barRodiness.ToString("0.0"));
    }
}
