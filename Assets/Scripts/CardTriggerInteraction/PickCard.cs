using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickCard : MonoBehaviour, IPointerDownHandler
{

    private Vector3 mOffset;
    private float mZCoord;
    GameObject handZone;
    void Start() {
        handZone = GameObject.Find("HandZone");
        PlayerActions.current = FindObjectOfType<PlayerActions>();
        TriggerManager.current = FindObjectOfType<TriggerManager>();
    }


    private Vector3 GetMouseAsWorldPoint() {

        // Pixel coordinates of mouse (x,y)

        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen

        mousePoint.z = mZCoord;

        // Convert it to world points

        return Camera.main.ScreenToWorldPoint(mousePoint);

    }

    bool isSelected;

    public bool GetIsSelected() {
        return isSelected;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (gameObject.tag == "HandPlayer")
        {
            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

            EventsManager.current.PickCardFromHand(gameObject);
            mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
            Debug.Log("Card selected: " + gameObject.name);

            DisableAllHighlight();

            if (gameObject.transform.GetChild(1) != null)
            {
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
            }
        }

    }

    private void DisableAllHighlight()
    {
        for (int i = 0; i < handZone.transform.childCount; i++)
        {
            handZone.transform.GetChild(i).gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
