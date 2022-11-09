using System.Collections;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CardHand : NetworkBehaviour
    {

        [SerializeField] CardSO cardSO;


        [SerializeField] TextMeshProUGUI Testo_Peso;

        [SerializeField] TextMeshProUGUI Testo_Speed;

        [SerializeField] TextMeshProUGUI Testo_Copie;

        public NetworkVariable<int> IdCard = new NetworkVariable<int>();
        public NetworkVariable<int> Copies = new NetworkVariable<int>();
        public NetworkVariable<int> Weight = new NetworkVariable<int>();
        public NetworkVariable<int> Speed = new NetworkVariable<int>();
        public NetworkVariable<int> IdOwner = new NetworkVariable<int>();
        public NetworkVariable<int> DeployCost = new NetworkVariable<int>(1);
        public NetworkVariable<int> CardPosition = new NetworkVariable<int>();
        public NetworkVariable<FixedString32Bytes> IdImageCard = new NetworkVariable<FixedString32Bytes>();

        private void Start()
        {
            IdCard.Value = cardSO.IdCard;
            Copies.Value = cardSO.Copies;
            Weight.Value = cardSO.Weight;
            Speed.Value = cardSO.Speed;
            IdOwner.Value = cardSO.IdOwner;
            IdImageCard.Value = cardSO.IdImageCard;

            Testo_Peso.text = Weight.Value.ToString();
            Testo_Speed.text = Speed.Value.ToString();
            Testo_Copie.text = Copies.Value.ToString();
            GetComponent<Image>().sprite = Resources.Load<Sprite>($"CardIllustration/{IdImageCard.Value}");
        }

        public void PlayCard()
        {
            if (Copies.Value > 0)
            {
                Copies.Value--;
            }
            else
            {
                Debug.Log("Class CardHand: Method PlayCard: zero copy, can't play this card!!");
            }


        }

        private void Update()
        {
                Testo_Copie.text = Copies.Value.ToString();
        }

    }
}