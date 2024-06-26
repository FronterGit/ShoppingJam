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
            Common, // 75%
            Rare, // 20%
            Legendary // 5%
        }

        public enum ProductType
        {
            General,
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
                if(!inHand) return;
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

        public abstract int AddedGoldFromProducts(Dictionary<string, ShopManager.ProductHolder> activeProductsDict, Customer customer);
    }
}


