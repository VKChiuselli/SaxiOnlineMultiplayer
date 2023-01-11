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
            if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
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

                Debug.Log("sono server");
            }
            else
            {
                Debug.Log("sono client");
                //GameObject IsCoreGameSpawned = GameObject.Find($"CoreGame(Clone)");
                //if (IsCoreGameSpawned == null)
                //{
                //    LoadCoreGame();
                //    LoadCards("PanelPlayerRight", "RPCH");
                //}
                // LoadCards("PanelPlayerLeft", "LPCH");
       //   LoadCards("PanelPlayerRight", "RPCH");
            }
        }
        NetworkObject CoreGameToSpawnNetwork;
        private void LoadCoreGame()
        {
            GameObject CoreGame = Resources.Load("PrefabToLoad\\Core\\CoreGame", typeof(GameObject)) as GameObject;

            CoreGameToSpawnNetwork = Instantiate(CoreGame.GetComponent<NetworkObject>());
        }

        public void LoadCards(string panelName, string tagName)
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
             GameObject serverHand = GameObject.Find($"CoreGame/CanvasHandPlayer/{panelName}/Dog(Clone)");
            if (serverHand == null)
            {
            LoadCardsServerRpc(panelName, tagName);
            }


        }
      
 
    
     
        [ServerRpc(RequireOwnership = false)]
        public void LoadCardsServerRpc(string panelName, string tagName)
        {

            GameObject cardToSpawn = Resources.Load("PrefabToLoad\\Cards\\Dog", typeof(GameObject)) as GameObject;
            //GameObject serverHand = GameObject.Find($"CoreGame(Clone)/CanvasHandPlayer/{panelName}");
            GameObject serverHand = GameObject.Find($"CoreGame/CanvasHandPlayer/{panelName}");
            NetworkObject cardToSpawnNetwork = Instantiate(cardToSpawn.GetComponent<NetworkObject>(), serverHand.transform);
            cardToSpawnNetwork.GetComponent<NetworkObject>().tag = tagName;
            serverHand.GetComponent<DeckLoad>().LoadCards();
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {

        }

      



    }
}