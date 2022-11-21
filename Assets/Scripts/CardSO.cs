using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CardKeyword
{
    NONE,
    ETB,
    MERGE,
    FUSION,
    DEPLOYCONDITION,
    ACTIVEEFFECT,
    COSTEFFECT,
    COSTMOVE,
    EFFECT,
    CONDITION,
    PUSH,
    PUSHMIX,
    STARTGAME,
    STARTROUND,
    PASSIVE,
    SPECIALMOVE,
    SPECIALMOVECOST
}

[CreateAssetMenu]
public class CardSO : ScriptableObject
{
    public int IdCard;
    public int Copies;
    public int Weight;
    public int Speed;
    public int IdOwner;
    public int CardPosition;
    public string IdImageCard;
    public string CardDescription;
    public CardKeyword keyword1;
    public CardKeyword keyword2;
    public CardKeyword keyword3;
}