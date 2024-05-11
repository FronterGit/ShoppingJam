using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerPreview : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI customerNameText;
    [SerializeField] private TMPro.TextMeshProUGUI customerAmountText;
    
    public string customerName;
    public int customerAmount;
    
    void Start()
    {
        customerNameText.text = customerName;
        customerAmountText.text = customerAmount.ToString();
    }
}
