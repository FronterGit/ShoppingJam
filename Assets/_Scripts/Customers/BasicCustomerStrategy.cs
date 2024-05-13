using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBus;

public class BasicCustomerStrategy : CustomerBehaviour
{
    public Dictionary<string, ShopManager.ProductHolder> extraProductsDictionary { get; set; }
    public BasicCustomerStrategy(Dictionary<string, ShopManager.ProductHolder> extraProductsDictionary)
    {
        this.extraProductsDictionary = extraProductsDictionary;
    }
    
    public void Buy(int goldToAdd)
    {
        Dictionary<string, ShopManager.ProductHolder> realActiveProducts = ShopManager.GetActiveProductsDictFunc.Invoke();
        
        //Calculate the total value of all active products
        int totalValue = 0;
        foreach (var productHolder in realActiveProducts)
        {
            //Loop over the products in the product holder and add their value to the total value
            foreach (var product in productHolder.Value.products)
            {
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
