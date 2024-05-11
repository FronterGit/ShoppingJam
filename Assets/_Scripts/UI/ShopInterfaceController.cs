using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;
using UnityEngine.UI;
using Cards;

public class ShopInterfaceController : MonoBehaviour
{
    //Menu control variables
    [SerializeField] private List<GameObject> shopMenus;
    private int currentMenuIndex = 0;

    //Product UI variables
    [SerializeField] private GameObject productsColumn;
    [SerializeField] private GameObject productsRowPrefab;
    [SerializeField] private List<GameObject> productRows;
    private Dictionary<string, GameObject> productRowsDict = new Dictionary<string, GameObject>();
    
    [SerializeField] public GameObject customerPreviewPrefab;
    [SerializeField] public GameObject customerPreviewColumn;

    private void OnEnable()
    {
        //The ShopManager will raise this event when a card is added or removed from play
        //It is also raised when the game starts, so we can update the entire UI
        EventBus<UpdateShopUIEvent>.Subscribe(UpdateShopInterface);
    }
    
    private void OnDisable()
    {
        EventBus<UpdateShopUIEvent>.Unsubscribe(UpdateShopInterface);
    }

    void Start()
    {
        foreach (var shopMenu in shopMenus)
        {
            shopMenu.SetActive(false);
        }
        shopMenus[0].SetActive(true);
        currentMenuIndex = 0;
    }
    
    public void NavigateMenu(int index)
    {
        shopMenus[currentMenuIndex].SetActive(false);
        currentMenuIndex += index;
        if (currentMenuIndex < 0)
        {
            currentMenuIndex = shopMenus.Count - 1;
        }
        else if (currentMenuIndex >= shopMenus.Count)
        {
            currentMenuIndex = 0;
        }
        shopMenus[currentMenuIndex].SetActive(true);
    }
    
    private void UpdateShopInterface(UpdateShopUIEvent e)
    {
        Card card = e.card;
        
        //If the card is null, we will update the entire UI
        if (card == null)
        {
            //Clear the product rows
            foreach (var productRow in productRowsDict)
            {
                Destroy(productRow.Value);
            }
            productRowsDict.Clear();
            
            //Get the active products and create a row for each product type
            foreach (var key in ShopManager.activeProductsDict.Keys)
            {
                //Create a new row
                ProductsRow productsRow = Instantiate(productsRowPrefab, productsColumn.transform).GetComponent<ProductsRow>();
                
                //Set the title of the row
                productsRow.productTypeTitle.text = key;
                
                //Add the row to the dictionary
                productRowsDict.Add(key, productsRow.gameObject);
                
                //Update the products in the row
                productsRow.UpdateProducts();
            }

            //We are done updating the UI, so we can return
            return;
        }
        
        //Depending on the card category, we will update the UI accordingly
        switch (e.card.category)
        {
            //If the card is a product...
            case Card.Category.Product:
                //Look up the corresponding row in the dictionary and update the products
                ProductsRow productsRow = productRowsDict[card.productInfo.productType.ToString()].GetComponent<ProductsRow>();
                
                //Tell that row to update its products
                productsRow.UpdateProducts();
                break;
        }
    }
    
    
}
