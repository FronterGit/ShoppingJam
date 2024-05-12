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
    public float storeTime = 5f;
    public CustomerBehaviour customerBehaviour;
    public Dictionary<string, ShopManager.ProductHolder> activeProductsDict;

    public enum CustomerType
    {
        Basic,
        Vegetarian
    }
    
    public CustomerType customerType;
    
    void Start()
    {
        activeProductsDict = ShopManager.activeProductsDict;
        
        
        
        
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
        if (canMove)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ShopHitbox"))
        {
            canMove = false;
            StartCoroutine(StoreTime());
            
            customerBehaviour.Buy();
        }
        else if (other.CompareTag("Death"))
        {
            Destroy(gameObject);
        }
    }
    

    
    private IEnumerator StoreTime()
    {
        yield return new WaitForSeconds(storeTime);
        canMove = true;
    }
}
