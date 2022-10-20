using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts
{
    public struct MyCardStruct : INetworkSerializable
    {
        public int IdCard;
        public int Copies;
        public int Weight;
        public int Speed;
        public int IdOwner;
        public int IdImageCard;

        void INetworkSerializable.NetworkSerialize<T>(BufferSerializer<T> serializer)
        {
            serializer.SerializeValue(ref IdCard);
            serializer.SerializeValue(ref Copies);
            serializer.SerializeValue(ref Weight);
            serializer.SerializeValue(ref IdOwner);
            serializer.SerializeValue(ref IdImageCard);
        }

        //public void FillStructure(CardHand card)
        //{
        //    IdCard = card.IdCard;
        //    Copies = card.Copies;
        //    Weight = card.Weight;
        //    Speed = card.Speed;
        //    IdOwner = card.IdOwner;
        //    IdImageCard = card.IdImageCard;
        //}
        // ~INetworkSerializable
    }
}