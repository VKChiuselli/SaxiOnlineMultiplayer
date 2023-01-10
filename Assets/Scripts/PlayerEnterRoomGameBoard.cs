using System;
using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class PlayerEnterRoomGameBoard : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();


        GameObject PanelPlayerRight;
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

                Debug.Log("unavolta");

                LoadCoreGame();
                LoadCards();
            }
    
        }
        NetworkObject CoreGameToSpawnNetwork;
        private void LoadCoreGame()
        {
            GameObject CoreGame = Resources.Load("PrefabToLoad\\Core\\CoreGame", typeof(GameObject)) as GameObject;

            CoreGameToSpawnNetwork = Instantiate(CoreGame.GetComponent<NetworkObject>());
        }

        public void LoadCards()
        {
            if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
            {
                LoadCardsLocals();
             
                Debug.Log("LoadCards method is server loading");

            }
            else if (NetworkManager.Singleton.IsClient)
            {
                LoadCardsServerRpca();
            }
            else
            {
                Debug.Log("LoadCards method is broken");
            }

        }

        private void LoadCardsServerRpca()
        {

            Debug.Log("LoadCardsServerRpca");
        }
   
        private NetworkManager netti;
        private GameObject deckManagerRight;
        GameObject cardToSpawn;
        private void LoadCardsLocals()
        {

            GameObject cardToSpawn = Resources.Load("PrefabToLoad\\Cards\\Dog", typeof(GameObject)) as GameObject;
     


            NetworkObject cardToSpawnNetwork = Instantiate(cardToSpawn.GetComponent<NetworkObject>());
            //     cardToSpawnNetwork.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
            //    netti = FindObjectOfType<NetworkManager>();
            //    netti.AddNetworkPrefab(cardToSpawn);
              cardToSpawnNetwork.transform.SetParent(CoreGameToSpawnNetwork.transform, false);
    //        deckManagerRight = GameObject.Find("CanvasHandPlayer/PanelPlayerRight");
          //  deckManagerRight.GetComponent<DeckLoad>().TriggerLoadDeck(cardToSpawn);
            Debug.Log("LoadCardsLocals");
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