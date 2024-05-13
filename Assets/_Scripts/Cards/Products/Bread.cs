using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

public class Bread : Card
{
    public override int AddedGoldFromProducts(Dictionary<string, ShopManager.ProductHolder> activeProductsDict, Customer customer)
    {
        //Per product type in the store that's not general, add 1 to the total value.
        int totalValue = 0;
        foreach (var productHolder in activeProductsDict)
        {
            if (productHolder.Value.products.Count > 0)
            {
                totalValue++;
            }
        }
        Debug.Log("Bread added " + totalValue + " gold.");
        return totalValue;
    }
}
