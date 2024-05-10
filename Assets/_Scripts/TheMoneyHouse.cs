using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TheMoneyHouse : MonoBehaviour
{
    public static event System.Action ShopClicked; 
    private SpriteRenderer shopSprite;
    private Vector2 mousePos;

    private void Start()
    {
        shopSprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //if the mousePos is over the shopSprite
            if (shopSprite.bounds.Contains(mousePos))
            {
                ShopClicked?.Invoke();
            }
        }
    }
}
