using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class CardInfoSetter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public TMPro.TextMeshProUGUI energyCostText;
    [SerializeField] public TMPro.TextMeshProUGUI titleText;
    [SerializeField] public TMPro.TextMeshProUGUI descriptionText;
    [SerializeField] public TMPro.TextMeshProUGUI categoryText;
    [SerializeField] public TMPro.TextMeshProUGUI rarityText;
    
    void Start()
    {
        SetCardInfo(GetComponent<Card>());
    }
    public void SetCardInfo(Card card)
    {
        titleText.text = card.title;
        descriptionText.text = card.description;
        categoryText.text = card.category.ToString();
        rarityText.text = card.rarity.ToString();
        energyCostText.text = card.energyCost.ToString();
    }
}
