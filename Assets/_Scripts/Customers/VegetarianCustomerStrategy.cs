using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;
using EventBus;

public class VegetarianCustomerStrategy : CustomerBehaviour
{
    public Dictionary<string, ShopManager.ProductHolder> extraProductsDictionary { get; set; }
    
    public VegetarianCustomerStrategy(Dictionary<string, ShopManager.ProductHolder> activeProductsDict)
    {
        this.extraProductsDictionary = activeProductsDict;
    }
    public void Buy(int goldToAdd)
    {
        //Get all active products
        Dictionary<string, ShopManager.ProductHolder> activeProductsDict = ShopManager.GetActiveProductsDictFunc?.Invoke();
        
        //Calculate the total value of all active products
        int totalValue = 0;
        foreach (var productHolder in activeProductsDict)
        {
            //Loop over the products in the product holder and add their value to the total value
            foreach (var product in productHolder.Value.products)
            {
                if (product.productInfo.productType == Card.ProductType.Vegetable)
                    totalValue += product.productInfo.productValue * 2;
                else if (product.productInfo.productType != Card.ProductType.Meat)
                    totalValue += product.productInfo.productValue;
            }
        }
        
        //Also loop over the extra products dictionary and add the value of the products to the total value
        foreach (var productHolder in extraProductsDictionary)
        {
            foreach (var product in productHolder.Value.products)
            {
                totalValue += product.productInfo.productValue;
            }
        }
        
        //Add the total value to the shop's money
        EventBus<ChangeMoneyEvent>.Raise(new ChangeMoneyEvent(totalValue + goldToAdd, true));
    }
}
