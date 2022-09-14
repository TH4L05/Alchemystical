using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonExtra : Button
{
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        //EventSystem.current.SetSelectedGameObject(null);
        
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        EventSystem.current.SetSelectedGameObject(gameObject);
        //DoStateTransition(SelectionState.Normal, false);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        //DoStateTransition(SelectionState.Normal, false);
    }

    public void ResetTransition()
    {
        DoStateTransition(SelectionState.Normal, false);
    }
}
