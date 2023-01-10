using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class ShowPlayerOnline : MonoBehaviour
{
   [SerializeField] GameObject relayCode;
 

    // Update is called once per frame
    void Update()
    {
        if (relayCode != null)
        {
            GetComponent<TextMeshProUGUI>().text = relayCode.GetComponent<TextMeshProUGUI>().text;
        }
    }
}
