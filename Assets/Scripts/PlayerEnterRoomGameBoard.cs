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
            }
                LoadCards();
        }
        NetworkObject CoreGameToSpawnNetwork;
        private void LoadCoreGame()
        {
            GameObject CoreGame = Resources.Load("PrefabToLoad\\Core\\CoreGame", typeof(GameObject)) as GameObject;

            CoreGameToSpawnNetwork = Instantiate(CoreGame.GetComponent<NetworkObject>());
        }

        public void LoadCards()
        {
            //if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
            //{
             
            //    Debug.Log("LoadCards method is server loading");

            //}
            //else if (NetworkManager.Singleton.IsClient)
            //{
            //    LoadCardsServerRpca();
            //}
            //else
            //{
            //    Debug.Log("LoadCards method is broken");
            //}
            LoadCardsServerRpc();


        }
 
    
     
        [ServerRpc(RequireOwnership = false)]
        public void LoadCardsServerRpc()
        {

            GameObject cardToSpawn = Resources.Load("PrefabToLoad\\Cards\\Dog", typeof(GameObject)) as GameObject;
            Debug.Log("CoreGameToSpawnNetwork.transform" + CoreGameToSpawnNetwork.name);
            GameObject serverHand = GameObject.Find("CoreGame(Clone)/CanvasHandPlayer/PanelPlayerRight");
            NetworkObject cardToSpawnNetwork = Instantiate(cardToSpawn.GetComponent<NetworkObject>(), serverHand.transform);

            cardToSpawnNetwork.GetComponent<NetworkObject>().tag = "RPCH";
            //  cardToSpawnNetwork.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
            //       cardToSpawnNetwork.transform.SetParent(CoreGameToSpawnNetwork.transform, false);
            //  netti = FindObjectOfType<NetworkManager>();
            //    netti.AddNetworkPrefab(cardToSpawn);
            //        deckManagerRight = GameObject.Find("CanvasHandPlayer/PanelPlayerRight");
            //  deckManagerRight.GetComponent<DeckLoad>().TriggerLoadDeck(cardToSpawn);
            Debug.Log("LoadCardsLocals n== " + cardToSpawn.transform.childCount);

            serverHand.GetComponent<DeckLoad>().LoadCards();
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {

        }

      



    }
}