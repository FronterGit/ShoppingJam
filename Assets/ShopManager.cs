using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public int customers;
    public int totalProductsValue;
    public int money;
    public List<Customer> customersList = new List<Customer>();
    
    public static event System.Action<int> OnMoneyChanged;
    public static event System.Action<int> OnCustomersChanged;

    private void OnEnable()
    {
        Shop.OnCustomer += OnCustomer;
        TheMoneyHouse.ShopClicked += OnClick;
    }
    
    private void OnDisable()
    {
        Shop.OnCustomer -= OnCustomer;
        TheMoneyHouse.ShopClicked -= OnClick;
    }

    void Start()
    {
        OnMoneyChanged?.Invoke(money);
        OnCustomersChanged?.Invoke(customers);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnClick();
        }
    }

    public void OnClick()
    {
        money += customers * totalProductsValue;
        OnMoneyChanged?.Invoke(money);
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
}
