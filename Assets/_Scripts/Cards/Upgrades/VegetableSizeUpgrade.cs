using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;
using EventBus;

public class VegetableSizeUpgrade : Card, UpgradeCardBeviour
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
        Debug.Log("Vegetable Size Upgrade applied.");
        
        //Upgrade the size of the general product holder in the shop by 1.
        ShopManager.GetActiveProductsDictFunc.Invoke()["Vegetable"].size++;
        
        //Raise UI Update event.
        EventBus<UpdateShopUIEvent>.Raise(new UpdateShopUIEvent(null));
    }
}
