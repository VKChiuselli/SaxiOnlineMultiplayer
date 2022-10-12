using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActiveCard : MonoBehaviour {

    bool isSelected;

    void OnMouseOver() {
        isSelected = true;
    }

    void OnMouseExit() {
        isSelected = false;
    }

    private Vector3 mOffset;
    private float mZCoord;

    void Start() {
        PlayerActions.current = FindObjectOfType<PlayerActions>();
        TriggerManager.current = FindObjectOfType<TriggerManager>();
    }

    void OnEnable() {
        OnMouseRightClick();
    }

    void OnDisable() {
        OnMouseRightClick();
    }


    private void Update() {
        OnMouseRightClick();
    }

    void OnMouseRightClick() {
        if (TriggerManager.current.EnableActiveCardManager) {
            if (Input.GetMouseButtonDown(1) && isSelected) {
                //active card from table bot
                if (gameObject.tag == "TableBot" && PlayerActions.current.PlayerBot) {
                    mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
                    EventsManager.current.ActiveCardFromTable(gameObject);
                    mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
                }
                //active card from table top
                else if (gameObject.tag == "TableTop" && PlayerActions.current.PlayerTop) {
                    mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
                    EventsManager.current.ActiveCardFromTable(gameObject);
                    mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
                }
            }
        }
    }

    private Vector3 GetMouseAsWorldPoint() {

        // Pixel coordinates of mouse (x,y)

        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen

        mousePoint.z = mZCoord;

        // Convert it to world points

        return Camera.main.ScreenToWorldPoint(mousePoint);

    }
}
