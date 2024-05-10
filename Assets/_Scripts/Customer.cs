using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public int speed;
    public bool canMove = true;
    public float storeTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ShopHitbox"))
        {
            canMove = false;
            StartCoroutine(StoreTime());
        }
    }
    
    private IEnumerator StoreTime()
    {
        yield return new WaitForSeconds(storeTime);
        canMove = true;
    }
}
