
using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class GameNetworkManager : NetworkBehaviour
    {
        public static int playerLogged;
  [SerializeField]      static GameObject gameManager;
        public override void OnNetworkSpawn()
        {
            gameManager = GameObject.Find("Managers/GameManager");
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();

                SubmitNewPosition();
            }

            GUILayout.EndArea();
        }

        static void StartButtons()
        {
            if (GUILayout.Button("Host"))
            {
                NetworkManager.Singleton.StartHost();
         //       gameManager.GetComponent<GameManager>().SetPlayerIDServerRpc();
            }
            if (GUILayout.Button("Client"))
            {
                NetworkManager.Singleton.StartClient();
                gameManager.GetComponent<GameManager>().SetPlayerID();
            }

            if (GUILayout.Button("Server")) {
                NetworkManager.Singleton.StartServer();
                gameManager.GetComponent<GameManager>().SetPlayerID();
            }
        }

        //[ServerRpc]
        //private static void LogClient()
        //{
        //    playerLogged++;
        //    if (playerLogged < 3)
        //    {
        //        Debug.Log(" playerLogged.Value: " + playerLogged);
        //        NetworkManager.Singleton.StartClient();

        //    }
        //    else
        //    {
        //        Debug.Log("Too much client");
        //    }
        //}

        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        static void SubmitNewPosition()
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Pass Turn"))
            {
                if (NetworkManager.Singleton.IsServer && !gameManager.GetComponent<GameManager>().IsRunningPlayer())
                {

                    Debug.Log("se è server e nn cliente fa questo");     //se è server e nn cliente fa questo
                }
                else
                {
                    gameManager.GetComponent<GameManager>().EndTurn();
                    Debug.Log("giocatore corrente: " + gameManager.GetComponent<GameManager>().CurrentTurn.Value);
                    //se è cliente  e nn server fa questo
                }
            }
        }

    }
}