using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    [SerializeField] private MeshRenderer _highlight;
    [SerializeField] private Material rosso;
    [SerializeField] private Material verde;
    void OnMouseEnter()
    {
        _highlight.material = rosso;
    }

    void OnMouseExit()
    {
        _highlight.material = verde;
    }

}
