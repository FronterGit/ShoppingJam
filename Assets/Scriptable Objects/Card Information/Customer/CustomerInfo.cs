using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerInfo", menuName = "Card Information/new Customer Info", order = 1)]
public class CustomerInfo : ScriptableObject
{
    public string customerName;
    public int timesToSpawn;
    public int turnsToSpawn;
    public GameObject customerPrefab;
}
