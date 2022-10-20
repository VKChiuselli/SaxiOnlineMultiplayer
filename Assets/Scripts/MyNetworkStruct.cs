using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts
{
    public struct MyNetworkStruct : INetworkSerializable
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public GameObject cardPassed;

   
        void INetworkSerializable.NetworkSerialize<T>(BufferSerializer<T> serializer) 
        {
            serializer.SerializeValue(ref Position);
            serializer.SerializeValue(ref Rotation);
        }
        // ~INetworkSerializable
    }
}