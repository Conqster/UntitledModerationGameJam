using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;

public class RandomChildColour : MonoBehaviour
{
    private void Awake()
    {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
        var rends = GetComponentsInChildren<Renderer>();
        for(int i = 0; i < rends.Length; i++)
        {

            block.SetColor("_Color", new Color(Random.value, Random.value, Random.value, rends[i].material.color.a * 0.1f));
            rends[i].SetPropertyBlock(block);
            //rend.material.color = new Color(Random.value, Random.value, Random.value, 0.1f);
        }
    }
}
