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

            Button CreateRelayGameButton = homePanel.transform.GetChild(1).gameObject.GetComponent<Button>();
            Button JoinCodeGameButton = homePanel.transform.GetChild(2).gameObject.GetComponent<Button>();

            CreateRelayGameButton.onClick.AddListener(delegate
            {
              StartCoroutine(  Example_ConfigureTransportAndStartNgoAsHost());
                // HostGame(4);
                //  CreateRelay2();
            });

            JoinCodeGameButton.onClick.AddListener(delegate
            {
                StartCoroutine(Example_ConfigreTransportAndStartNgoAsConnectingPlayer(codeToRetrieve.text));
                //     JoinGame(codeToRetrieve.text);
                //     JoinCode2();
            });
        }



        async void Start()
        {

            await UnityServices.InitializeAsync();

            AuthenticationService.Instance.SignedIn += () =>
            {
                Debug.Log("TestRelay --> Signed in " + AuthenticationService.Instance.PlayerId);


            };

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        const int m_MaxConnections = 4;


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


        public  async Task<RelayServerData> JoinRelayServerFromJoinCode(string joinCode)
        {
            JoinAllocation allocation;
            try
            {
                allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            }
            catch
            {
                Debug.LogError("Relay create join code request failed");
                throw;
            }

            Debug.Log($"client: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
            Debug.Log($"host: {allocation.HostConnectionData[0]} {allocation.HostConnectionData[1]}");
            Debug.Log($"client: {allocation.AllocationId}");

            return new RelayServerData(allocation, "udp");
          //  return new RelayServerData(allocation, "dtls");
        }

        IEnumerator Example_ConfigreTransportAndStartNgoAsConnectingPlayer(string RelayJoinCode)
        {
            // Populate RelayJoinCode beforehand through the UI

            var clientRelayUtilityTask = JoinRelayServerFromJoinCode(RelayJoinCode.Substring(0,6));

            while (!clientRelayUtilityTask.IsCompleted)
            {
                yield return null;
            }

            if (clientRelayUtilityTask.IsFaulted)
            {
                Debug.LogError("Exception thrown when attempting to connect to Relay Server. Exception: " + clientRelayUtilityTask.Exception.Message);
                yield break;
            }

            var relayServerData = clientRelayUtilityTask.Result;

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
          
            yield return null;
        }

    }
}