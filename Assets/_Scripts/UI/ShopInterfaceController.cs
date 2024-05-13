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
    
    //Customer preview UI variables
    [SerializeField] public GameObject customerPreviewPrefab;
    [SerializeField] public GameObject customerPreviewColumn;
    [SerializeField] public TMPro.TextMeshProUGUI customerPreviewTurnCountValue;
    
    //Revenue UI variables
    [SerializeField] public TMPro.TextMeshProUGUI revenueValue;
    [SerializeField] public TMPro.TextMeshProUGUI regionalTurn;
    [SerializeField] public TMPro.TextMeshProUGUI regionalExpected;
    [SerializeField] public TMPro.TextMeshProUGUI totalTurns;
    
    //Other UI
    [SerializeField] public Image promptBackGround;
    [SerializeField] public TMPro.TextMeshProUGUI notEnoughMoneyText;
    [SerializeField] public TMPro.TextMeshProUGUI notEnoughSpaceText;
    [SerializeField] public TMPro.TextMeshProUGUI notEnoughEnergyText;

    private void OnEnable()
    {
        //The ShopManager will raise this event when a card is added or removed from play
        //It is also raised when the game starts, so we can update the entire UI
        EventBus<UpdateShopUIEvent>.Subscribe(UpdateShopInterface);
        EventBus<UpdateCustomerPreviewEvent>.Subscribe(UpdateCustomerPreview);
        EventBus<ErrorPromptEvent>.Subscribe(OnErrorPrompt);

    }
    
    private void OnDisable()
    {
        EventBus<UpdateShopUIEvent>.Unsubscribe(UpdateShopInterface);
        EventBus<UpdateCustomerPreviewEvent>.Unsubscribe(UpdateCustomerPreview);
        EventBus<ErrorPromptEvent>.Unsubscribe(OnErrorPrompt);
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
        EventBus<UpdateCustomerPreviewEvent>.Raise(new UpdateCustomerPreviewEvent());
        EventBus<UpdateShopUIEvent>.Raise(new UpdateShopUIEvent(null));
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
            foreach (var key in ShopManager.GetActiveProductsDictFunc.Invoke().Keys)
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
            
            //Update the revenue values
            
            //Set the regional turn value
            revenueValue.text = ShopManager.GetRevenueFunc?.Invoke().ToString();
            
            //See when the regional manager is coming and how much revenue is expected
            List<Turn> turns = TurnManager.GetTurns();
            int currentTurn = TurnManager.turnIndex;
            for (int i = currentTurn; i < turns.Count; i++)
            {
                if (turns[i].shouldSpawnManager)
                {
                    int turn = i + 1;
                    regionalTurn.text = turn.ToString();
                    regionalExpected.text = turns[i].expectedRevenue.ToString();
                    break;
                }
            }
            
            //Set the total turns
            totalTurns.text = turns.Count.ToString();
 
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
    
    private void UpdateCustomerPreview(UpdateCustomerPreviewEvent e)
    {
        //Clear the customer preview column
        foreach (Transform child in customerPreviewColumn.transform)
        {
            Destroy(child.gameObject);
        }
        
        List<GameObject> customerList = new List<GameObject>();
        customerList = CustomerManager.customersToSpawn;
        
        List<CustomerPreview> customerPreviews = new List<CustomerPreview>();
        
        foreach (var customer in customerList)
        {
            CustomerPreview customerPreview = Instantiate(customerPreviewPrefab, customerPreviewColumn.transform).GetComponent<CustomerPreview>();
            customerPreview.customerName = customer.GetComponent<Customer>().customerType.ToString();
            customerPreview.customerAmount = 1;
            customerPreviews.Add(customerPreview);
        }
        
        //Loop over customerPreviews and delete duplicates, incrementing the amount of the first entry with the same type
        for (int i = 0; i < customerPreviews.Count; i++)
        {
            for (int j = i + 1; j < customerPreviews.Count; j++)
            {
                if (customerPreviews[i].customerName == customerPreviews[j].customerName)
                {
                    customerPreviews[i].customerAmount++;
                    Destroy(customerPreviews[j].gameObject);
                    customerPreviews.RemoveAt(j);
                    j--;
                }
            }
        }
        
        //Update the turn count
        int turnIndex = TurnManager.turnIndex + 1;
        customerPreviewTurnCountValue.text = turnIndex.ToString();
    }

    public void OnErrorPrompt(ErrorPromptEvent e)
    {
        switch (e.errorType)
        {
            case ErrorPromptEvent.ErrorType.NotEnoughMoney:
                notEnoughMoneyText.gameObject.SetActive(true);
                break;
            case ErrorPromptEvent.ErrorType.NotEnoughSpace:
                notEnoughSpaceText.gameObject.SetActive(true);
                break;
            case ErrorPromptEvent.ErrorType.NotEnoughEnergy:
                notEnoughEnergyText.gameObject.SetActive(true);
                break;
        }
        
        promptBackGround.gameObject.SetActive(true);
        StartCoroutine(HideErrorPrompt());
    }
    
    public IEnumerator HideErrorPrompt()
    {
        yield return new WaitForSeconds(2f);
        notEnoughMoneyText.gameObject.SetActive(false);
        notEnoughSpaceText.gameObject.SetActive(false);
        notEnoughEnergyText.gameObject.SetActive(false);
        promptBackGround.gameObject.SetActive(false);
    }
    
    
}
