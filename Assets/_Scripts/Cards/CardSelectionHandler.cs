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

    private GameObject cardHandParent;

    private Vector3 _originalPosition;
    private Vector3 _originalScale;

    private Card card;

    private void Start()
    {
        cardHandParent = GameObject.FindGameObjectsWithTag("HandParent")[0];
        _originalPosition = transform.position;
        _originalScale = transform.localScale;

        card = GetComponent<Card>();
    }

    private IEnumerator MoveCard(bool startingAnimation)
    {
        if (!card.inHand) yield break;

        Vector3 endPosition;
        Vector3 endScale;

        float elapsedTime = 0f;
        while(elapsedTime < _moveTime)
        {
            elapsedTime += Time.deltaTime;

            if (startingAnimation)
            {
                endPosition = new Vector3(_originalPosition.x, _originalPosition.y + _verticalMoveAmount, 0.1f);
                endScale = _originalScale * _scaleAmount;
            }
            else
            {
                endPosition = _originalPosition;
                endScale = _originalScale;
            }
            
            Vector3 lerpedPos = Vector3.Lerp(_originalPosition, endPosition, elapsedTime / _moveTime);
            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, endScale, elapsedTime / _moveTime);

            transform.position = lerpedPos;
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
