using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoGame : MonoBehaviour
{

     GameObject gameManager;
    string info = "";

    private void Start()
    {
        gameManager = GameObject.Find("Managers/GameManager");

    }

    private void Update()
    {
        if (gameManager != null)
        {
            if (gameManager.GetComponent<GameManager>().CurrentTurn.Value == 0)
            {
                info = $"CURRENT PLAYER: RIGHT\n DEPLOY POINTS: {gameManager.GetComponent<GameManager>().PlayerZeroDP.Value}\n MOVE POINTS:  {gameManager.GetComponent<GameManager>().PlayerZeroMP.Value}";
            }
            else
            {
                info = $"CURRENT PLAYER: LEFT\n DEPLOY POINTS: {gameManager.GetComponent<GameManager>().PlayerOneDP.Value}\n MOVE POINTS:  {gameManager.GetComponent<GameManager>().PlayerOneMP.Value}";
            }
        }
        else
        {
            gameManager = GameObject.Find("Managers/GameManager");
        }
       
        gameObject.GetComponent<TextMeshProUGUI>().text = info;
      
    }

}
