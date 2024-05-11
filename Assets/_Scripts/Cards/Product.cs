using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBus;

namespace Cards
{
    public class Product : Card, ProductBehaviour
    {
        public enum ProductType
        {
            Dairy,
            Meat,
            Vegetable
        }

        public ProductInfo productInfo;
        // Start is called before the first frame update
        void Start()
        {
            productBehaviour = this;
            EventBus<CardEvent>.Raise(new CardEvent(this, true));
        }
        
        private void OnDestroy()
        {
            EventBus<CardEvent>.Raise(new CardEvent(this, false));
        }
        
        public int GetProductValue()
        {
            return productInfo.productValue;
        }
    }
}
