using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionCard : MonoBehaviour//, IPointerDownHandler
{

    PlaceManager placeManager;

    //TODO fare popup che blocchi qualsiasi interazione di placecard e 
    void Start()
    {
        placeManager = FindObjectOfType<PlaceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
