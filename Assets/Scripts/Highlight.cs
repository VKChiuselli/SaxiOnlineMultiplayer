using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Highlight : NetworkBehaviour
{
    [SerializeField] private Image _highlight;
    private Color trasparency;

    private void Start()
    {
        trasparency = _highlight.color;
    }

    public void ResetColorTile()
    {
        gameObject.GetComponent<CoordinateSystem>().isDeployable = 0;
        _highlight.color = trasparency;
    }

    public void ShowTileCanDeploy()
    {
        gameObject.GetComponent<CoordinateSystem>().isDeployable = 1;
        _highlight.color = Color.red;
        _highlight.color = new Color(1f, 1f, 1f, 0.3f);
    }
}
