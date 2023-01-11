using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class CollectionEventManager : MonoBehaviour
    {
 
        public int CardCountHand=0; 

        void OnEnable()
        {
            CollectionEventHandler.current.onPickCardFromCollection += PickCardCollection;
        }
        void OnDisable()
        {
            CollectionEventHandler.current.onPickCardFromCollection -= PickCardCollection;
        }

        public void PickCardCollection(GameObject cardCollection)
        {
            Debug.Log("entro1");
            if (cardCollection.GetComponent<CollectionInteractionCard>().IsSelectedCard == 0)
            {
                CardCountHand++;
                Debug.Log("entro2");
            }
            else if (cardCollection.GetComponent<CollectionInteractionCard>().IsSelectedCard == 1)
            {
                CardCountHand--;
                Debug.Log("entro3");
            }
        }

        public bool GetCardHand()
        {
            if (CardCountHand >= 4)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}