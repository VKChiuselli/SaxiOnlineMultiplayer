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
//place card in empty spaces
public class PlaceCardFromHand : NetworkBehaviour, IDropHandler//, IPointerDownHandler
{
    bool isPlaceable;
    bool isCurrentPlayer;
    bool cardSelected;
    private Vector3 mOffset;
    private float mZCoord;
    PlaceManager placeManager;
    GameObject gridContainer;
    GameObject gameManager;
    [SerializeField] GameObject CardTableToSpawn;

    void Start()
    {
        placeManager = FindObjectOfType<PlaceManager>();
        gridContainer = GameObject.Find("CanvasHandPlayer/GridManager");
        // PlayerActions.current = FindObjectOfType<PlayerActions>();
        //   TriggerManager.current = FindObjectOfType<TriggerManager>();
        gameManager = GameObject.Find("Managers/GameManager");
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


    public void OnDrop(PointerEventData eventData)
    {//we must put a condition that a card can be dropped only where the tile is eligible
        //in the future we will edit the card: each card has a deploy cost, because if we have a card that doens't cost deploy, we can play it. so we will check below the cost of deploy with the actual deploy.
        //place card from hand to table section
        if (NetworkManager.Singleton.IsClient && gameManager.GetComponent<GameManager>().IsPopupChoosing.Value == 0 && placeManager.GetCardSelectedFromHand()!=null) //bisogna mettere molte pi� condizioni per mettere la carta
        {//IsPopupChoosing vuol dire che se � 0, allora non c'� in corso una scelta di popup, se c'�, allora disabilitiamo tutto
            if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 0)//&& (NetworkManager.Singleton.LocalClientId % 2) == 1)
            {
                if (placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().IdOwner.Value == 0)
                {
                    if (gameManager.GetComponent<GameManager>().PlayerZeroDP.Value > 0)
                    {
                        Debug.Log("punto sottratto PlayerZero deploy");
                        bool isDeployed = DeployCardFromHand("DeployTileRight", "RPCT");
                        if (isDeployed)
                        {
                            gameManager.GetComponent<GameManager>().DeployPointSpent(1, 0);
                        }
                    }
                }
            }
            else if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 1)//&& (NetworkManager.Singleton.LocalClientId % 2) == 0)
            {
                if (placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().IdOwner.Value == 1)
                {
                    if (gameManager.GetComponent<GameManager>().PlayerOneDP.Value > 0) //valore maggiore uguale dei punti che "devo spendere", e poi la variabile "devo spendere" va dentro deploypointspent
                    {
                        Debug.Log("punto sottratto PlayerOne deploy");
                        bool isDeployed = DeployCardFromHand("DeployTileLeft", "LPCT");
                        if (isDeployed)
                        {
                            gameManager.GetComponent<GameManager>().DeployPointSpent(1, 1);
                        }
                    }
                }
            }
            gridContainer.GetComponent<GridContainer>().ResetShowTiles();
            placeManager.ResetCardHand();
            placeManager.ResetMergedCardTable();
            placeManager.ResetSingleCardTable();
        }
    

        }

      
    private bool DeployCardFromHand(string deploy, string cardTableTag)
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
          0,
          0
          );
            }
            else
                Debug.Log("Classe PlaceCard, metodo OnPointerDown, Errore! CardHand vuota");

            placeManager.ResetCardHand();
            return true;
        }
        else
            return false;
    }


    [ServerRpc(RequireOwnership = false)]
    public void ChangeOwnerServerRpc()
    {
        Debug.Log("1OwnerClientId " + OwnerClientId + " , del server? " + IsOwnedByServer);
        Debug.Log("1NetworkManager.Singleton.LocalClientId " + NetworkManager.Singleton.LocalClientId);
        GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
    }


    [ServerRpc(RequireOwnership = false)]
    public void SpawnCardFromServerRpc(int IdCard, int Weight, int Speed, int IdOwner, string IdImageCard, string tag, bool toDestroy, int x, int y, int xToDelete, int yToDelete, int checkTransform) //MyCardStruct cartaDaSpawnare
    {
        Debug.Log("2OwnerClientId " + OwnerClientId + " , del server? " + IsOwnedByServer);
        Debug.Log("2NetworkManager.Singleton.LocalClientId " + NetworkManager.Singleton.LocalClientId);
        CardTableToSpawn.tag = tag;
        NetworkObject go=null;
        if (checkTransform == 0)
        {
             go = Instantiate(CardTableToSpawn.GetComponent<NetworkObject>(),
           transform.position, Quaternion.identity);
            go.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
            go.transform.SetParent(transform, false);
        }
        else if (checkTransform == 1)
        {
             go = Instantiate(CardTableToSpawn.GetComponent<NetworkObject>(),
        transform.parent.position, Quaternion.identity);
            go.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
            go.transform.SetParent(transform.parent, false);
        }
        else
        {
            Debug.Log("checkTransform passed wrong!!!");
        }
        
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
        //  gameManager.GetComponent<GameManager>().CurrentTurn.Value = (gameManager.GetComponent<GameManager>().CurrentTurn.Value==1 ? 0 : 1);
        if (toDestroy)
        {
            gridContainer.GetComponent<GridContainer>().RemoveCardFromTable(xToDelete, yToDelete);
        }
    
    }


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