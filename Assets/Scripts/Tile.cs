using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    [SerializeField] private Material _baseColor, _deployStartColor;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private GameObject _highlight;

    public float variabile_x;
    public float variabile_y;
    public float risoluzione_x;
    public float risoluzione_y;

    private void Start() {
        risoluzione_x = Screen.width;
        risoluzione_y = Screen.height;
    }

    public void Init(bool isDeployStart) {
        _meshRenderer.material = isDeployStart ? _deployStartColor : _baseColor;
    }

    void OnMouseEnter() {
        _highlight.SetActive(true);
    }

    void OnMouseExit() {
        _highlight.SetActive(false);
    }

 

    private void Update() {
        resolution_control();
    }

    private void resolution_control() {
        risoluzione_x = Screen.width;
        risoluzione_y = Screen.height;
        transform.localScale = new Vector3(0.0004f * risoluzione_x, 0.0005f * risoluzione_y, 1f);
    }
}