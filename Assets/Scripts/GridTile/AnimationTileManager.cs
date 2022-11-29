using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GridTile
{
    public class AnimationTileManager : MonoBehaviour
    {

        [SerializeField] List<GameObject> particleEffectsTypeOne;
        [SerializeField] List<GameObject> particleEffectsTypeTwo;
        [SerializeField] List<GameObject> particleEffectsTypeSeven;

        public GameObject GetRandomEffect(int v)
        {
            GameObject a =null;
            if (v == 1)
            {
                return particleEffectsTypeOne[Random.Range(0, particleEffectsTypeOne.Count)];
            }
            else if (v == 2)
            {
                return particleEffectsTypeOne[Random.Range(0, particleEffectsTypeTwo.Count)];
            } else if (v == 7)
            {
                return particleEffectsTypeOne[Random.Range(0, particleEffectsTypeSeven.Count)];
            }
            Debug.Log("no random effect found");
            return a;
        }
    }
}