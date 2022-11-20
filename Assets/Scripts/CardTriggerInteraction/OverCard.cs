using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Assets.Scripts;

public class OverCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] TextMeshProUGUI showInfo;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.GetComponent<CardHand>() != null)
        {
        showInfo.text = gameObject.GetComponent<CardHand>().CardDescription;
        }
        else if (gameObject.GetComponent<CardTable>() != null)
        {
            showInfo.text = "TODO implement card table info text";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        showInfo.text = "";
    }
}
