using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cards;

public class ProductsRow : MonoBehaviour
{
    //Variables
    public TMPro.TextMeshProUGUI productTypeTitle;
    public List<Image> productImages;
    public Transform productImagesParent;
    
    public Image productImagePrefab;

    public void UpdateProducts()
    {
        //Get the size of the product list of our type
        if(ShopManager.activeProductsDict[productTypeTitle.text].size != productImages.Count)
        {
            //If the size of the product list has changed, we need hard reset our images
            ResetImages();
            return;
        }
        
        //Get the product list of our type
        List<Card> products = ShopManager.activeProductsDict[productTypeTitle.text].products;
        
        //Update the images
        for (int i = 0; i < products.Count; i++)
        {
            productImages[i].sprite = products[i].productInfo.productIcon;
        }
    }
    
    private void ResetImages()
    {
        foreach (var image in productImages)
        {
            Destroy(image.gameObject);
        }
        productImages.Clear();
        
        for (int i = 0; i < ShopManager.activeProductsDict[productTypeTitle.text].size; i++)
        {
            Image newImage = Instantiate(productImagePrefab, productImagesParent);
            productImages.Add(newImage);
        }
        
        UpdateProducts();
    }
}
