using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBus;

namespace Cards
{
    public class Milk : Card
    {
        public override int AddedGoldFromProducts(Dictionary<string, ShopManager.ProductHolder> activeProductsDict, Customer customer)
        {
            //No effect.
            return 0;
        }
    }
}

