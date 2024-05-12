using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class Tomato : Card
{
    //Tomato adds 1 gold per customer.
    public override int AddedGoldFromProducts(Dictionary<string, ShopManager.ProductHolder> activeProductsDict, Customer customer)
    {
        //No additional effect.
        return 0;
    }
}
