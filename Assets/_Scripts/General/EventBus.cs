using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;
using Event = EventBus.Event;

namespace EventBus {
    public abstract class EventBus<T> where T : Event {
        private static event System.Action<T> onEventRaised;

        public static void Subscribe(System.Action<T> action) {
            onEventRaised += action;
        }

        public static void Unsubscribe(System.Action<T> action) {
            onEventRaised -= action;
        }

        public static void Raise(T eventToRaise) {
            onEventRaised?.Invoke(eventToRaise);
        }
    }

    public abstract class Event {
    }

    public class ProductCardEvent : Event {
        public Card card;
        public bool open;

        public ProductCardEvent(Card card, bool open) {
            this.card = card;
            this.open = open;
        }
    }

    public class CustomerCardEvent : Event {
        public Card card;
        public bool open;

        public CustomerCardEvent(Card card, bool open) {
            this.card = card;
            this.open = open;
        }
    }
    
    public class UpgradeCardEvent : Event {
        public Card card;
        public bool open;

        public UpgradeCardEvent(Card card, bool open) {
            this.card = card;
            this.open = open;
        }
    }

    public class UpdateShopUIEvent : Event {
        public Card card;

        public UpdateShopUIEvent(Card card) {
            this.card = card;
        }
    }

    public class CardPackEvent : Event {
        public CardPack cardPack;
        public bool open;

        public CardPackEvent(CardPack cardPack, bool open) {
            this.cardPack = cardPack;
            this.open = open;
        }
    }

    public class ChangeMoneyEvent : Event {
        public int money;
        public readonly bool isRevenue; // boolean to determine whether it is revenue made from a customer

        public ChangeMoneyEvent(int money, bool isRevenue) {
            this.money = money;
            this.isRevenue = isRevenue;
        }
    }

    public class MoneyChangedEvent : Event {
        public int money;

        public MoneyChangedEvent(int money) {
            this.money = money;
        }
    }

    public class ChangeEnergyEvent : Event {
        public int energy;

        public ChangeEnergyEvent(int energy) {
            this.energy = energy;
        }
    }

    public class EnergyChangedEvent : Event {
        public int energy;
        public int maxEnergy;

        public EnergyChangedEvent(int energy, int maxEnergy) {
            this.energy = energy;
            this.maxEnergy = maxEnergy;
        }
    }

    public class ChangeMaxEnergyEvent : Event {
        public int energy;

        public ChangeMaxEnergyEvent(int energy) {
            this.energy = energy;
        }
    }

    public class StartTurnEvent : Event {
        public Turn turn;

        public StartTurnEvent(Turn turn) {
            this.turn = turn;
        }
    }

    public class EndTurnEvent : Event {
        public EndTurnEvent() {
        }
    }

    public class RequestNewCustomersListEvent : Event {
        public RequestNewCustomersListEvent() {
        }
    }

    public class UpdateCustomerPreviewEvent : Event {
        public UpdateCustomerPreviewEvent() {
        }
    }

    /// <summary>
    /// 0 for spawn, 1 for despawn
    /// </summary>
    public class RegionalManagerEvent : Event {
        public readonly int action; // 0 for spawn, 1 for despawn;
        public readonly int expectedRevenue;

        public RegionalManagerEvent(int action, int expectedRevenue) {
            this.action = action;
            this.expectedRevenue = expectedRevenue;
        }
    }

    public class CardFinishedLerpingEvent : Event
    {
        public CardFinishedLerpingEvent()
        {
        }
        
    }
}