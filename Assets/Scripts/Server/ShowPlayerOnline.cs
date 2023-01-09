using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class ShowPlayerOnline : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI codeToShow;


    // Update is called once per frame
    void Update()
    {
        if (GetComponent<TextMeshProUGUI>()!=null)
        {
            GetComponent<TextMeshProUGUI>().text = codeToShow.text;
        }
    }
}
