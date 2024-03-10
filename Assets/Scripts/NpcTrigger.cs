using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcTrigger : MonoBehaviour
{
    [SerializeField] UIOrder uiOrder;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Beverage"))
        {
            if(other.TryGetComponent( out BeverageObject beverageObject))
            {
                if(uiOrder != null)
                {
                    Debug.Log("Got Drink");
                    uiOrder.RemoveDrinkFromList(beverageObject.Beverage);
                    Destroy(other.gameObject, 0.1f);
                }
            }
        }
    }

    // Debugging
    private void Start()
    {
        uiOrder.RequestBeverages(Beverages.Battery, 2, Beverages.Kerosene, 1);
    }
}
