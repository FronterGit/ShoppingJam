using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public GameObject basicCustomerPrefab;

    [SerializeField] private GameObject shopObject;
    private List<AdditionalCustomer> additionalCustomers = new List<AdditionalCustomer>();

    private void OnEnable()
    {
        EventBus<StartTurnEvent>.Subscribe(OnStartTurn);
        EventBus<CustomerCardEvent>.Subscribe(OnCustomerCardActivated);
    }
    
    private void OnDisable()
    {
        EventBus<StartTurnEvent>.Unsubscribe(OnStartTurn);
        EventBus<CustomerCardEvent>.Unsubscribe(OnCustomerCardActivated);
    }

    public void OnStartTurn(StartTurnEvent e)
    {
        //Start the customer spawn coroutine
        StartCoroutine(SpawnCustomer(e.turn.playTime, e.turn.basicCustomerCount));
        
        Debug.Log("Start turn");
    }
    
    private IEnumerator SpawnCustomer(float turnPlayTime, int customers)
    {
        //Declare a list of customers to spawn
        List<GameObject> customersToSpawn = new List<GameObject>();
        
        //Add the basic customer to the list
        for(int i = 0; i < customers; i++) customersToSpawn.Add(basicCustomerPrefab);

        
        //Add the additional customers to the list
        //For each additional customer...
        foreach (var additionalCustomer in additionalCustomers)
        {
            //For each time the customer should spawn...
            for (int i = 0; i < additionalCustomer.timesToSpawn; i++)
            {
                //Add the customer to the list
                customersToSpawn.Add(additionalCustomer.customer.gameObject);
            }
            
            //Decrement the turn to spawn
            additionalCustomer.turnToSpawn--;
            
            //If the turn to spawn is 0, remove the customer from the list
            if (additionalCustomer.turnToSpawn <= 0)
            {
                additionalCustomers.Remove(additionalCustomer);
            }
        }
        
        //Calculate the time between customers
        int customerSpawnCount = customers + additionalCustomers.Count;
        float timeBetweenCustomers = turnPlayTime / customerSpawnCount;
        
        //Spawn the customers
        for (int i = 0; i < customerSpawnCount; i++)
        {
            //Set a random customer to spawn
            int random = UnityEngine.Random.Range(0, customersToSpawn.Count);
            GameObject customer = customersToSpawn[random];
            
            //Spawn the customer and remove it from the list
            SpawnCustomer(customer);
            customersToSpawn.Remove(customer);
            
            //Wait for the time between customers
            yield return new WaitForSeconds(timeBetweenCustomers);
        }
        
        //End the turn
        EventBus<EndTurnEvent>.Raise(new EndTurnEvent());
    }
    
    void SpawnCustomer(GameObject customer)
    {
        //Spawn the customer
        Instantiate(customer, transform.position, Quaternion.identity, shopObject.transform);
    }
    
    public void AddAdditionalCustomer(Customer customer, int timesToSpawn, int turnToSpawn)
    {
        //Create a new additional customer
        AdditionalCustomer additionalCustomer = new AdditionalCustomer(customer, timesToSpawn, turnToSpawn);
        
        //Add the additional customer to the list
        additionalCustomers.Add(additionalCustomer);
    }
    
    public void OnCustomerCardActivated(CustomerCardEvent e)
    {
        Debug.Log("Customer card activated");
    }

    public class AdditionalCustomer
    {
        public Customer customer;
        public int timesToSpawn;
        public int turnToSpawn;
        
        public AdditionalCustomer(Customer customer, int timesToSpawn, int turnToSpawn)
        {
            this.customer = customer;
            this.timesToSpawn = timesToSpawn;
            this.turnToSpawn = turnToSpawn;
        }
    }
}
