using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEvents : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnMouseEnter()
    {
        Debug.Log("Mouse Enter");
        animator.SetTrigger("AnimateUp");
    }

    public void OnMouseExit()
    {
        animator.SetTrigger("AnimateDown");
    }
}
