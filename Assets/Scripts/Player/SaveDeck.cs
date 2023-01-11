using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Player
{
    public class SaveDeck : MonoBehaviour
    {

        [SerializeField] GameObject fatherOfimages;
       public List<string> imageNames;
        public void     SaveAndGoBackHome()
        {
            imageNames = new List<string>();

            foreach (Transform cardSelected in fatherOfimages.transform)
            {
                if (cardSelected.gameObject.GetComponent<CollectionInteractionCard>() != null)
                {
                    if (cardSelected.gameObject.GetComponent<CollectionInteractionCard>().IsSelectedCard == 1)
                    {
                        imageNames.Add( cardSelected.gameObject.GetComponent<Image>().sprite.name);
                    }
                }
            }
        }

    }
}