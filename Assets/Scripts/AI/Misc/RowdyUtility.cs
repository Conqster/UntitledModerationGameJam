using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RowdyUtility
{
    

    public void ReduceRowdiness(ref float rowdiness, float tendency)
    {

        float reduceRate = (1.0f - tendency) * Reduction();
        rowdiness = rowdiness - (rowdiness * reduceRate);
        rowdiness = Mathf.Clamp01(rowdiness);
    }

    public void IncreaseRowdiness(ref float rowdiness, float tendency)
    {
        float incRate = (1.0f - tendency) * Increase();
        rowdiness = rowdiness + (tendency * incRate);
        rowdiness = Mathf.Clamp01(rowdiness);
    }


    public void IncreaseRowdinessNotGettingDrinks(ref float rowdiness, float tendency)
    {
        float incRate = (1.0f - tendency) * Reduction();
        rowdiness = rowdiness + (tendency * incRate);
        rowdiness = Mathf.Clamp01(rowdiness);
    }


    public void IncreaseRowinessFromDrink(ref float rowdiness, float tendency)
    {
        float incRate = (1.0f - tendency) * Increase();
        rowdiness = rowdiness + (tendency * incRate);
        rowdiness = Mathf.Clamp01(rowdiness);
    }


    private float Reduction()
    {
        float reduce = 0.25f;
        if (NPC_BarManager.Instance != null)
            reduce = NPC_BarManager.Instance.GlobalReduceRowdyUtil;

        return reduce;
    }

    private float Increase()
    {
        float inc = 0.5f;

        if (NPC_BarManager.Instance != null)
            inc = NPC_BarManager.Instance.GlobalIncRowdyUtil;

        return inc;
    }

}
