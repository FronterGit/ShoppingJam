using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class CardEventTrigger : MonoBehaviour
{
    private Card card;

    private void Start()
    {
        card = GetComponent<Card>();
    }
    
    public void TriggerOnClick()
    {
        card.OnClick();
    }
}
