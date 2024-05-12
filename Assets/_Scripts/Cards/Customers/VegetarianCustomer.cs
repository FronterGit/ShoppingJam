using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class Vegetarian : Card
{
    public override int AddedGoldFromProducts(Dictionary<string, ShopManager.ProductHolder> activeProductsDict, Customer customer)
    {
        throw new System.NotImplementedException();
    }
}
