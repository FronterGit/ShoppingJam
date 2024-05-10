using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public GameObject customerPrefab;
    public float baseSpawnChance = 1f;
    public float spawnChance = 1f;

    [SerializeField] private GameObject shopObject;

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
    
    void SpawnCustomer()
    {
        Instantiate(customerPrefab, transform.position, Quaternion.identity, shopObject.transform);
        spawnChance = baseSpawnChance;
    }
}
