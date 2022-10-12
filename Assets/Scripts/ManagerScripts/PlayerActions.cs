using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour {
    public static PlayerActions current;
    public bool HasMoved { get; set; }
    public bool HasPlaced { get; set; }
    public bool HasSpelled { get; set; }   
    public bool PlayerTop { get; set; } 
    public bool PlayerBot { get; set; }

    private void Awake() {
        HasMoved = true;
        HasPlaced = true;
        HasSpelled = true;
        PlayerBot = true; 
        PlayerTop = false;
    }

}
