using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

public class BBQStoreUpgrade : Card, UpgradeCardBeviour{
    public override void CardAction()
    {
        Debug.Log("BBQ Store Upgrade card action");
    }

    public Dictionary<string, ShopManager.ProductHolder> GetNewActiveProductsDict(Dictionary<string, ShopManager.ProductHolder> activeProductsDict)
    {
        Debug.Log("BBQ Store Upgrade doing stuff with active products dict");
        return activeProductsDict;
    }
}
