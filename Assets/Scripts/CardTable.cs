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
        public NetworkVariable<int> Speed = new NetworkVariable<int>();
        public NetworkVariable<int> IdOwner = new NetworkVariable<int>();
        public NetworkVariable<int> CurrentPositionX = new NetworkVariable<int>();
        public NetworkVariable<int> CurrentPositionY = new NetworkVariable<int>();
        public NetworkVariable<FixedString32Bytes> IdImageCard = new NetworkVariable<FixedString32Bytes>();

        //private void Start()
        //{
        //    idcard = cardso.idcard;
        //    weight = cardso.weight;
        //    speed = cardso.speed;
        //    idowner = cardso.idowner;
        //    idimagecard = cardso.idimagecard;

        //    testo_peso.text = weight.tostring();

        //}

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
            Testo_Peso.text = Weight.Value.ToString();

            //TODO caricare immagine corretta
            GetComponent<Image>().sprite = Resources.Load<Sprite>($"CardIllustration/{IdImageCard.Value}");
        }


    }
}