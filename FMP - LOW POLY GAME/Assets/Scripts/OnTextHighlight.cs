using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnTextHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Vector3 origScale;

    void Start()
    {
        origScale = this.transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.scale(gameObject, new Vector3(origScale.x * 1.3f, origScale.y * 1.3f, origScale.z), 0.08f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {        
        LeanTween.scale(gameObject, new Vector3(origScale.x, origScale.y, origScale.z), 0.1f);        
    }
}
