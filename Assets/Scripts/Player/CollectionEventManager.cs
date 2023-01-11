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
            if (cardCollection.GetComponent<CollectionInteractionCard>().IsSelectedCard == 0)
            {
                CardCountHand++;
            }
            else if (cardCollection.GetComponent<CollectionInteractionCard>().IsSelectedCard == 1)
            {
                CardCountHand--;
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