
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace HelloWorld
{
    public class GameNetworkManager : NetworkBehaviour
    {
        public static int playerLogged;
        static GameObject gameManager;
        public override void OnNetworkSpawn()
        {
            gameManager = GameObject.Find("Managers/GameManager");



        }

        async void Start()
        {

            await UnityServices.InitializeAsync();

            AuthenticationService.Instance.SignedIn += () =>
            {
                Debug.Log("Player logged with ID: " + AuthenticationService.Instance.PlayerId);


            };

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
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
           //     gameManager.GetComponent<GameManager>().SetPlayerID();
            }
            if (GUILayout.Button("Client"))
            {
                NetworkManager.Singleton.StartClient();
     //           gameManager.GetComponent<GameManager>().SetPlayerIDServerRpc();
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
                if (NetworkManager.Singleton.IsServer && !gameManager.GetComponent<GameManager>().IsRunningPlayer())
                {

                    Debug.Log("se ? server e nn cliente fa questo");     //se ? server e nn cliente fa questo
                }
                else
                {
                  //  gameManager.GetComponent<GameManager>().EndTurn();
                    Debug.Log("giocatore corrente: " + gameManager.GetComponent<GameManager>().CurrentTurn.Value);
                    //se ? cliente  e nn server fa questo
                }
            }
        }

    }
}