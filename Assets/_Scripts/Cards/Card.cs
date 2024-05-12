using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;
using UnityEngine.UI;

namespace Cards
{
    public abstract class Card : MonoBehaviour
    {
        public ProductBehaviour productBehaviour;
        public bool inHand;
        public enum Category
        {
            Product,
            Employee,
            Customer,
            Upgrade
        }
        public enum Rarity
        {
            Common,
            Rare,
            Legendary
        }

        public enum ProductType
        {
            Dairy,
            Meat,
            Vegetable
        }
        public string title;
        public string description;
        public Rarity rarity;
        public Category category;
        public int energyCost;
        public ProductInfo productInfo;
        public CustomerInfo customerInfo;

        public void OnClick()
        {
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                CardManager.instance.DiscardCard(this);
                return;
            }
            
            if (inHand)
            {
                CardManager.instance.CardAction(this);
            }
            else
            {
                CardManager.instance.SetCardInHand(this);
                EventBus<CardPackEvent>.Raise(new CardPackEvent(null, false));
            }
        }

        public abstract void CardAction();
    }
}


