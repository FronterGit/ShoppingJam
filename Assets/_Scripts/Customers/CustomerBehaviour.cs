using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CustomerBehaviour
{
    public Dictionary<string, ShopManager.ProductHolder> extraProductsDictionary { get; set; }
    void Buy(int gold);
}
