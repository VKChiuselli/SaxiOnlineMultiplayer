using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Player
{
    public class CollectionInteractionCard : MonoBehaviour, IPointerDownHandler
    {

        public int IsSelectedCard=0;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (IsSelectedCard == 0 && collectionEventManager.GetCardHand())
            {
                CollectionEventHandler.current.PickCardFromCollection(gameObject);
                IsSelectedCard = 1;
                GetComponent<Image>().color = Color.green;
            }
            else if (IsSelectedCard == 1)
            {
                CollectionEventHandler.current.PickCardFromCollection(gameObject);
                IsSelectedCard = 0;
                GetComponent<Image>().color = Color.red;
            }
        }
        CollectionEventManager collectionEventManager;
        void Start()
        {
            collectionEventManager = FindObjectOfType<CollectionEventManager>();
            GetComponent<Image>().color = Color.red;
        }

      
    }
}