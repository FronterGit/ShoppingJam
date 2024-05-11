using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;
using EventBus;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    public List<Card> hand = new List<Card>();
    [SerializeField] private float cardSpacing;
    [SerializeField] private float cardHeight;
    
    private List<Card> spawnedCards = new List<Card>();

    [SerializeField] private Transform cardPackOpeningParent;
    private Canvas canvas;

    private void OnEnable()
    {
        EventBus<CardPackEvent>.Subscribe(OpenPack);
    }
    
    private void OnDisable()
    {
        EventBus<CardPackEvent>.Unsubscribe(OpenPack);
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();
    }

    public void ActivateCard(Card card)
    {
        //TODO: Check if card is allowed to be played
        Instantiate(card.cardPrefab, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform);
        UpdateHand(card);
    }
    
    public void SetCardInHand(Card card)
    {
        UpdateHand(card);
        
        foreach (var spawnedCard in spawnedCards)
        {
            Destroy(spawnedCard.gameObject);
        }
        
        spawnedCards.Clear();
        
    }
    
    public void SpawnCard(Card card)
    {
        GameObject newCard = Instantiate(card.gameObject, new Vector3(0, 0, 0), Quaternion.identity, cardPackOpeningParent);
        Card newCardScript = newCard.GetComponent<Card>();
        spawnedCards.Add(newCardScript);
    }
    
    public void UpdateHand(Card card)
    {
        //If the card was in our hand, remove it from the hand list and destroy the game object
        if (card.inHand)
        {
            hand.Remove(card);
            Destroy(card.gameObject);
        }
        //If the card was not in our hand, add it to the hand list and create a new game object to show the card is in our hand
        else
        {
            GameObject newCard = Instantiate(card.gameObject, new Vector3(hand.Count * cardSpacing, cardHeight, 0), Quaternion.identity, canvas.transform);
            Card newCardScript = newCard.GetComponent<Card>();
            newCardScript.inHand = true;
            hand.Add(newCardScript);
            
            Destroy(card.gameObject);
        }
    }
    
    private void OpenPack(CardPackEvent e)
    {
        if(!e.open) return;
        foreach (var card in e.cardPack.cards)
        {
            SpawnCard(card);
        }
        EventBus<ChangeMoneyEvent>.Raise(new ChangeMoneyEvent(-e.cardPack.cardPackValue));
    }
    
    
    
}
