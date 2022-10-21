using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.EventSystems;
using Unity.Netcode;
using UnityEngine.UI;
using Assets.Scripts;
using Unity.Collections;

public class PlaceCard : NetworkBehaviour, IPointerDownHandler
{
    // public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    bool isPlaceable;
    bool isCurrentPlayer;
    bool cardSelected;
    private Vector3 mOffset;
    private float mZCoord;
    PlaceManager placeManager;
    GameObject gridContainer;
    [SerializeField] GameObject CardTableToSpawn;
    void Start()
    {
        //  Position.Value = new Vector3(1f, 1f, 1f);
        placeManager = FindObjectOfType<PlaceManager>();
        gridContainer = GameObject.Find("CanvasHandPlayer/GridManager");
        PlayerActions.current = FindObjectOfType<PlayerActions>();
        TriggerManager.current = FindObjectOfType<TriggerManager>();

    }

    public bool IsCardSelected()
    {
        return cardSelected;
    }
    public bool IsPlaceable()
    {
        return isPlaceable;
    }
    public bool IsCurrentPlayer()
    {
        return isCurrentPlayer;
    }

    public void SetIsPlaceable(bool value)
    {
        isPlaceable = value;
    }

    public void SetIsCurrentPlayer(bool value)
    {
        isCurrentPlayer = value;
    }

    public void SetCardSelected(bool value)
    {
        cardSelected = value;
    }


    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {

        //place card from hand to table section
        if (NetworkManager.Singleton.IsClient) //bisogna mettere molte più condizioni per mettere la carta
        {
            if (placeManager.GetCardSelectedFromHand() != null )//&& (NetworkManager.Singleton.LocalClientId % 2) == 1)
            {
                DeployCardFromHand("DeployTileRight", "RPCT");
                gridContainer.GetComponent<GridContainer>().ResetShowTiles();
            }
            else if (placeManager.GetCardSelectedFromTable() != null)//&& (NetworkManager.Singleton.LocalClientId % 2) == 1)
            {
                MoveCardFromTableRightPlayer("RPCT");
                gridContainer.GetComponent<GridContainer>().ResetShowTiles();
            }
            else if (placeManager.GetCardSelectedFromHand() != null)//&& (NetworkManager.Singleton.LocalClientId % 2) == 0)
            {
                DeployCardFromHand("DeployTileLeft", "LPCT");
                gridContainer.GetComponent<GridContainer>().ResetShowTiles();
            }
            else if (placeManager.GetCardSelectedFromTable() != null)//&& (NetworkManager.Singleton.LocalClientId % 2) == 0)
            {
                MoveCardFromTableRightPlayer("LPCT");
                gridContainer.GetComponent<GridContainer>().ResetShowTiles();
            }
        }
    }

    private void DeployCardFromHand(string deploy, string cardTableTag)
    {
        if (gameObject.tag == deploy)
        {
            ChangeOwnerServerRpc();
            if (placeManager.GetCardSelectedFromHand().GetComponent<CardHand>() != null)
            {
                SpawnCardFromServerRpc(
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().IdCard.Value,
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().Weight.Value,
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().Speed.Value,
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().IdOwner.Value,
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().IdImageCard.Value.ToString(),
          cardTableTag, //RPT Right player Table
          false, //it means that we have to destroy the game object when we move
          gameObject.GetComponent<CoordinateSystem>().x,
          gameObject.GetComponent<CoordinateSystem>().y,
          0,
          0
          );
            }
            else
                Debug.Log("Classe PlaceCard, metodo OnPointerDown, Errore! CardHand vuota");

            placeManager.ResetCardHand();
        }
    }

    private void MoveCardFromTableRightPlayer(string cardTableTag)
    {
        if (gameObject.GetComponent<CoordinateSystem>().isDeployable >= 1) //RPCT stands for RIGHT PLAYER CARD TABLE
                                                                           //togliere ai move points  .GetComponent<CoordinateSystem>().isDeployable, per questo è maggiore uguale di uno il check
        {
            ChangeOwnerServerRpc();
            if (placeManager.GetCardSelectedFromTable().GetComponent<CardTable>() != null)
            {
                SpawnCardFromServerRpc(
          placeManager.GetCardSelectedFromTable().GetComponent<CardTable>().IdCard.Value,
          placeManager.GetCardSelectedFromTable().GetComponent<CardTable>().Weight.Value,
          placeManager.GetCardSelectedFromTable().GetComponent<CardTable>().Speed.Value,
          placeManager.GetCardSelectedFromTable().GetComponent<CardTable>().IdOwner.Value,
          placeManager.GetCardSelectedFromTable().GetComponent<CardTable>().IdImageCard.Value.ToString(),
          cardTableTag, //RPT Right player Table
          true, //it means that we have to destroy the old game object when we move
             gameObject.GetComponent<CoordinateSystem>().x,
          gameObject.GetComponent<CoordinateSystem>().y,
               placeManager.GetCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionX.Value,
          placeManager.GetCardSelectedFromTable().GetComponent<CardTable>().CurrentPositionY.Value
          );
            }
            else
            {
                Debug.Log("Classe PlaceCard, metodo OnPointerDown, Errore! CardHand vuota");
            }

            placeManager.ResetCardHand();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeOwnerServerRpc()
    {
        Debug.Log("1OwnerClientId " + OwnerClientId + " , del server? " + IsOwnedByServer);
        Debug.Log("1NetworkManager.Singleton.LocalClientId " + NetworkManager.Singleton.LocalClientId);
        GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
    }


    [ServerRpc(RequireOwnership = false)]
    public void SpawnCardFromServerRpc(int IdCard, int Weight, int Speed, int IdOwner, string IdImageCard, string tag, bool toDestroy, int x, int y, int xToDelete, int yToDelete) //MyCardStruct cartaDaSpawnare
    {
        Debug.Log("2OwnerClientId " + OwnerClientId + " , del server? " + IsOwnedByServer);
        Debug.Log("2NetworkManager.Singleton.LocalClientId " + NetworkManager.Singleton.LocalClientId);
        CardTableToSpawn.tag = tag;
        NetworkObject go = Instantiate(CardTableToSpawn.GetComponent<NetworkObject>(),
           transform.position, Quaternion.identity);

        go.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
        go.transform.SetParent(transform, false);

        go.GetComponent<CardTable>().IdCard.Value = IdCard;
        go.GetComponent<CardTable>().Weight.Value = Weight;
        go.GetComponent<CardTable>().Speed.Value = Speed;
        go.GetComponent<CardTable>().IdOwner.Value = IdOwner;
        go.GetComponent<CardTable>().IdImageCard.Value = IdImageCard;
        go.GetComponent<CardTable>().CurrentPositionX.Value = x;
        go.GetComponent<CardTable>().CurrentPositionY.Value = y;
        go.GetComponent<NetworkObject>().tag = tag;
        go.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        go.transform.localPosition = new Vector3(0.5f, 0.5f, 1f);

        if (toDestroy)
        {
            gridContainer.GetComponent<GridContainer>().RemoveCardFromTable(xToDelete, yToDelete);
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




    private Vector3 GetMouseAsWorldPoint()
    {

        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;
        // z coordinate of game object on screen

        mousePoint.z = mZCoord;

        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }



    public const byte CustomManualInstantiationEventCode = 1;
}
