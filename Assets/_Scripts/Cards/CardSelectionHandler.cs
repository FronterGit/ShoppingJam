using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cards;

public class CardSelectionHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private float _verticalMoveAmount = 0.5f;
    [SerializeField] private float _moveTime = 0.1f;
    [Range(0f, 2f), SerializeField] private float _scaleAmount = 1.1f;

    private Vector3 _originalPosition;
    private Vector3 _originalScale;

    private Card card;

    private void Start()
    {
        _originalPosition = transform.position;
        _originalScale = transform.localScale;

        card = GetComponent<Card>();
    }

    private IEnumerator MoveCard(bool startingAnimation)
    {   
        // get the card's current position
        Vector3 endPosition = transform.position;
        Vector3 endScale;

        float elapsedTime = 0f;
        while(elapsedTime < _moveTime)
        {
            elapsedTime += Time.deltaTime;

            if (startingAnimation)
            {
                endPosition = new Vector3(transform.position.x, transform.position.y + _verticalMoveAmount, transform.position.z + 0.1f);
                endScale = _originalScale * _scaleAmount;
            }
            else
            {
                endScale = _originalScale;
            }

            //set the card's position to the end position
            transform.position = endPosition;

            //set its z position to 0.1 to make sure it's on top of the other cards
            
            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, endScale, elapsedTime / _moveTime);

           // transform.position = lerpedPos;
            transform.localScale = lerpedScale;

            yield return null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;  
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventData.selectedObject = null;
    }

    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(MoveCard(true));
    }

    public void OnDeselect(BaseEventData eventData)
    {
        StartCoroutine(MoveCard(false));
    }
}
