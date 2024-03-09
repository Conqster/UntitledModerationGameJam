using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawn : MonoBehaviour
{
    [SerializeField] private GameObject m_SpawnObject;
    [SerializeField, Range(0.5f, 5.0f)] private float m_SpawnInterval = 1.0f;
    [SerializeField, Range(0,10)] private int m_MaxSpawnCount = 2;
    [SerializeField] private int m_SpawnCount = 0;
    [SerializeField] Transform m_SpawnLocation;

    public float testValue;

    private void Start()
    {
        StartCoroutine(Spawner());
    }



    private IEnumerator Spawner()
    {
        while (m_SpawnCount < m_MaxSpawnCount)
        {
            GameObject spawnObj = Instantiate(m_SpawnObject, m_SpawnLocation);
            spawnObj.AddComponent<IdHolder>();
            float rate = (testValue == 0) ? 1 : 0;

            testValue = rate;

            if (spawnObj.TryGetComponent<NPC>(out NPC npc))
            {
                npc.UpdateRowdiness(rate);
                
            }
                
            if (NPC_BarManager.Instance != null)
                NPC_BarManager.Instance.AddToCollection(npc);
                    
            m_SpawnCount++;
            yield return new WaitForSeconds(m_SpawnInterval);
        }
    }
}
