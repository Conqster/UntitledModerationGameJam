using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeverageObject : MonoBehaviour
{
    [SerializeField] Beverages beverage;

    public Beverages Beverage { get { return beverage; } }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
