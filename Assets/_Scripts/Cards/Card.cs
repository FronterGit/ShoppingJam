using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public ProductBehaviour productBehaviour;
    public bool inHand;
    public enum Category
    {
        Product,
        Employee,
        Upgrade
    }
    public Category category;
    public CardDescription cardDescription;
    public GameObject cardPrefab;

    public void OnClick()
    {
        if (inHand)
        {
            CardManager.instance.SpawnCard(this);
        }
        else
        {
            CardManager.instance.SetCardInHand(this);
        }
    }

    private void Start()
    {
        transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
    }
}
