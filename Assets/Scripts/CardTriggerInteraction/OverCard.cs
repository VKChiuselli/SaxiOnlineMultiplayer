using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverCard : MonoBehaviour {

    private Vector3 mOffset;
    private float mZCoord;

    void Start() {
        PlayerActions.current = FindObjectOfType<PlayerActions>();
        TriggerManager.current = FindObjectOfType<TriggerManager>();
    }


    void OnMouseOver() {
        if (gameObject.tag == "HandBot" || gameObject.tag == "HandTop" ||
            gameObject.tag == "TableTop" || gameObject.tag == "TableBot" ||
            gameObject.tag == "GraveBot" || gameObject.tag == "GraveTop" ||
            gameObject.tag == "HandTopSpell" || gameObject.tag == "HandBotSpell") {
            gameObject.transform.GetChild(2).gameObject.SetActive(true);
            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            // EventsManager.current.OverCard(gameObject);
            mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        }
    }
    void OnMouseExit() {
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
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
