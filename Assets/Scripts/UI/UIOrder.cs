using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOrder : MonoBehaviour
{
    [SerializeField] GameObject orderSpeachBubble;
    [SerializeField] Transform beverageIconParent;
    [SerializeField, NamedArrayAttribute(new string[] {"Kerosene", "Diesel", "Petrol", "Battery" })] GameObject[] beverageIconPrefabs;

    [Header("Read Only")]
    [SerializeField] List<RequestedBeverages> requestedBeveragesList = new List<RequestedBeverages>();

    public event Action OnFinishedDrinkingEvent;

    public struct RequestedBeverages
    {
        public GameObject beverageIconObject;
        public Beverages beverage;
    }

    public void RequestBeverages(Beverages beverageOne, int beverageOneCount = 1, Beverages beverageTwo = Beverages.None, int beverageTwoCount = 0, Beverages beverageThree = Beverages.None, int beverageThreeCount = 0, Beverages beverageFour = Beverages.None, int beverageFourCount = 0 )
    {
        orderSpeachBubble.SetActive(true);

        SpawnBeveragesOfType(beverageOne, beverageOneCount);
        SpawnBeveragesOfType(beverageTwo, beverageTwoCount);
        SpawnBeveragesOfType(beverageThree, beverageThreeCount);
        SpawnBeveragesOfType(beverageFour, beverageFourCount);
    }

    public void SpawnBeveragesOfType(Beverages beverage, int count)
    {
        if(beverage == Beverages.None ) return;
        if (orderSpeachBubble.activeSelf == false) orderSpeachBubble.SetActive(true);
        
        RequestedBeverages request = new RequestedBeverages();
        request.beverage = beverage;
        for(int i = 0; i < count; i++)
        {
            request.beverageIconObject = Instantiate(beverageIconPrefabs[(int)beverage], beverageIconParent);
            requestedBeveragesList.Add(request);
        }
    }

    public void Satisfied()
    {
        requestedBeveragesList.Clear();
        orderSpeachBubble.SetActive(false);

        OnFinishedDrinkingEvent?.Invoke();
    }

    public void RemoveDrinkFromList(Beverages beverage)
    {
        if (requestedBeveragesList == null || requestedBeveragesList.Count <= 0)
        {
            Satisfied();
            return;
        }

        for(int i = 0; i < requestedBeveragesList.Count; i++)
        {
            if (requestedBeveragesList[i].beverage == beverage)
            {
                Destroy(requestedBeveragesList[i].beverageIconObject);
                requestedBeveragesList.RemoveAt(i);
                return;
            }
        }

        if (requestedBeveragesList == null || requestedBeveragesList.Count <= 0)
        {
            Satisfied();
            return;
        }
    }
}

public enum Beverages {Kerosene, Diesel, Petrol, Battery, None}

public class NamedArrayAttribute : PropertyAttribute
{
    public readonly string[] names;
    public NamedArrayAttribute(string[] names) { this.names = names; }
}