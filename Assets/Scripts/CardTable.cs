using System;
using System.Collections;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CardTable : NetworkBehaviour
    {

        [SerializeField] TextMeshProUGUI Testo_Peso;

        public NetworkVariable<int> IdCard = new NetworkVariable<int>();
        public NetworkVariable<int> Weight = new NetworkVariable<int>();
        public NetworkVariable<int> MergedWeight = new NetworkVariable<int>();
        public NetworkVariable<int> MoveCost = new NetworkVariable<int>(1);
        public NetworkVariable<int> CurrentSpeed = new NetworkVariable<int>(); //it is max move, how much can move max a card each turn
        public NetworkVariable<int> BaseSpeed = new NetworkVariable<int>(); //it is max move, how much can move max a card each turn
        public NetworkVariable<int> IdOwner = new NetworkVariable<int>();
        public NetworkVariable<int> CurrentPositionX = new NetworkVariable<int>();
        public NetworkVariable<int> CurrentPositionY = new NetworkVariable<int>();
        public NetworkVariable<bool> IsOnTop = new NetworkVariable<bool>(true);
        public NetworkVariable<FixedString32Bytes> IdImageCard = new NetworkVariable<FixedString32Bytes>();

        GameObject SpawnManager;

        private void Start()
        {
            SpawnManager = GameObject.Find("CoreGame/Managers/SpawnManager");
        }

        //public void ConvertCardFromHandToTable(MyCardStruct cardHandToConvert)
        //{
        //    IdCard.Value = cardHandToConvert.IdCard;
        //    Weight.Value = cardHandToConvert.Weight;
        //    Speed.Value = cardHandToConvert.Speed;
        //    IdOwner.Value = cardHandToConvert.IdOwner;
        //    IdImageCard.Value = cardHandToConvert.IdImageCard;
        //}

        private void Update()
        {
            if (MergedWeight.Value > 0)
            {
                Testo_Peso.text = MergedWeight.Value.ToString();
            }
            else
            {
                Testo_Peso.text = Weight.Value.ToString();
            }

            //TODO caricare immagine corretta
            GetComponent<Image>().sprite = Resources.Load<Sprite>($"CardIllustration/{IdImageCard.Value}");
        }

        [ServerRpc(RequireOwnership = false)]
        public void ChangeMoveCostServerRpc(int newMoveCost)
        {
            ChangeMoveCost(newMoveCost);
        }  
        
        public void ChangeMoveCost(int newMoveCost)
        {
            MoveCost.Value = newMoveCost;
        }


        public void RefreshSpeed()
        {
            CurrentSpeed.Value = BaseSpeed.Value;
        }

        public void RemoveSpeed()
        {
            CurrentSpeed.Value = CurrentSpeed.Value - 1;
        }

    }
}