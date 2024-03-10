using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posts
{
    public Vector3 post;
    public bool inUse = false;
}


[System.Serializable]   
public class DanceFloorEQS 
{
    [Header("System")]
    [SerializeField] private List<Vector3> EQSPost = new List<Vector3>();
    [SerializeField] private List<NPC> NpcsUsingAPost = new List<NPC>();
    [SerializeField] private int numOfPostGivenOut = 0;
    [SerializeField] private bool systemRunning = false;
    [SerializeField] private bool reloadEQS = false;
    //[SerializeField] List<Posts> EQSPosts = new List<Posts>();

    [Header("EQS Properties")]
    [SerializeField, Range(2, 10)] private int numOfCardinalPoints = 4;
    [SerializeField, Range(1, 10)] private int numOfRings = 1;
    [SerializeField, Range(0.0f, 5.0f)] private float distanceBtwRings = 0.5f;
    [SerializeField] private Transform centerLocation;


    [Header("Dev Debugger")]
    [SerializeField] private Color postDebugColour = Color.blue;
    [SerializeField] private Color postCenterDebugColour = Color.red;
    [SerializeField] private bool useWireGizmos = false;
    [SerializeField, Range(0.0f, 1.0f)] private float postRadius = 0.2f;


    public bool QueryAPost(NPC requester, out Vector3 position)
    {
        position = Vector3.zero;

        if (numOfPostGivenOut == EQSPost.Count)
            return false;

        //Just in case an Npc requests twice an already has a post
        if (NpcsUsingAPost.Contains(requester))
            return true;

        position = EQSPost[numOfPostGivenOut++];
        NpcsUsingAPost.Add(requester);

        return true;
    }

    public void ReleaseAPost(NPC npc)
    {
        if(NpcsUsingAPost.Contains(npc))
        {
            NpcsUsingAPost.Remove(npc);
            //manually adjust the other npcs
            //UnityEngine.MonoBehaviour.Invoke("UpdateNPCsUsingEQS", 1.0f);
            //Invoke("UpdateNPCsUsingEQS", 1.0f);
            UpdateNPCsUsingEQS();
        }
    }



    public void StartModule()
    {
        systemRunning = false;
        RegenerateEQS();
    }

    public void UpdateModule()
    {
        if (reloadEQS)
            RegenerateEQS();


        systemRunning = true;
    }


    public void UpdateNPCsUsingEQS()
    {
        for(int i = 0; i < NpcsUsingAPost.Count; i++)
        {
            NpcsUsingAPost[i].UpdateMoveTo(EQSPost[i]);
        }
    }

    private void RegenerateEQS()
    {
        EQSPost.Clear();


        float angle = 360.0f / numOfCardinalPoints;

        for (int i = 0; i < numOfRings; i++)
        {

            for (float j = 0; j < 360; j += (angle / ((2 * (i) - 2) + 3)))
            {
                Vector3 pos = centerLocation.transform.position + Quaternion.Euler(0.0f, j, 0.0f) * (centerLocation.transform.forward * distanceBtwRings * (i + 1));

                //add position into post collection 
                EQSPost.Add(pos);

            }
        }
    }


    public void DrawDebugger()
    { 

        if(centerLocation == null)
            return;

        Gizmos.color = postCenterDebugColour;


        if (useWireGizmos)
            Gizmos.DrawWireSphere(centerLocation.position, postRadius);
        else
            Gizmos.DrawSphere(centerLocation.position, postRadius);

        Gizmos.color = postDebugColour;

        if (systemRunning)
        {
            foreach(Vector3 post in EQSPost)
                if (useWireGizmos)
                    Gizmos.DrawWireSphere(post, postRadius);
                else
                    Gizmos.DrawSphere(post, postRadius);

            for(int i = 0; i < numOfRings; i++)
                DrawWireCircle(centerLocation.transform.position, distanceBtwRings * (i + 1));
        }
        else
        {
            float angle = 360.0f / numOfCardinalPoints;

            for (int i = 0; i < numOfRings; i++)
            {

                for (float j = 0; j < 360; j += (angle / ((2 * (i) - 2) + 3)))
                {
                    Vector3 pos = centerLocation.transform.position + Quaternion.Euler(0.0f, j, 0.0f) * (centerLocation.transform.forward * distanceBtwRings * (i + 1));

                    if (useWireGizmos)
                        Gizmos.DrawWireSphere(pos, postRadius);
                    else
                        Gizmos.DrawSphere(pos, postRadius);

                }

                DrawWireCircle(centerLocation.transform.position, distanceBtwRings * (i + 1));
            }
        }


    }

    private void DrawWireCircle(Vector3 center, float radius)
    {
        float stepSize = 0.1f;
        float theta = 0f;
        Vector3 previousPoint = center + new Vector3(radius, 0f, 0f);

        while (theta < 2 * Mathf.PI)
        {
            float x = center.x + radius * Mathf.Cos(theta);
            float z = center.z + radius * Mathf.Sin(theta);
            Vector3 currentPoint = new Vector3(x, center.y, z);

            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
            theta += stepSize;
        }

        Gizmos.DrawLine(previousPoint, center + new Vector3(radius, 0f, 0f));
    }

}
