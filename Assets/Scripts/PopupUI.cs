using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupUI : MonoBehaviour
{
    [SerializeField] GameObject pushButton;
    [SerializeField] GameObject mergeButton;
    [SerializeField] GameObject cancelButton;
    GridContainer gridContainer;

    int _xOldTile;
    int _yOldTile;
    int _xNewTile;
    int _yNewTile;
    int _typeOfTile;
    GameObject SpawnManager;

    void Start()
    {
        SpawnManager = GameObject.Find("CoreGame/Managers/SpawnManager");
    }


    private void OnEnable()
    {
        gameObject.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(() => button_pressed(0));
        gameObject.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => button_pressed(1));
        gameObject.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => button_pressed(2));
        gameObject.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(() => button_pressed(3));
        gridContainer = FindObjectOfType<GridContainer>();
    }

    private void button_pressed(int numberAction)
    {
        if (numberAction == 0)
        {
            Debug.Log("TODO push");
            int result = SpawnManager.GetComponent<SpawnCardServer>().PushCardFromTable(_xOldTile, _yOldTile, _xNewTile, _yNewTile);
            if (result>0)
            {
                Debug.Log("Card pushed from UI!");
            }
            else
            {
                Debug.Log("Card not pushed from UI!");
            }
            gameObject.SetActive(false);
        }
        else
        if (numberAction == 1)
        {
            Debug.Log("TODO move all cards");
            SpawnManager.GetComponent<SpawnCardServer>().MoveToFriendlyTile(_xOldTile, _yOldTile, _xNewTile, _yNewTile);
            gameObject.SetActive(false);
        }
        else
        if (numberAction == 2)
        {
            Debug.Log("TODO move top card");
            SpawnManager.GetComponent<SpawnCardServer>().MoveTopCardToAnotherTile(_xOldTile, _yOldTile, _xNewTile, _yNewTile);
            gameObject.SetActive(false);
        }
        else
        if (numberAction == 3)
        {
            Debug.Log("TODO cancel");
            gameObject.SetActive(false);
        }
    }
 

    public void InitializeVariables(int xOldTile, int yOldTile, int xNewTile, int yNewTile, int typeOfTile)
    {
        _xOldTile = xOldTile;
        _yOldTile = yOldTile;
        _xNewTile = xNewTile;
        _yNewTile = yNewTile;
        _typeOfTile = typeOfTile;


        if (xOldTile == 0 && yOldTile == 0 && xNewTile == 0 && yNewTile == 0)
        {
            return;
        }

        GameObject tile = gridContainer.GetTile(xOldTile, yOldTile);
        if (tile.transform.childCount == 1)
        {
            if (typeOfTile == 1)
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
                gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "MOVE";
                gameObject.transform.GetChild(2).gameObject.SetActive(false);
            }
            else if (typeOfTile == 2)
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
                gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "MOVE";
                gameObject.transform.GetChild(2).gameObject.SetActive(false);
            }
            else if (typeOfTile == 3)
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                gameObject.transform.GetChild(1).gameObject.SetActive(false);
                gameObject.transform.GetChild(2).gameObject.SetActive(false);
            }
        }
        else if (tile.transform.childCount > 1)
        {
            if (typeOfTile == 1)
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
                gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "MOVE ALL CARDS";
                gameObject.transform.GetChild(2).gameObject.SetActive(true);
                gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "MOVE TOP CARD";
            }
            else if (typeOfTile == 2)
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
                gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "MOVE ALL CARDS";
                gameObject.transform.GetChild(2).gameObject.SetActive(true);
                gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "MOVE TOP CARD";
            }
            else if (typeOfTile == 3)
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                gameObject.transform.GetChild(1).gameObject.SetActive(false);
                gameObject.transform.GetChild(2).gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("InitializeVariables ERROR! no child on the tile");
        }



    }

    private void OnDisable()
    {
        _xOldTile = 0;
        _yOldTile = 0;
        _xNewTile = 0;
        _yNewTile = 0;
    }

}
