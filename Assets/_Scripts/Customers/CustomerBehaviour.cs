using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CustomerBehaviour
{
    public Dictionary<string, ShopManager.ProductHolder> activeProductsDict { get; set; }
    void Buy();
}
