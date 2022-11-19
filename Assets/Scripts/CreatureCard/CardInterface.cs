using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CardInterface : NetworkBehaviour
{

    public bool CanWin;
    public string CardDescription;
 public  CardKeyword keyword1;
 public  CardKeyword keyword2;
    public CardKeyword keyword3;

    void Awake()
    {
        CanWin = true;
        CardDescription = "this card is a normal unit";
        keyword1 = CardKeyword.NONE;
        keyword2 = CardKeyword.NONE;
        keyword3 = CardKeyword.NONE;
    }
    public virtual void MyCardEffect()
    {
        Debug.Log("Children must implemenmt MyCardEffect");
    }
    public virtual void MyCardCostEffect(GameObject card)
    {
        Debug.Log("Children must implemenmt MyCardCostEffect");
    }
    public virtual bool MyCardDeploy(GameObject card)
    {
        Debug.Log("Children must implemenmt MyCardDeploy");
        return true;
    }


}
