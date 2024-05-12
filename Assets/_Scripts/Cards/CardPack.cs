using System.Collections;
using System.Collections.Generic;
using Cards;
using EventBus;
using UnityEngine;

public class CardPack : MonoBehaviour
{
    public int cardPackValue;
    public int cardCount;
    public List<Card> cards;
    public List<Card> gameCards = new List<Card>();
    

    public void OnClick()
    {
        if (CardManager.instance.GetHand().Count >= CardManager.instance.maxHandSize)
        {
            return;
        }
        
        if(ShopManager.GetMoneyFunc?.Invoke() < cardPackValue)
        {
            return;
        }
        
        EventBus<CardPackEvent>.Raise(new CardPackEvent(this, true));
    }
}
