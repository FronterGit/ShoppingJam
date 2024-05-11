using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Top bar")]
    [SerializeField] TMPro.TextMeshProUGUI moneyText;
    [SerializeField] TMPro.TextMeshProUGUI customersText;
    
    [Header("Shop Interface")]
    [SerializeField] private GameObject shopInterface;
    private bool shopOpen = false;
    private bool shopAnimating = false;
    [SerializeField] private float shopSpeed = 1f;
    [SerializeField] private RectTransform shopOpenPos;
    [SerializeField] private RectTransform shopClosedPos;
    
    [Header("Card Pack opening")]
    [SerializeField] private Image cardPackOpeningBackground;
    [SerializeField] private GameObject cardPackOpeningParent;

    //TODO: Camera should be responsible for its own position
    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform cameraCenterPos;
    [SerializeField] private Transform cameraOffsetPos;

    private void OnEnable()
    {
        EventBus<MoneyChangedEvent>.Subscribe(OnMoneyChanged);
        EventBus<CardPackEvent>.Subscribe(OnCardPackOpen);
    }
    
    private void OnDisable()
    {
        EventBus<MoneyChangedEvent>.Unsubscribe(OnMoneyChanged);
        EventBus<CardPackEvent>.Unsubscribe(OnCardPackOpen);
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void OnMoneyChanged(MoneyChangedEvent e)
    {
        moneyText.text = e.money.ToString();
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
    
    public void OnCardPackOpen(CardPackEvent e)
    {
        if (e.open)
        {
            cardPackOpeningBackground.gameObject.SetActive(true);
        }
        else
        {
            cardPackOpeningBackground.gameObject.SetActive(false);
        }
    }
}
