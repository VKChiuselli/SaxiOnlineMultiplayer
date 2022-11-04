using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoGame : MonoBehaviour
{

    [SerializeField] GameObject GM;

    private void Update()
    {
        string info = "";

        if (GM.GetComponent<GameManager>().CurrentTurn.Value == 0)
        {
            info = $"CURRENT PLAYER: RIGHT\n DEPLOY POINTS: {GM.GetComponent<GameManager>().PlayerZeroDP.Value}\n MOVE POINTS:  {GM.GetComponent<GameManager>().PlayerZeroMP.Value}";
        }
        else
        {
            info = $"CURRENT PLAYER: LEFT\n DEPLOY POINTS: {GM.GetComponent<GameManager>().PlayerOneDP.Value}\n MOVE POINTS:  {GM.GetComponent<GameManager>().PlayerOneMP.Value}";
        }
        gameObject.GetComponent<TextMeshProUGUI>().text = info;
      
    }

}
