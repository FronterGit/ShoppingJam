using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;
using UnityEngine.UI;

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
            Customer,
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
        public ProductInfo productInfo;

        public void OnClick()
        {
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                CardManager.instance.DiscardCard(this);
                return;
            }
            
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


