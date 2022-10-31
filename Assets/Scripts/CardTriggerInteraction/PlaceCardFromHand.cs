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
public class PlaceCardFromHand : NetworkBehaviour, IDropHandler
{
    private float mZCoord;
    PlaceManager placeManager;
    GameObject gridContainer;
    GameObject gameManager;
    GameObject deckManager;
    [SerializeField] GameObject CardTableToSpawn;

    void Start()
    {
        placeManager = FindObjectOfType<PlaceManager>();
        gridContainer = GameObject.Find("CanvasHandPlayer/GridManager");
        gameManager = GameObject.Find("Managers/GameManager");
        deckManager = GameObject.Find("CanvasHandPlayer/PanelPlayerRight");
    }

    public void OnDrop(PointerEventData eventData)
    {//we must put a condition that a card can be dropped only where the tile is eligible
        //in the future we will edit the card: each card has a deploy cost, because if we have a card that doens't cost deploy, we can play it. so we will check below the cost of deploy with the actual deploy.
        //place card from hand to table section
        if (NetworkManager.Singleton.IsClient && placeManager.GetCardSelectedFromHand() != null) //bisogna mettere molte più condizioni per mettere la carta
        {//IsPopupChoosing vuol dire che se è 0, allora non c'è in corso una scelta di popup, se c'è, allora disabilitiamo tutto
            if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 0)
            {
                bool IsOpponentCard = true;
                if (gameObject.GetComponent<CardTable>() != null)
                {
                    if (gameObject.GetComponent<CardTable>().IdOwner.Value == 1)
                    {
                        IsOpponentCard = false; //it means that I'm deploying on opponent card where I can't do it.
                    }
                }
                if (placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().IdOwner.Value == 0 && IsOpponentCard)
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
                bool IsOpponentCard = true;
                if (gameObject.GetComponent<CardTable>() != null)
                {
                    if (gameObject.GetComponent<CardTable>().IdOwner.Value == 0)
                    {
                        IsOpponentCard = false; //it means that I'm deploying on opponent card where I can't do it.
                    }
                }
                if (placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().IdOwner.Value == 1 && IsOpponentCard)
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
            gameManager.GetComponent<GameManager>().SetUnmergeChoosing(0);
        }


    }


    private bool DeployCardFromHand(string deploy, string cardTableTag)
    {//placing into empty space
        if (gameObject.tag == deploy)
        {
            ChangeOwnerServerRpc();
            if (placeManager.GetCardSelectedFromHand().GetComponent<CardHand>() != null)
            {
                DeployCardFromHandServerRpc(
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
            return true;
        }
        else if (gameObject.transform.parent.tag == deploy)
        {//placing into filled space, so it is a merge
            ChangeOwnerServerRpc();
            if (placeManager.GetCardSelectedFromHand().GetComponent<CardHand>() != null)
            {
                MergeCardFromHandServerRpc(
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().IdCard.Value,
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().Weight.Value,
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().Speed.Value,
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().IdOwner.Value,
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().IdImageCard.Value.ToString(),
          cardTableTag, //RPT Right player Table
          false, //it means that we have to destroy the game object when we move
          gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().x,
          gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().y,
          0,
          0
          );
                UpdateWeightTopCard(placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().Weight.Value);
            }
            else
                Debug.Log("Classe PlaceCard, metodo OnPointerDown, Errore! CardHand vuota");

            placeManager.ResetCardHand();
            return true;
        }
        else
            return false;
    }


    private void UpdateWeightTopCard(int cardWeight)
    {
        int finalWeight = cardWeight;
        CardTable cardTable =  gameObject.GetComponent<CardTable>();

        if (cardTable != null)
        {
            foreach (Transform singleCard in transform.parent)
            {
                if (singleCard.GetComponent<CardTable>() != null)
                {
                    finalWeight += singleCard.GetComponent<CardTable>().Weight.Value;
                }
            }
            Debug.Log("finalWeight " + finalWeight);
            Debug.Log(" gameObject.GetComponent<CardTable>().CurrentPositionX.Value " + gameObject.GetComponent<CardTable>().CurrentPositionX.Value);
            Debug.Log(" gameObject.GetComponent<CardTable>().CurrentPositionX.Value " + gameObject.GetComponent<CardTable>().CurrentPositionY.Value);
            UpdateWeightTopCardServerRpc(finalWeight, gameObject.GetComponent<CardTable>().CurrentPositionX.Value, gameObject.GetComponent<CardTable>().CurrentPositionY.Value);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateWeightTopCardServerRpc(int finalWeight, int x, int y)
    {
        GameObject cardOnTop = gridContainer.GetComponent<GridContainer>().GetTopCardOnTile(x, y);
        cardOnTop.GetComponent<CardTable>().MergedWeight.Value = finalWeight;
    }


    [ServerRpc(RequireOwnership = false)]
    public void ChangeOwnerServerRpc()
    {
        Debug.Log("1OwnerClientId " + OwnerClientId + " , del server? " + IsOwnedByServer);
        Debug.Log("1NetworkManager.Singleton.LocalClientId " + NetworkManager.Singleton.LocalClientId);
        GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
    }


    [ServerRpc(RequireOwnership = false)]
    public void DeployCardFromHandServerRpc(int IdCard, int Weight, int Speed, int IdOwner, string IdImageCard, string tag, bool toDestroy, int x, int y, int xToDelete, int yToDelete) //MyCardStruct cartaDaSpawnare
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

        GameObject cardInterface = deckManager.GetComponent<DeckLoad>().GetCard(0);

        NetworkObject cardInterfaceNetwork = Instantiate(cardInterface.GetComponent<NetworkObject>(),
          transform.position, Quaternion.identity);
        cardInterfaceNetwork.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
        cardInterfaceNetwork.transform.SetParent(go.transform, false);
    }

    [ServerRpc(RequireOwnership = false)]
    public void MergeCardFromHandServerRpc(int IdCard, int Weight, int Speed, int IdOwner, string IdImageCard, string tag, bool toDestroy, int x, int y, int xToDelete, int yToDelete) //MyCardStruct cartaDaSpawnare
    {
        Debug.Log("2OwnerClientId " + OwnerClientId + " , del server? " + IsOwnedByServer);
        Debug.Log("2NetworkManager.Singleton.LocalClientId " + NetworkManager.Singleton.LocalClientId);
        CardTableToSpawn.tag = tag;
        NetworkObject go = go = Instantiate(CardTableToSpawn.GetComponent<NetworkObject>(),
        transform.parent.position, Quaternion.identity);

        go.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
        go.transform.SetParent(transform.parent, false);


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
