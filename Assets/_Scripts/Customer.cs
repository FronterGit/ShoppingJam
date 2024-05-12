using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;
using EventBus;

public class Customer : MonoBehaviour
{
    public int speed;
    public bool canMove = true;
    public bool reachedGoal = false;
    public float storeTime = 5f;
    public CustomerBehaviour customerBehaviour;
    public Dictionary<string, ShopManager.ProductHolder> activeProductsDict;
    private SpriteRenderer spriteRenderer;
    public Sprite backSprite;
    
    private Vector3 spawnPoint;
    public Vector3 goalPoint;

    public enum CustomerType
    {
        Basic,
        Vegetarian
    }
    
    public CustomerType customerType;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spawnPoint = transform.position;
    }

    private void OnEnterShop()
    {
        //Get the active products from the shop manager
        activeProductsDict = ShopManager.activeProductsDict;
        
        int goldToAdd = 0;
        //Loop through all active products and let them modify the active products dictionary
        foreach (ShopManager.ProductHolder product in activeProductsDict.Values)
        {
            foreach (Card card in product.products)
            {
                goldToAdd += card.AddedGoldFromProducts(activeProductsDict, this);
            }
        }

        //Get the active upgrades from the shop manager
        List<Card> activeUpgrades = ShopManager.GetActiveUpgradesFunc?.Invoke();

        //Loop over all active upgrades and let them modify the active products dictionary
        foreach (UpgradeCardBeviour upgradeCard in activeUpgrades)
        {
            Dictionary<string, ShopManager.ProductHolder> newActiveProductsDict =
                upgradeCard.GetNewActiveProductsDict(activeProductsDict);
            activeProductsDict = newActiveProductsDict;
        }

        switch (customerType)
        {
            case CustomerType.Basic:
                customerBehaviour = new BasicCustomerStrategy(activeProductsDict);
                break;
            case CustomerType.Vegetarian:
                customerBehaviour = new VegetarianCustomerStrategy(activeProductsDict);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !reachedGoal)
        {
            transform.position = Vector3.MoveTowards(transform.position, goalPoint, speed * Time.deltaTime);
        }
        else if(canMove && reachedGoal)
        {
            transform.position = Vector3.MoveTowards(transform.position, spawnPoint, speed * Time.deltaTime);
        }
        
        if(reachedGoal && transform.position == spawnPoint)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ShopHitbox"))
        {
            canMove = false;
            StartCoroutine(StoreTime());
            
            OnEnterShop();
            customerBehaviour.Buy();
            
            reachedGoal = true;
            spriteRenderer.sprite = null;
        }
    }
    

    
    private IEnumerator StoreTime()
    {
        yield return new WaitForSeconds(storeTime);
        canMove = true;
        spriteRenderer.sprite = backSprite;
    }

    private void OnDestroy()
    {
        EventBus<RemoveCustomerEvent>.Raise(new RemoveCustomerEvent(this.gameObject));
    }
}
