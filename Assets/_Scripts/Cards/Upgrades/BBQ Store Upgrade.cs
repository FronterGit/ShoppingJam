using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

public class BBQStoreUpgrade : Card, UpgradeCardBeviour{

    public Dictionary<string, ShopManager.ProductHolder> GetNewActiveProductsDict(Dictionary<string, ShopManager.ProductHolder> activeProductsDict)
    {
        List<Card> productsToAdd = new List<Card>();
        
        //Loop over all active products
        foreach (var productHolder in activeProductsDict)
        {
            //Loop over all products in the product holder
            foreach (var product in productHolder.Value.products)
            {
                //If the product is a meat product, clone it
                if (product.productInfo.productType == ProductType.Meat)
                {
                    productsToAdd.Add(product);
                }
            }
        }
        
        //Loop over all cloned products and add them to the active products dictionary
        foreach (var product in productsToAdd)
        {
            activeProductsDict[product.productInfo.productType.ToString()].products.Add(product);
            Debug.Log("Cloned product added to active products dictionary with type: " + product.productInfo.productType);
        }
        
        
        return activeProductsDict;
    }

    public override int AddedGoldFromProducts(Dictionary<string, ShopManager.ProductHolder> activeProductsDict, Customer customer)
    {
        throw new System.NotImplementedException();
    }
    
    public void ApplyUpgrade()
    {
        //No effect.
    }
}
