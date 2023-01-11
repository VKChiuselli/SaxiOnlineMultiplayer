using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Player
{
    public class InitializeHandPlayer : MonoBehaviour
    {
        GameObject deckManager;
        GameObject saveDeck;
        void Start()
        {
         //     saveDeck = FindObjectOfType<SaveDeck>();
            deckManager = GameObject.Find("CanvasHandPlayer/PanelPlayerRight");
        }

      public  void CreateHand()
        {
            saveDeck = GameObject.Find("Relay/Canvas/DeckBuildPanel/BackButtom");
            if (saveDeck != null)
            {
                Debug.Log("CreateHand error");
            }
          foreach(Transform card in deckManager.transform)
            {
                if (card.gameObject.GetComponent<Image>()!=null)
                {
                  if(  card.gameObject.GetComponent<Image>().sprite.name == saveDeck.GetComponent<SaveDeck>().imageNames[0])
                    {
                        Debug.Log("card " + card.gameObject.GetComponent<Image>().sprite.name);
                        break;
                    }
                    if (card.gameObject.GetComponent<Image>().sprite.name == saveDeck.GetComponent<SaveDeck>().imageNames[1])
                    {
                        Debug.Log("card " + card.gameObject.GetComponent<Image>().sprite.name);
                        break;
                    }
                    if (card.gameObject.GetComponent<Image>().sprite.name == saveDeck.GetComponent<SaveDeck>().imageNames[2])
                    {
                        Debug.Log("card " + card.gameObject.GetComponent<Image>().sprite.name);
                        break;
                    }
                    if (card.gameObject.GetComponent<Image>().sprite.name == saveDeck.GetComponent<SaveDeck>().imageNames[3])
                    {
                        Debug.Log("card " + card.gameObject.GetComponent<Image>().sprite.name);
                        break;
                    }
                    Debug.Log("cardRemoved " + card.gameObject.GetComponent<Image>().sprite.name);
                }
            }
        }
      
    }
}