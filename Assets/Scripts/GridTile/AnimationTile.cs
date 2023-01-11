using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GridTile
{
    public class AnimationTile : MonoBehaviour
    {

        AnimationTileManager animationTileManager;
    


        private void Start()
        {
            GameObject atm = GameObject.Find("CoreGame/Managers/AnimationManager");
            animationTileManager = atm.GetComponent<AnimationTileManager>();
        }

        private void OnEnable()
        {
            Highlight.myTypeOfTileDelegate += ActiveAnimation;
        }

        private void OnDisable()
        {
            Highlight.myTypeOfTileDelegate -= ActiveAnimation;
        }

        private void ActiveAnimation()
        {
            if (GetComponent<CoordinateSystem>().typeOfTile == 1)
            {
                ParticleSystem clone = animationTileManager.GetRandomEffect(1).GetComponent<ParticleSystem>();//.Emit(3);
                ParticleSystem asd = Instantiate(clone, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as ParticleSystem;
                Destroy(asd);
            }
            else if (GetComponent<CoordinateSystem>().typeOfTile == 2)
            {
                ParticleSystem clone = animationTileManager.GetRandomEffect(2).GetComponent<ParticleSystem>();//.Emit(3);
                ParticleSystem asd = Instantiate(clone, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as ParticleSystem;
                Destroy(asd);
            }
            else if (GetComponent<CoordinateSystem>().typeOfTile == 7)
            {
                ParticleSystem clone = animationTileManager.GetRandomEffect(7).GetComponent<ParticleSystem>();//.Emit(3);
                ParticleSystem asd = Instantiate(clone, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as ParticleSystem;
                Destroy(asd);
            }
            else
            {
                Debug.Log("cioasdz");
            }
        }

    }
}