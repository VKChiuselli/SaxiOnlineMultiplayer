
using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class GameNetworkManager : MonoBehaviour
    {
        public static int playerLogged;

        //private void Start()
        //{
        //        playerLogged.Value = 0;
        //        Debug.Log(" playerLogged.Value: " + playerLogged.Value);
        //        playerLogged.Value++;
        //        Debug.Log(" playerLogged.Value: " +  playerLogged.Value);
        //}

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            //else
            //{
            //    StatusLabels();

            //    SubmitNewPosition();
            //}

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
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
            {
                if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
                {

                    Debug.Log("se è server e nn cliente fa questo");     //se è server e nn cliente fa questo
                }
                else
                {
                    Debug.Log("se è cliente  e nn server fa questo");
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