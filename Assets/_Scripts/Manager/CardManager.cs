using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;
using EventBus;
using Random = System.Random;

public class CardManager : MonoBehaviour {
    public static CardManager instance;
    public List<Card> hand = new List<Card>();
    [SerializeField] private float cardSpacing;
    [SerializeField] private float cardHeight;
    public int maxHandSize = 5;

    private List<Card> cardsToPick = new List<Card>();

    [SerializeField] private Transform cardPackOpeningParent;
    [SerializeField] private Transform cardHandParent;
    private Canvas canvas;


    [SerializeField] private float cardPackAnimationMoveTime = 0.1f;
    private GameObject cardPack;
    private List<Card> cardsToLerp = new List<Card>();
    public List<Vector3> toPositions = new List<Vector3>();

    public static Func<List<Card>> GetHandFunc;

    public float commonChance = 0.75f;
    public float rareChance = 0.2f;
    public float legendaryChance = 0.05f;

    private void OnEnable() {
        EventBus<CardPackEvent>.Subscribe(OpenPack);
        EventBus<CardFinishedLerpingEvent>.Subscribe(LerpCard);

        GetHandFunc += GetHand;
    }

    private void OnDisable() {
        EventBus<CardPackEvent>.Unsubscribe(OpenPack);
        EventBus<CardFinishedLerpingEvent>.Unsubscribe(LerpCard);

        GetHandFunc -= GetHand;
    }

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(this);
        }
    }

    private void Start() {
        canvas = FindObjectOfType<Canvas>();
    }

    public void CardAction(Card card) {
        //IMPORTANT: Return out of the function if the card can't be activated.
        if (ShopManager.GetEnergyFunc?.Invoke() < card.energyCost)
        {
            EventBus<ErrorPromptEvent>.Raise(new ErrorPromptEvent(ErrorPromptEvent.ErrorType.NotEnoughEnergy));
            return;
        }

        //Depending on the card category, different actions will be taken
        switch (card.category) {
            //If the card is a product card...
            case Card.Category.Product:
                string productType = card.productInfo.productType.ToString();

                
                Dictionary<string, ShopManager.ProductHolder> activeProductsDict = ShopManager.GetActiveProductsDictFunc?.Invoke();
                
                //Check if we have it's type in the active products dictionary
                if (activeProductsDict.ContainsKey(productType)) {
                    //Check if we have space for another product of this type
                    if (activeProductsDict[productType].size >=
                        activeProductsDict[productType].products.Count + 1) {
                        Debug.Log("Product activated");
                        EventBus<ProductCardEvent>.Raise(new ProductCardEvent(card, true));

                        if (AudioManager.instance != null) AudioManager.instance.PlaySound("card_interact_2");
                    }
                    else {
                        Debug.Log(
                            "Product can't be activated because the limit for this product type has been reached");
                        EventBus<ErrorPromptEvent>.Raise(new ErrorPromptEvent(ErrorPromptEvent.ErrorType.NotEnoughSpace));
                        return;
                    }
                }
                else {
                    Debug.Log(
                        "Product can't be activated because the product type is not in the active products dictionary");
                    EventBus<ErrorPromptEvent>.Raise(new ErrorPromptEvent(ErrorPromptEvent.ErrorType.NotEnoughSpace));
                    return;
                }

                break;

            //If the card is a customer card...
            case Card.Category.Customer:
                Debug.Log("Customer card activated");
                EventBus<CustomerCardEvent>.Raise(new CustomerCardEvent(card, true));
                if (AudioManager.instance != null) AudioManager.instance.PlaySound("card_interact_2");
                break;
            case Card.Category.Employee:
                Debug.Log("Employee card activated");
                break;
            case Card.Category.Upgrade:
                if (AudioManager.instance != null) AudioManager.instance.PlaySound("card_interact_2");
                Debug.Log("Upgrade card activated");
                EventBus<UpgradeCardEvent>.Raise(new UpgradeCardEvent(card, true));
                break;
        }

        //If we reach this point, the card can be activated
        OnSuccessfulCardAction(card);
    }

    public void SetCardInHand(Card card) {
        OnSuccessfulCardAction(card);

        //Remove any spawned cards
        foreach (var pickingCards in cardsToPick) {
            Destroy(pickingCards.gameObject);
        }

        if (AudioManager.instance != null) AudioManager.instance.PlaySound("card_interact_1");

        cardsToPick.Clear();
        RespawnHand();
    }

    public void DiscardCard(Card card) {
        OnSuccessfulCardAction(card, true);
    }


    public void OnSuccessfulCardAction(Card card, bool discard = false) {
        if (discard) {
            hand.Remove(card);
            Destroy(card.gameObject);
            RespawnHand();
            return;
        }


        //If the card was in our hand...
        if (card.inHand) {
            //Remove the card from the hand list
            hand.Remove(card);

            //Raise an event to change the energy
            EventBus<ChangeEnergyEvent>.Raise(new ChangeEnergyEvent(-card.energyCost));

            //Destroy the card game object
            Destroy(card.gameObject);
            
            RespawnHand();
        }
        //If the card was not in our hand, add it to the hand list and create a new game object to show the card is in our hand
        else {
            GameObject newCard = Instantiate(card.gameObject, new Vector3(hand.Count * cardSpacing, cardHeight, 0),
                Quaternion.identity, cardHandParent);
            Card newCardScript = newCard.GetComponent<Card>();
            newCardScript.inHand = true;
            newCard.transform.position = new Vector3(cardHandParent.position.x + hand.Count * cardSpacing,
                cardHandParent.transform.position.y, 0);
            hand.Add(newCardScript);

            Destroy(card.gameObject);
        }
    }
    
    public void RespawnHand() {
        //Make a copy of the hand
        List<Card> handCopy = hand;
        List<Card> newHand = new List<Card>();
        


        foreach (var card in handCopy){
            int cardPosition = handCopy.IndexOf(card);
            float offset = canvas.pixelRect.width / 2;
            GameObject newCard = Instantiate(card.gameObject, new Vector3((cardPosition * cardSpacing) + offset, cardHeight, 0),
                Quaternion.identity, cardHandParent);
            Card newCardScript = newCard.GetComponent<Card>();
            newCardScript.inHand = true;
            newHand.Add(newCardScript);
        }
        
        foreach (var card in hand) {
            Destroy(card.gameObject);
        }
        hand.Clear();
        hand = newHand;
        

    }

    private void OpenPack(CardPackEvent e) {
        if (!e.open) return;

        //get the number of cards in the pack
        // int cardCount = e.cardPack.cards.Count;
        int cardCount = e.cardPack.cardCount;
        //get the middle of the canvas

        // Vector3 startPosition = new Vector3(canvas.pixelRect.width / 2, canvas.pixelRect.height / 2, 0);
        float cardSpacing = 300;

        // calculate card spacing
        // int cardsOnScreen = e.cardPack.cardCount;
        // float cardWidth = e.cardPack.cards[0].gameObject.GetComponent<RectTransform>().rect.width;
        // float totalCardSizes = cardsOnScreen * cardWidth;
        // float screenSize = canvas.pixelRect.width;
        // float remainingSpace = screenSize - totalCardSizes;
        // cardSpacing = remainingSpace / (cardsOnScreen + 1); 
        //
        // if(cardSpacing < 0) {
        //     cardSpacing = 0;
        // }

        Debug.Log("Card spacing: " + cardSpacing);

        // startPosition = new Vector3(startPosition.x - (cardCount * cardSpacing), startPosition.y, startPosition.z);
        // float cardRowWidth = startPosition.x + (cardCount * cardSpacing);
        //get the width of a card


/*
        foreach (var card in e.cardPack.cards)
        {

            //place all cards along the cardRowWidth in equal spacing
            startPosition = new Vector3(startPosition.x + cardSpacing * 1.5f, startPosition.y, startPosition.z);

            //spawn a new card offscreen and lerp it to the position
            SpawnCardsToPick(card, startPosition);
        }
 */

        CardPack cardPack = e.cardPack;

        List<Card> common = new();
        List<Card> rare = new();
        List<Card> legendary = new();

        foreach (var card in cardPack.cards) {
            // startPosition = new Vector3(startPosition.x + cardSpacing * 1.5f, startPosition.y, startPosition.z);

            switch (card.rarity) {
                case Card.Rarity.Common:
                    common.Add(card);
                    break;
                case Card.Rarity.Rare:
                    rare.Add(card);
                    break;
                case Card.Rarity.Legendary:
                    legendary.Add(card);
                    break;
            }
        }

        float offset = cardPack.cards[0].GetComponent<RectTransform>().rect.width;

        for (int i = 0; i < cardPack.cardCount; i++) {
            float random = UnityEngine.Random.Range(0f, 1f);

            Vector3 startPosition = new Vector3(300 + (offset / 2f) + 10, canvas.pixelRect.height / 2, 0);
            startPosition = new Vector3(startPosition.x + cardSpacing * i, startPosition.y, startPosition.z);

            Card card;

            if (random < commonChance) {
                //If we have no common cards, reroll
                if (common.Count == 0) {
                    i--;
                    continue;
                }
                
                // cardPack.cards[i].rarity = Card.Rarity.Common;
                card = common[UnityEngine.Random.Range(0, common.Count)];
                cardPack.gameCards.Add(card);

                Debug.Log("Common card added to gameCards");
            }
            else if (random > commonChance && random < commonChance + rareChance) {
                //If we have no rare cards, reroll
                if (rare.Count == 0) {
                    i--;
                    continue;
                }
                
                // cardPack.cards[i].rarity = Card.Rarity.Rare;
                card = rare[UnityEngine.Random.Range(0, rare.Count)];
                cardPack.gameCards.Add(card);

                Debug.Log("Rare card added to gameCards");
            }
            else {
                //If we have no legendary cards, reroll
                if (legendary.Count == 0) {
                    i--;
                    continue;
                }
                
                // cardPack.cards[i].rarity = Card.Rarity.Legendary;
                card = legendary[UnityEngine.Random.Range(0, legendary.Count)];
                cardPack.gameCards.Add(card);

                Debug.Log("Legendary card added to gameCards");
            }


            SpawnCardsToPick(card, startPosition);
        }

        if (AudioManager.instance != null) AudioManager.instance.PlaySound("shuffle_sound_1");
        EventBus<CardFinishedLerpingEvent>.Raise(new CardFinishedLerpingEvent());
        EventBus<ChangeMoneyEvent>.Raise(new ChangeMoneyEvent(-e.cardPack.cardPackValue, false));
        
    }

    public void SpawnCardsToPick(Card card, Vector3 toPosition) {
        cardPack = GameObject.Find("Product Card Pack");
        Vector3 cardPackPosition = cardPack.transform.position;
        GameObject newCard = Instantiate(card.gameObject, cardPackPosition, Quaternion.identity, cardPackOpeningParent);

        //throw the instantiated card in cardsToLerp
        cardsToLerp.Add(newCard.GetComponent<Card>());
        toPositions.Add(toPosition);


        Card newCardScript = newCard.GetComponent<Card>();
        cardsToPick.Add(newCardScript);
    }


    public void LerpCard(CardFinishedLerpingEvent e) {
        if (cardsToLerp.Count == 0) return;
        //lerp the card to the position
        StartCoroutine(LerpCard(cardsToLerp[0].gameObject, toPositions[0]));
        cardsToLerp.RemoveAt(0);
        toPositions.RemoveAt(0);
    }

    private IEnumerator LerpCard(GameObject card, Vector3 toPosition) {
        float elapsedTime = 0f;
        Vector3 startPosition = card.transform.position;
        while (elapsedTime < cardPackAnimationMoveTime) {
            elapsedTime += Time.deltaTime;
            card.transform.position = Vector3.Lerp(startPosition, toPosition, elapsedTime / cardPackAnimationMoveTime);
            yield return null;
        }

        EventBus<CardFinishedLerpingEvent>.Raise(new CardFinishedLerpingEvent());
    }

    public List<Card> GetHand() {
        return hand;
    }
}