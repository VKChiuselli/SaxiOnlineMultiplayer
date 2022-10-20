using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color _baseColor, _deployStartColor;
    [SerializeField] private Image _renderer;
    PlaceManager placeManager;
    public float variabile_x;
    public float variabile_y;
    public float risoluzione_x;
    public float risoluzione_y;

    private void Start()
    {
        placeManager = FindObjectOfType<PlaceManager>();
        risoluzione_x = Screen.width;
        risoluzione_y = Screen.height;
    }

    public void Init(bool isDeployStart)
    {
        _renderer.color = isDeployStart ? _deployStartColor : _baseColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.GetComponent<Image>().color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.GetComponent<Image>().color = Color.white;
    }



    private void Update()
    {
        //  resolution_control();
    }

    private void resolution_control()
    {
        risoluzione_x = Screen.width;
        risoluzione_y = Screen.height;
        transform.localScale = new Vector3(variabile_x * risoluzione_x, variabile_y * risoluzione_y);
    }


}