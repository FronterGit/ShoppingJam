using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static event System.Action<GameObject, bool> OnCustomer;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Customer"))
        {
            OnCustomer?.Invoke(other.gameObject, true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Customer"))
        {
            OnCustomer?.Invoke(other.gameObject, false);
        }
    }
}
