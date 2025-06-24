using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonSelectionHandler : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private TMP_Text _text;

    public void OnDeselect(BaseEventData eventData)
    { 
        _text.color = Color.black;
    }

    public void OnSelect(BaseEventData eventData)
    {
        _text.color = Color.white;
    }
}
