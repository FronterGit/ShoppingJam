using System.Collections;
using System.Collections.Generic;
using Cards;
using EventBus;
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
        Debug.Log("General Size Upgrade applied.");
        
        //Upgrade the size of the general product holder in the shop by 1.
        ShopManager.activeProductsDict["General"].size++;
        
        //Raise UI Update event.
        EventBus<UpdateShopUIEvent>.Raise(new UpdateShopUIEvent(null));
    }
}
