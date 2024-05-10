using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI moneyText;
    [SerializeField] TMPro.TextMeshProUGUI customersText;

    private void OnEnable()
    {
        ShopManager.OnMoneyChanged += OnMoneyChanged;
        ShopManager.OnCustomersChanged += OnCustomersChanged;
    }
    
    private void OnDisable()
    {
        ShopManager.OnMoneyChanged -= OnMoneyChanged;
        ShopManager.OnCustomersChanged -= OnCustomersChanged;
    }

    void OnMoneyChanged(int money)
    {
        moneyText.text = money.ToString();
    }
    
    void OnCustomersChanged(int customers)
    {
        customersText.text = customers.ToString();
    }
}
