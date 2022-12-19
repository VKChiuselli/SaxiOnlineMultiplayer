using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Lobby
{
    public class RelayTesting : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI codeToShow;
        [SerializeField] TextMeshProUGUI codeToRetrieve;
        GameObject homePanel;
        void Awake()
        {
            GameObject canvas = GameObject.Find("/Relay/Canvas");
            homePanel = canvas.transform.GetChild(0).gameObject;


            Button CreateRelayGameButton = homePanel.transform.GetChild(0).gameObject.GetComponent<Button>();
            Button JoinCodeGameButton = homePanel.transform.GetChild(1).gameObject.GetComponent<Button>();

            CreateRelayGameButton.onClick.AddListener(delegate
            {
              StartCoroutine(  Example_ConfigureTransportAndStartNgoAsHost());
                // HostGame(4);
                //  CreateRelay2();
            });

            JoinCodeGameButton.onClick.AddListener(delegate
            {
                //     JoinGame(codeToRetrieve.text);
           //     JoinCode2();
            });
            //   Example_ConfigureTransportAndStartNgoAsHost();
        }



        async void Start()
        {
            //   gameManager = GameObject.Find("Managers/GameManager");

            await UnityServices.InitializeAsync();

            AuthenticationService.Instance.SignedIn += () =>
            {
                Debug.Log("TestRelay --> Signed in " + AuthenticationService.Instance.PlayerId);


            };

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        const int m_MaxConnections = 4;

        public string RelayJoinCode;

        public  async Task<RelayServerData> AllocateRelayServerAndGetJoinCode(int maxConnections, string region = null)
        {
            Allocation allocation;
            string createJoinCode;
            try
            {
                allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections, region);
            }
            catch (Exception e)
            {
                Debug.LogError($"Relay create allocation request failed {e.Message}");
                throw;
            }

            Debug.Log($"server: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
            Debug.Log($"server: {allocation.AllocationId}");

            try
            {
                createJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            }
            catch
            {
                Debug.LogError("Relay create join code request failed");
                throw;
            }


            RelayServerEndpoint ServerEndpoint22 = null;

            foreach (RelayServerEndpoint ServerEndpoint in allocation.ServerEndpoints)
            {
                if (ServerEndpoint.ConnectionType == "udp")
                {
                    ServerEndpoint22 = ServerEndpoint;
                }
            }
            codeToRetrieve.text = $"joincode: {createJoinCode}";

            return new RelayServerData(allocation, "udp");
        }

   public     IEnumerator Example_ConfigureTransportAndStartNgoAsHost()
        {
          var serverRelayUtilityTask = AllocateRelayServerAndGetJoinCode(m_MaxConnections);
            while (!serverRelayUtilityTask.IsCompleted)
            {
                yield return null;
            }
            if (serverRelayUtilityTask.IsFaulted)
            {
                Debug.LogError("Exception thrown when attempting to start Relay Server. Server not started. Exception: " + serverRelayUtilityTask.Exception.Message);
                yield break;
            }

            var relayServerData = serverRelayUtilityTask.Result;

            // Display the joinCode to the user.

            codeToShow.text = $"host:  {relayServerData.HostConnectionData}:{relayServerData.Endpoint}\n ";


            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
            yield return null;
        }


    }
}