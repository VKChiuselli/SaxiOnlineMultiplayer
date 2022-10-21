
using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class GameNetworkManager : MonoBehaviour
    {
        public static int playerLogged;
        static GameObject gameManager;
         void Start()
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
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client"))
            {
                NetworkManager.Singleton.StartClient();
            }

            if (GUILayout.Button("Server")) { 
                NetworkManager.Singleton.StartServer();
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
                if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
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

        [ServerRpc]
        public void LoggingClientCount()
        {
            playerLogged++;
        }

    }
}