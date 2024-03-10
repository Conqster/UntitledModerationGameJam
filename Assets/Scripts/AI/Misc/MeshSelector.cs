using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSelector : MonoBehaviour
{
    [SerializeField] private GameObject[] Meshes;


    private void Awake()
    {
       GameObject body = Instantiate(Meshes[Random.Range(0, Meshes.Length)], gameObject.transform.position, Quaternion.identity);
        body.transform.parent = this.transform;
    }
}
