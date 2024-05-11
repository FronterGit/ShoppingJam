using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public GameObject basicCustomerPrefab;
    public float baseSpawnChance = 1f;
    public float spawnChance = 1f;

    [SerializeField] private GameObject shopObject;
    private List<AdditionalCustomer> additionalCustomers = new List<AdditionalCustomer>();

    private void Update()
    {
        float random = UnityEngine.Random.Range(0f, spawnChance);
        if (random <= 0.1f)
        {
            SpawnCustomer();
        }
        else
        {
            spawnChance -= 0.1f;
        }
    }
    
    private IEnumerator SpawnCustomer(float turnPlayTime, int customers)
    {
        //Declare a list of customers to spawn
        List<GameObject> customersToSpawn = new List<GameObject>();
        
        //Add the basic customer to the list
        for(int i = 0; i < customers; i++)
        {
            customersToSpawn.Add(basicCustomerPrefab);
        }
        
        //Add the additional customers to the list
        //For each additional customer
        foreach (var additionalCustomer in additionalCustomers)
        {
            //For each time the customer should spawn
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
            yield return new WaitForSeconds(timeBetweenCustomers);
            SpawnCustomer();
        }
    }
    
    void SpawnCustomer()
    {
        Instantiate(basicCustomerPrefab, transform.position, Quaternion.identity, shopObject.transform);
        spawnChance = baseSpawnChance;
    }
    
    public void AddAdditionalCustomer(Customer customer, int timesToSpawn, int turnToSpawn)
    {
        AdditionalCustomer additionalCustomer = new AdditionalCustomer(customer, timesToSpawn, turnToSpawn);
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
