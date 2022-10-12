using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.EventSystems;

public class PlaceCard : MonoBehaviour, IPointerDownHandler
{

    bool isPlaceable;
    bool isCurrentPlayer;
    bool cardSelected;
    private Vector3 mOffset;
    private float mZCoord;
    PlaceManager placeManager;
   // GameBoard gameBoard;    

    void Start() {
        placeManager = FindObjectOfType<PlaceManager>();
  //      gameBoard = FindObjectOfType<GameBoard>();
        PlayerActions.current = FindObjectOfType<PlayerActions>();
        TriggerManager.current = FindObjectOfType<TriggerManager>();
    }

    public bool IsCardSelected() {
        return cardSelected;
    }
    public bool IsPlaceable() {
        return isPlaceable;
    }
    public bool IsCurrentPlayer() {
        return isCurrentPlayer;
    }

    public void SetIsPlaceable(bool value) {
        isPlaceable = value;
    }

    public void SetIsCurrentPlayer(bool value) {
        isCurrentPlayer = value;
    }

    public void SetCardSelected(bool value) {
        cardSelected = value;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPlaceable)
        {
      //      PlaceCard();
        }
    }

    //void OnMouseDown()
    //{
    //    if (TriggerManager.current.EnablePlaceManager)
    //    {
    //        if (isPlaceable && placeManager.GetCardSelectedFromHand() != null && PlayerActions.current.HasPlaced && PlayerActions.current.PlayerBot)
    //        {
    //            PlaceCardBot();
    //        }
    //        else if (isPlaceable && placeManager.GetCardSelectedFromTable() != null && PlayerActions.current.HasMoved && PlayerActions.current.PlayerBot)
    //        {
    //            MoveCardBot();
    //        }
    //        else if (isPlaceable && placeManager.GetCardSelectedFromHand() != null && PlayerActions.current.HasPlaced && PlayerActions.current.PlayerTop)
    //        {
    //            PlaceCardTop();
    //        }
    //        else if (isPlaceable && placeManager.GetCardSelectedFromTable() != null && PlayerActions.current.HasMoved && PlayerActions.current.PlayerTop)
    //        {
    //            MoveCardTop();
    //        }
    //        else if (!isPlaceable && placeManager.GetCardSelectedFromTable() != null && PlayerActions.current.HasMoved && PlayerActions.current.PlayerBot)
    //        {
    //            MoveAndPushCardBot();
    //        }
    //        else if (!isPlaceable && placeManager.GetCardSelectedFromTable() != null && PlayerActions.current.HasMoved && PlayerActions.current.PlayerTop)
    //        {
    //            MoveAndPushCardTop();
    //        }
    //    }

    //}

    // private void MoveAndPushCardTop() {
    //     if (placeManager.GetCardSelectedFromTable().GetComponent<Card>().Ready &&
    //         placeManager.GetCardSelectedFromTable().GetComponent<Card>().CanMoveCardTop(gameObject) &&
    //             gameBoard.CanBePushedTop(gameObject)) {
    //         mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

    //         isPlaceable = false; //placeholder decativate
    //         PlayerActions.current.HasMoved = false;
    //         PushCardTop();
    //         GameObject card = placeManager.GetCardSelectedFromTable();

    //         Instantiate(card, gameObject.GetComponent<BoxCollider>().center, Quaternion.identity).transform.parent = transform;
    //         //  PhotonNetwork.Instantiate(card.name, gameObject.GetComponent<BoxCollider>().center, Quaternion.identity).transform.parent = transform;

    //         mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    //         Destroy(placeManager.GetCardSelectedFromTable());
    //         placeManager.ResetCardTable();
    //     }
    // }

    // private void MoveAndPushCardBot() {
    //     if (placeManager.GetCardSelectedFromTable().GetComponent<Card>().Ready &&
    //         placeManager.GetCardSelectedFromTable().GetComponent<Card>().CanMoveCardBot(gameObject) &&
    //        gameBoard.CanBePushedBot(gameObject)
    //         ) {
    //         mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

    //         isPlaceable = false; //placeholder decativate
    //         PlayerActions.current.HasMoved = false;
    //         PushCardBot();
    //         GameObject card = placeManager.GetCardSelectedFromTable();

    //         Instantiate(card, gameObject.GetComponent<BoxCollider>().center, Quaternion.identity).transform.parent = transform;
    //         //     PhotonNetwork.Instantiate(card.name, gameObject.GetComponent<BoxCollider>().center, Quaternion.identity).transform.parent = transform;

    //         mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    //         Destroy(placeManager.GetCardSelectedFromTable());
    //         placeManager.ResetCardTable();
    //     }
    // }

    // private void PushCardBot() {
    //     //    PhotonNetwork.Instantiate(transform.GetChild(0).gameObject.name, gameBoard.SpotPushedBot(gameObject), Quaternion.identity).transform.parent = gameBoard.GetTransformPushedBot(gameObject);
    //     if (gameBoard.CanBePushedBot(gameObject)) {
    //         Instantiate(transform.GetChild(0).gameObject, gameBoard.SpotPushedBot(gameObject), Quaternion.identity).transform.parent = gameBoard.GetTransformPushedBot(gameObject);
    //         Destroy(transform.GetChild(0).gameObject);
    //     }
    // }
    // private void PushCardTop() {
    //     if (gameBoard.CanBePushedTop(gameObject)) {
    //         Instantiate(transform.GetChild(0).gameObject, gameBoard.SpotPushedTop(gameObject), Quaternion.identity).transform.parent = gameBoard.GetTransformPushedTop(gameObject);
    //         Destroy(transform.GetChild(0).gameObject);
    //     }
    // }

    // private void MoveCardTop() {
    //     if (placeManager.GetCardSelectedFromTable().GetComponent<Card>().Ready && placeManager.GetCardSelectedFromTable().GetComponent<Card>().CanMoveCardTop(gameObject)) {
    //         mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

    //         isPlaceable = false; //placeholder decativate
    //         PlayerActions.current.HasMoved = false;
    //         GameObject card = placeManager.GetCardSelectedFromTable();

    //         Instantiate(card, gameObject.GetComponent<BoxCollider>().center, Quaternion.identity).transform.parent = transform;

    //         mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    //         Destroy(placeManager.GetCardSelectedFromTable());
    //         placeManager.ResetCardTable();
    //     }
    // }

    //private GameObject SpawnCardOnTable()
    //{
    //    GameObject card = Instantiate(placeManager.GetCardSelectedFromHand(), gameObject.GetComponent<BoxCollider>().center, Quaternion.identity);
    //    //     GameObject card = PhotonNetwork.Instantiate(RemoveCloneString(placeManager.GetCardSelectedFromHand().name), gameObject.GetComponent<BoxCollider>().center, Quaternion.identity);
    //    card.transform.parent = transform;
    //    card.AddComponent<GridCoordinate>();
    //    card.GetComponent<GridCoordinate>().x = gameObject.GetComponent<GridCoordinate>().x;
    //    card.GetComponent<GridCoordinate>().y = gameObject.GetComponent<GridCoordinate>().y;
    //    card.transform.GetChild(1).gameObject.SetActive(false);
    //    return card;
    //}

    // private string RemoveCloneString(string name) {
    //     return     name.Replace("(Clone)", "");
    // }

    // private void MoveCardBot() {
    //     if (placeManager.GetCardSelectedFromTable().GetComponent<Card>().Ready && placeManager.GetCardSelectedFromTable().GetComponent<Card>().CanMoveCardBot(gameObject)) {
    //         mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

    //         isPlaceable = false; //placeholder decativate
    //         PlayerActions.current.HasMoved = false;
    //         GameObject card = placeManager.GetCardSelectedFromTable();

    //         Instantiate(card, gameObject.GetComponent<BoxCollider>().center, Quaternion.identity).transform.parent = transform;

    //         mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    //         Destroy(placeManager.GetCardSelectedFromTable());
    //         placeManager.ResetCardTable();
    //     }
    // }
    // private void PlaceCardTop() {
    //     if (placeManager.GetCardSelectedFromHand().GetComponent<Card>().WherePlaceTheCardTop(gameObject) &&
    //          placeManager.GetCardSelectedFromHand().transform.GetChild(1).GetComponent<CardCounter>().numberOfCardPlayable > 0) {
    //         mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
    //         placeManager.GetCardSelectedFromHand().transform.GetChild(1).GetComponent<CardCounter>().numberOfCardPlayable--;
    //         isPlaceable = false; //placeholder decativate
    //         PlayerActions.current.HasPlaced = false;

    //         GameObject card = SpawnCardOnTable();
    //         card.tag = "TableTop";
    //         card.layer = LayerMask.NameToLayer("Default");
    //         card.transform.localScale = new Vector3(1, 1, 1);
    //         card.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Default");
    //         mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    //         placeManager.ResetCardHand();
    //     }

    // }

    //private void PlaceCard()
    //{
       
    //    {
    //        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
    //  //      placeManager.GetCardSelectedFromHand().transform.GetChild(1).GetComponent<CardCounter>().numberOfCardPlayable--;
    //        isPlaceable = false; //placeholder decativate
    //        PlayerActions.current.HasPlaced = false;

    //        GameObject card = SpawnCardOnTable();
    //        card.tag = "PlayerCardOnTable";
    //        card.layer = LayerMask.NameToLayer("Default");
    //        card.transform.localScale = new Vector3(1, 1, 1);
    //        card.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Default");
    //        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    //        placeManager.ResetCardHand();
    //    }
    //}

    private Vector3 GetMouseAsWorldPoint() {

        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;
        // z coordinate of game object on screen

        mousePoint.z = mZCoord;

        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }



    public const byte CustomManualInstantiationEventCode = 1;
}
