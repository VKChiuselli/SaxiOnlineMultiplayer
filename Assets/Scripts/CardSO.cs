using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EquipmentType
{
    Armor, Weapon, Jewlery
}

[CreateAssetMenu]
public class CardSO : ScriptableObject
{
    public int IdCard;
    public int Copies;
    public int Weight;
    public int Speed;
    public int IdOwner;
    public string IdImageCard;



}