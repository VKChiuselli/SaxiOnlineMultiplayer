using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class CollectionEventHandler : MonoBehaviour
    {
        public static CollectionEventHandler current;

        private void Awake()
        {
            current = this;
        }

        public event Action<GameObject> onPickCardFromCollection;

        public void PickCardFromCollection(GameObject cardPicked)
        {
                if (onPickCardFromCollection != null)
                {
                onPickCardFromCollection(cardPicked);
                }
        }
    }
}