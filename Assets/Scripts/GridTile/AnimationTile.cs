using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GridTile
{
    public class AnimationTile : MonoBehaviour
    {

        private void Update()
        {
            if (GetComponent<CoordinateSystem>().typeOfTile == 7)
            {
                //TODO trigger animation
            }
            else
            {
                //TODO stop animation
            }
        }

    }
}