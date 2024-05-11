using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBus;

namespace Cards
{
    public class Milk : Card
    {
        public override void CardAction()
        {
            Debug.Log("Milk card action");
        }
    }
}

