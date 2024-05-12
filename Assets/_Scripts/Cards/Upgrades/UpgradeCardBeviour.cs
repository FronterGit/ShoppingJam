using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UpgradeCardBeviour
{
    public Dictionary<string, ShopManager.ProductHolder> GetNewActiveProductsDict(Dictionary<string, ShopManager.ProductHolder> activeProductsDict);
    public void ApplyUpgrade();
}
