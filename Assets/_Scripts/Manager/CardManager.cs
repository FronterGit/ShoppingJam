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
    
    private List<Card> cardsToPick = new List<Card>();

    [SerializeField] private Transform cardPackOpeningParent;
    [SerializeField] private Transform cardHandParent;
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

    public void CardAction(Card card)
    {
        //Depending on the card category, different actions will be taken
        //IMPORTANT: Return out of the function if the card can't be activated.
        switch (card.category)
        {
            //If the card is a product card...
            case Card.Category.Product:
                string productType = card.productInfo.productType.ToString();
                
                //Check if we have it's type in the active products dictionary
                if (ShopManager.activeProductsDict.ContainsKey(productType))
                {
                    //Check if we have space for another product of this type
                    if (ShopManager.activeProductsDict[productType].size >=
                        ShopManager.activeProductsDict[productType].products.Count + 1)
                    {
                        Debug.Log("Product activated");
                        EventBus<ProductCardEvent>.Raise(new ProductCardEvent(card, true));
                    }
                    else
                    {
                        Debug.Log("Product can't be activated because the limit for this product type has been reached");
                        return;
                    }
                }
                else
                {
                    Debug.Log("Product can't be activated because the product type is not in the active products dictionary");
                    return;
                }
                break;
            
            //If the card is a customer card...
            case Card.Category.Customer:
                Debug.Log("Customer card activated");
                EventBus<CustomerCardEvent>.Raise(new CustomerCardEvent(card, true));
                break;
            case Card.Category.Employee:
                Debug.Log("Employee card activated");
                break;
            case Card.Category.Upgrade:
                Debug.Log("Upgrade card activated");
                break;
        }
        
        //If we reach this point, the card can be activated
        OnSuccessfulCardAction(card);
    }
    
    public void SetCardInHand(Card card)
    {
        //TODO: Hand size limit?
        OnSuccessfulCardAction(card);
        
        //Remove any spawned cards
        foreach (var pickingCards in cardsToPick)
        {
            Destroy(pickingCards.gameObject);
        }
        
        cardsToPick.Clear();
    }
    
    public void DiscardCard(Card card)
    {
        OnSuccessfulCardAction(card, true);
    }
    

    
    public void OnSuccessfulCardAction(Card card, bool discard = false)
    {
        if (discard)
        {
            hand.Remove(card);
            Destroy(card.gameObject);
            return;
        }
        
        
        //If the card was in our hand, remove it from the hand list and destroy the game object
        if (card.inHand)
        {
            hand.Remove(card);
            Destroy(card.gameObject);
        }
        //If the card was not in our hand, add it to the hand list and create a new game object to show the card is in our hand
        else
        {
            GameObject newCard = Instantiate(card.gameObject, new Vector3(hand.Count * cardSpacing, cardHeight, 0), Quaternion.identity, cardHandParent);
            Card newCardScript = newCard.GetComponent<Card>();
            newCardScript.inHand = true;
            newCard.transform.position = new Vector3(cardHandParent.position.x + hand.Count * cardSpacing, cardHandParent.transform.position.y, 0);
            hand.Add(newCardScript);
            
            Destroy(card.gameObject);
        }
    }
    
    private void OpenPack(CardPackEvent e)
    {
        if(!e.open) return;

        //get the middle of the canvas
        Vector3 middlePosition = new Vector3(canvas.pixelRect.width / 2, canvas.pixelRect.height / 2, 0);
        //get the width of a card
        float cardSpacing = 300;
        


        foreach (var card in e.cardPack.cards)
        {
            //place the first card to the left of the middle of the canvas, then place the rest of the cards to the right of the previous card
            
            middlePosition.x -= cardSpacing;
            SpawnCardsToPick(card, middlePosition);

         

        }
        EventBus<ChangeMoneyEvent>.Raise(new ChangeMoneyEvent(-e.cardPack.cardPackValue));
    }
    
    public void SpawnCardsToPick(Card card, Vector3 position)
    {
        GameObject newCard = Instantiate(card.gameObject, position, Quaternion.identity, cardPackOpeningParent);
        Card newCardScript = newCard.GetComponent<Card>();
        cardsToPick.Add(newCardScript);
    }
    
    
    
}
