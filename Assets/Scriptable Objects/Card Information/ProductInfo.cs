using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

[CreateAssetMenu(fileName = "ProductInfo", menuName = "Scriptable Objects/New Product Info", order = 1)]
public class ProductInfo : ScriptableObject
{
    public string productName;
    public int productValue;
    public Sprite productIcon;
    public Product.ProductType productType;
    public string productDescription;
}
