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
    public int energy;
    public int maxEnergy;
    public int revenueBeforeManagerVisit;
    public List<Customer> customersList = new List<Customer>();
    
    public List<StartingProducts> startingProducts;
    public static Dictionary<string, ProductHolder> activeProductsDict;
    public static List<Card> activeEmployees = new List<Card>();
    public static List<Card> activeUpgrades = new List<Card>();

    public static Func<int> GetMoneyFunc;
    public static Func<int> GetEnergyFunc;
    public static Func<int> GetMaxEnergyFunc;
    public static Func<int> GetRevenueFunc;

    private void OnEnable()
    {
        Shop.OnCustomer += OnCustomer;
        
        GetMoneyFunc += GetMoney;
        GetEnergyFunc += GetEnergy;
        GetMaxEnergyFunc += GetMaxEnergy;
        GetRevenueFunc += GetRevenue;
        
        EventBus<ProductCardEvent>.Subscribe(ReceiveCard);
        EventBus<ProductCardEvent>.Subscribe(RemoveCard);
        EventBus<ChangeMoneyEvent>.Subscribe(OnChangeMoney);
        EventBus<ChangeEnergyEvent>.Subscribe(OnChangeEnergy);
    }
    
    private void OnDisable()
    {
        Shop.OnCustomer -= OnCustomer;
        
        GetMoneyFunc -= GetMoney;
        GetEnergyFunc -= GetEnergy;
        GetMaxEnergyFunc -= GetMaxEnergy;
        GetRevenueFunc -= GetRevenue;

        EventBus<ProductCardEvent>.Unsubscribe(ReceiveCard);
        EventBus<ProductCardEvent>.Unsubscribe(RemoveCard);
        EventBus<ChangeMoneyEvent>.Unsubscribe(OnChangeMoney);
        EventBus<ChangeEnergyEvent>.Unsubscribe(OnChangeEnergy);
    }

    void Start()
    {
        EventBus<MoneyChangedEvent>.Raise(new MoneyChangedEvent(money));
        energy = maxEnergy;
        EventBus<EnergyChangedEvent>.Raise(new EnergyChangedEvent(energy, maxEnergy));

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
    }
    
    private void OnChangeMoney(ChangeMoneyEvent e)
    {
        money += e.money;

        // if the money is income, add it to the revenue counter
        if (e.isRevenue) {
            revenueBeforeManagerVisit += e.money;
        }
        
        EventBus<MoneyChangedEvent>.Raise(new MoneyChangedEvent(money));
    }
    
    private void OnChangeEnergy(ChangeEnergyEvent e)
    {
        energy += e.energy;
        EventBus<EnergyChangedEvent>.Raise(new EnergyChangedEvent(energy, maxEnergy));
    }
    
    private void OnChangeMaxEnergy(ChangeMaxEnergyEvent e)
    {
        maxEnergy += e.energy;
        EventBus<EnergyChangedEvent>.Raise(new EnergyChangedEvent(maxEnergy, maxEnergy));
    }
    
    private void ReceiveCard(ProductCardEvent e)
    {
        if (!e.open) return;
        switch (e.card.category)
        {
            case Card.Category.Product:
                activeProductsDict[e.card.productInfo.productType.ToString()].products.Add(e.card);
                Debug.Log("Product added, size of list: " +
                          activeProductsDict[e.card.productInfo.productType.ToString()].size + " product count: " + activeProductsDict[e.card.productInfo.productType.ToString()].products.Count);
                UpdateTotalProductsValue();
                break;
            case Card.Category.Employee:
                activeEmployees.Add(e.card);
                break;
            case Card.Category.Upgrade:
                activeUpgrades.Add(e.card);
                break;
            case Card.Category.Customer:
                //Do nothing, this is for the customerManager.
                break;
        }
        
        EventBus<UpdateShopUIEvent>.Raise(new UpdateShopUIEvent(e.card));
    }

    private void RemoveCard(ProductCardEvent e)
    {
        if (e.open) return;
        switch (e.card.category)
        {
            case Card.Category.Product:
                activeProductsDict.Remove(e.card.productInfo.productType.ToString());
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
    
    public int GetMoney()
    {
        return money;
    }
    
    public int GetEnergy()
    {
        return energy;
    }
    
    public int GetMaxEnergy()
    {
        return maxEnergy;
    }

    public int GetRevenue() => revenueBeforeManagerVisit;
    
    [System.Serializable]
    public class StartingProducts
    {
        public Card.ProductType productType;
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
