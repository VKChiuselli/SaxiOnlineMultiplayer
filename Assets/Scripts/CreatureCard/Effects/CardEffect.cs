using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect : MonoBehaviour {

    public string EffectDescription;
    public bool triggerEffectOneTime;

    void Start() {
        PlayerActions.current = FindObjectOfType<PlayerActions>();
        EffectDescription = "";
        triggerEffectOneTime = true;
    }

    public virtual void MyCardEffect90() {
        Debug.Log("Childrenzzzz");
    }

}
