using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;

namespace Cards
{
    public class Card : MonoBehaviour
    {
        public ProductBehaviour productBehaviour;
        public bool inHand;
        public enum Category
        {
            Product,
            Employee,
            Upgrade
        }
        public enum Rarity
        {
            Common,
            Rare,
            Epic,
            Legendary
        }
        public Rarity rarity;
        public Category category;
        public CardDescription cardDescription;
        public GameObject cardPrefab;

        public void OnClick()
        {
            if (inHand)
            {
                CardManager.instance.ActivateCard(this);
            }
            else
            {
                CardManager.instance.SetCardInHand(this);
                EventBus<CardPackEvent>.Raise(new CardPackEvent(null, false));
            }
        }
    }
}


