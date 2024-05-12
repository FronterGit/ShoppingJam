using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

public class GeneralSizeUpgrade : Card, UpgradeCardBeviour
{
    public override int AddedGoldFromProducts(Dictionary<string, ShopManager.ProductHolder> activeProductsDict, Customer customer)
    {
        //No effect.
        return 0;
    }

    public Dictionary<string, ShopManager.ProductHolder> GetNewActiveProductsDict(Dictionary<string, ShopManager.ProductHolder> activeProductsDict)
    {
        //No effect.
        return activeProductsDict;
    }
    
    public void ApplyUpgrade()
    {
        //No effect.
    }
}
