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
    GameObject SpawnManager;
    [SerializeField] GameObject CardTableToSpawn;

    void Start()
    {
        placeManager = FindObjectOfType<PlaceManager>();
        gridContainer = GameObject.Find("CanvasHandPlayer/GridManager");
        gameManager = GameObject.Find("Managers/GameManager");
        SpawnManager = GameObject.Find("Managers/SpawnManager");
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
                        DeployCardFromHand("DeployTileRight", "RPCT");
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
                        DeployCardFromHand("DeployTileLeft", "LPCT");
                    }
                }
            }
            gridContainer.GetComponent<GridContainer>().ResetShowTiles();
            placeManager.ResetCardHand();
            placeManager.ResetMergedCardTable();
            placeManager.ResetSingleCardTable();
            gameManager.GetComponent<GameManager>().SetUnmergeChoosing(0);
            gameManager.GetComponent<GameManager>().SetIsPopupChoosing(0);
        }


    }


    private bool DeployCardFromHand(string deploy, string cardTableTag)
    {//placing into empty space
        if (gameObject.tag == deploy)
        {
            ChangeOwnerServerRpc();
            if (placeManager.GetCardSelectedFromHand().GetComponent<CardHand>() != null)
            {
                if (placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().Copies.Value == 0)
                {
                    Debug.Log("  Errore! not enough copies");
                    placeManager.ResetCardHand();
                    return false;
                }
                SpawnManager.GetComponent<SpawnCardServer>().DeployServerRpc(
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().IdCard.Value,
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().Weight.Value,
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().Speed.Value,
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().IdOwner.Value,
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().IdImageCard.Value.ToString(),
          cardTableTag, //RPT Right player Table
          gameObject.GetComponent<CoordinateSystem>().x,
          gameObject.GetComponent<CoordinateSystem>().y,
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().DeployCost.Value,
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().Copies.Value,
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().CardPosition.Value
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
                if (placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().Copies.Value == 0)
                {
                    Debug.Log("  Errore! not enough copies");
                    placeManager.ResetCardHand();
                    return false;
                }
                SpawnManager.GetComponent<SpawnCardServer>().DeployMergeServerRpc(
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().IdCard.Value,
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().Weight.Value,
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().Speed.Value,
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().IdOwner.Value,
          placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().IdImageCard.Value.ToString(),
          cardTableTag, //RPT Right player Table
          gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().x,
          gameObject.transform.parent.gameObject.GetComponent<CoordinateSystem>().y,
            placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().DeployCost.Value,
            placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().Copies.Value,
            placeManager.GetCardSelectedFromHand().GetComponent<CardHand>().CardPosition.Value
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
        GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
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
