using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI moneyText;
    [SerializeField] TMPro.TextMeshProUGUI customersText;
    [SerializeField] private Camera mainCamera;
    
    [SerializeField] private GameObject shopInterface;
    private bool shopOpen = false;
    private bool shopAnimating = false;
    [SerializeField] private float shopSpeed = 1f;
    [SerializeField] private RectTransform shopOpenPos;
    [SerializeField] private RectTransform shopClosedPos;

    [SerializeField] private Transform cameraCenterPos;
    [SerializeField] private Transform cameraOffsetPos;

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

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void OnMoneyChanged(int money)
    {
        moneyText.text = money.ToString();
    }
    
    void OnCustomersChanged(int customers)
    {
        customersText.text = customers.ToString();
    }
    
    public void ToggleShop()
    {
        shopOpen = !shopOpen;
        shopAnimating = true;
    }

    private void Update()
    {
        if (shopAnimating)
        {
            if (shopOpen)
            {
                shopInterface.transform.position = Vector3.Lerp(shopInterface.transform.position, shopOpenPos.position, shopSpeed * Time.deltaTime);
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraOffsetPos.position, shopSpeed * Time.deltaTime);
                if (Vector3.Distance(shopInterface.transform.position, shopOpenPos.position) < 0.1f)
                {
                    shopAnimating = false;
                }
            }
            else
            {
                shopInterface.transform.position = Vector3.Lerp(shopInterface.transform.position, shopClosedPos.position, shopSpeed * Time.deltaTime);
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraCenterPos.position, shopSpeed * Time.deltaTime);
                if (Vector3.Distance(shopInterface.transform.position, shopClosedPos.position) < 0.1f)
                {
                    shopAnimating = false;
                }
            }
        }
    }
}
