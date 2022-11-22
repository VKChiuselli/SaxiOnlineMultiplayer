using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Highlight : NetworkBehaviour
{
    private Image _highlight;
    private Color trasparency;


    public override void OnNetworkSpawn()
    {
        transform.localScale = new Vector3(10f, 10f, 10f);
    }


    private void Start()
    {
        _highlight = gameObject.GetComponent<Image>();
        trasparency = _highlight.color;
    }

    public void ResetColorTile()
    {
        gameObject.GetComponent<CoordinateSystem>().typeOfTile = 0;
        _highlight.color = trasparency;
    }

    public void ShowTileCanInteract(int typeOfTile)
    {
        gameObject.GetComponent<CoordinateSystem>().typeOfTile = typeOfTile;
        _highlight.color = Color.red;
        _highlight.color = new Color(1f, 1f, 1f, 0.3f);
    }
}
