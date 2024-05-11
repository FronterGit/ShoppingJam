using System.Collections;
using System.Collections.Generic;
using Cards;
using EventBus;
using UnityEngine;

public class CardPack : MonoBehaviour
{
    public int cardPackValue;
    public List<Card> cards;

    public void OnClick()
    {
        EventBus<CardPackEvent>.Raise(new CardPackEvent(this, true));
    }
}
