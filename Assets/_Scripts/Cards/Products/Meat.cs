using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class Meat : Card
{
    public override int AddedGoldFromProducts(Dictionary<string, ShopManager.ProductHolder> activeProductsDict, Customer customer)
    {
        //Beef adds 1 gold per customer.
        return 1;
    }
}
