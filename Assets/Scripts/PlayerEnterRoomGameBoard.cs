using System;
using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class PlayerEnterRoomGameBoard : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                Debug.Log("Giocatore server loggato " + NetworkManager.Singleton.ConnectedClients.Count);
                if (NetworkManager.Singleton.ConnectedClients.Count == 1)
                {
                    Debug.Log("connessi giocatori: " + NetworkManager.Singleton.ConnectedClients.Count);
                    Position.Value = new Vector3(7f, -3f, -1f);
                    transform.position = Position.Value;
                }else
                if (NetworkManager.Singleton.ConnectedClients.Count == 2)
                {
                    Debug.Log("connessi giocatori: " + NetworkManager.Singleton.ConnectedClients.Count);
                    Position.Value = new Vector3(-7f, -3f, -1f);
                    transform.position = Position.Value;
                }
                Debug.Log("NetworkManager.Singleton.LocalClientId: " + NetworkManager.Singleton.LocalClientId);
            }
    
        }

    

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {

        }

        [ClientRpc] //il giocatore è questo client. QUando fa qualsiasi cosa, chiama il ServerRpc e fa quella cosa. Chiaramente deve controllare se può funzionare, ad esempiop spostare un oggetto 
        void TestClientRpc(int value)
        {
            if (IsClient)
            {
                Debug.Log("Client Received the RPC #" + value);
                //spostaPedinaServer
            }
        }



    }
}