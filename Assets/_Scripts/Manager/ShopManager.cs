using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using EventBus;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    //RESPONSIBILITIES:
    // 1. Keep track of the shop's resources
    // 2. Keep track of the shop's active products, employees, and upgrades
    // 3. Keep track of the shop's customers
    
    public int customers;
    public int totalProductsValue;
    public int money;
    public List<Customer> customersList = new List<Customer>();
    
    public List<StartingProducts> startingProducts;
    public static Dictionary<string, ProductHolder> activeProductsDict;
    public static List<Card> activeEmployees = new List<Card>();
    public static List<Card> activeUpgrades = new List<Card>();
    
    public static event System.Action<int> OnMoneyChanged;
    public static event System.Action<int> OnCustomersChanged;

    private void OnEnable()
    {
        Shop.OnCustomer += OnCustomer;
        TheMoneyHouse.ShopClicked += OnClick;
        EventBus<CardEvent>.Subscribe(ReceiveCard);
        EventBus<CardEvent>.Subscribe(RemoveCard);
        EventBus<ChangeMoneyEvent>.Subscribe(OnChangeMoney);
    }
    
    private void OnDisable()
    {
        Shop.OnCustomer -= OnCustomer;
        TheMoneyHouse.ShopClicked -= OnClick;
        EventBus<CardEvent>.Unsubscribe(ReceiveCard);
        EventBus<CardEvent>.Unsubscribe(RemoveCard);
        EventBus<ChangeMoneyEvent>.Unsubscribe(OnChangeMoney);
    }

    void Start()
    {
        EventBus<MoneyChangedEvent>.Raise(new MoneyChangedEvent(money));

        InitializeActiveProductsDict();
    }
    
    private void InitializeActiveProductsDict()
    {
        activeProductsDict = new Dictionary<string, ProductHolder>();
        foreach(var product in startingProducts)
        {
            activeProductsDict.Add(product.productType.ToString(), new ProductHolder(product.amount, new List<Card>()) );
        }
        EventBus<UpdateShopUIEvent>.Raise(new UpdateShopUIEvent(null));
    }

    public void OnClick()
    {
        money += customers * totalProductsValue;
        EventBus<MoneyChangedEvent>.Raise(new MoneyChangedEvent(money));
    }
    
    public void OnCustomer(GameObject customer, bool isEntering)
    {
        if (isEntering)
        {
            customersList.Add(customer.GetComponent<Customer>());
        }
        else
        {
            customersList.Remove(customer.GetComponent<Customer>());
        }
        customers = customersList.Count;
        OnCustomersChanged?.Invoke(customers);
    }
    
    private void OnChangeMoney(ChangeMoneyEvent e)
    {
        money += e.money;
    }
    
    private void ReceiveCard(CardEvent e)
    {
        if (!e.open) return;
        switch (e.card.category)
        {
            case Card.Category.Product:
                activeProductsDict[e.card.productInfo.productType.ToString()].products.Add(e.card);
                UpdateTotalProductsValue();
                break;
            case Card.Category.Employee:
                activeEmployees.Add(e.card);
                break;
            case Card.Category.Upgrade:
                activeUpgrades.Add(e.card);
                break;
        }
        
        EventBus<UpdateShopUIEvent>.Raise(new UpdateShopUIEvent(e.card));
    }

    private void RemoveCard(CardEvent e)
    {
        if (e.open) return;
        switch (e.card.category)
        {
            case Card.Category.Product:
                Product product = e.card as Product;
                activeProductsDict.Remove(product.productInfo.productType.ToString());
                UpdateTotalProductsValue();
                break;
            case Card.Category.Employee:
                activeEmployees.Remove(e.card);
                break;
            case Card.Category.Upgrade:
                activeUpgrades.Remove(e.card);
                break;
        }
        
        EventBus<UpdateShopUIEvent>.Raise(new UpdateShopUIEvent(e.card));
    }
    
    private void UpdateTotalProductsValue()
    {
        totalProductsValue = 0;
        foreach (var productType in activeProductsDict)
        {
            foreach (var product in productType.Value.products)
            {
                totalProductsValue += product.productInfo.productValue;
            }
        }
    }
    
    [System.Serializable]
    public class StartingProducts
    {
        public Product.ProductType productType;
        public int amount;
    }
    
    public class ProductHolder
    {
        public int size;
        public List<Card> products;
        
        public ProductHolder(int size, List<Card> products)
        {
            this.size = size;
            this.products = products;
        }
    }
}
