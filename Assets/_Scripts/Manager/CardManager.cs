using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    public List<Card> hand = new List<Card>();
    [SerializeField] private float cardSpacing;
    [SerializeField] private float cardHeight;
    
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
    
    public void SpawnCard(Card card)
    {
        Instantiate(card.cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        UpdateHand(card);
    }
    
    public void SetCardInHand(Card card)
    {
        UpdateHand(card);
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
            GameObject newCard = Instantiate(card.gameObject, new Vector3(hand.Count * cardSpacing, cardHeight, 0), Quaternion.identity);
            Card newCardScript = newCard.GetComponent<Card>();
            newCardScript.inHand = true;
            hand.Add(newCardScript);
            
            Destroy(card.gameObject);
        }
    }
    
    
    
}
