using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class Avocado : Card
{
    public override int AddedGoldFromProducts(Dictionary<string, ShopManager.ProductHolder> activeProductsDict, Customer customer)
    {
        if(customer.customerType == Customer.CustomerType.Vegetarian)
        {
            return 6;
        }

        return 2;
    }
}
