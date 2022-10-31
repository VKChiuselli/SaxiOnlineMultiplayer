using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardCard : MonoBehaviour {

    private Vector3 mOffset;
    private float mZCoord;

    void Start() {
        PlayerActions.current = FindObjectOfType<PlayerActions>();
        TriggerManager.current = FindObjectOfType<TriggerManager>();
    }

    void OnMouseDown() {
        if (TriggerManager.current.EnableDiscardManager) {
            //pick card from hand bot
            if (gameObject.tag == "HandBot" && PlayerActions.current.PlayerBot) {
                mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

                EventsManager.current.DiscardCardFromHand(gameObject);
                mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
            }
            //pick card from hand top
            else if (gameObject.tag == "HandTop" && PlayerActions.current.PlayerTop) {
                mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

                EventsManager.current.DiscardCardFromHand(gameObject);
                mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
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
