using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;
using Event = EventBus.Event;

namespace EventBus
{
    public abstract class EventBus<T> where T : Event
    {
        private static event System.Action<T> onEventRaised;

        public static void Subscribe(System.Action<T> action)
        {
            onEventRaised += action;
        }

        public static void Unsubscribe(System.Action<T> action)
        {
            onEventRaised -= action;
        }

        public static void Raise(T eventToRaise)
        {
            onEventRaised?.Invoke(eventToRaise);
        }
    }
    
    public abstract class Event
    {
    }
    
    public class ProductCardEvent : Event
    {
        public Card card;
        public bool open;
        public ProductCardEvent(Card card, bool open)
        {
            this.card = card;
            this.open = open;
        }
    }
    
    public class CustomerCardEvent : Event
    {
        public Card card;
        public bool open;
        public CustomerCardEvent(Card card, bool open)
        {
            this.card = card;
            this.open = open;
        }
    }
    
    public class UpdateShopUIEvent : Event
    {
        public Card card;
        public UpdateShopUIEvent(Card card)
        {
            this.card = card;
        }
    }
    
    public class CardPackEvent : Event
    {
        public CardPack cardPack;
        public bool open;
        
        public CardPackEvent(CardPack cardPack, bool open)
        {
            this.cardPack = cardPack;
            this.open = open;
        }
    }
    
    public class ChangeMoneyEvent : Event
    {
        public int money;
        public ChangeMoneyEvent(int money)
        {
            this.money = money;
        }
    }
    
    public class MoneyChangedEvent : Event
    {
        public int money;
        public MoneyChangedEvent(int money)
        {
            this.money = money;
        }
    }
    
    public class StartTurnEvent : Event
    {
        public Turn turn;
        public StartTurnEvent(Turn turn)
        {
            this.turn = turn;
        }
    }
    
    public class EndTurnEvent : Event
    {
        public EndTurnEvent()
        {
        }
    }
    
    public class RequestNewCustomersListEvent : Event
    {
        public RequestNewCustomersListEvent()
        {
        }
    }
    
    public class UpdateCustomerPreviewEvent : Event
    {
        public UpdateCustomerPreviewEvent()
        {
        }
    }
}