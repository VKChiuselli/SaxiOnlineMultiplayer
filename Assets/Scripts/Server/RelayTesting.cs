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

            CreateRelayGameButton.onClick.AddListener(delegate {
                // HostGame(4);
                CreateRelay2();
            });

            JoinCodeGameButton.onClick.AddListener(delegate {
                //     JoinGame(codeToRetrieve.text);
                JoinCode2();
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


        public async void CreateRelay2()
        {
            try
            {

                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2);

                string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
                Debug.Log("TestRelay --> join code " + joinCode);
                codeToShow.text = joinCode;
            //    NetworkManager.Singleton.StartHost();
                SetPlayerID();
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e.Message);
            }

        }

        public async void JoinCode2()
        {
            try
            {
                string joinCode = codeToRetrieve.text;
                Debug.Log("joined with this code : " + joinCode);
                joinCode = joinCode.Substring(0, 6);
                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

                RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

                //   NetworkManager.Singleton.StartClient();
                SetPlayerID();
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e.Message);
            }

        }

        public async void CreateRelay()
        {
            try
            {


                string joinCode = await AllocateRelayServerAndGetJoinCode(m_MaxConnections);

                Debug.Log("TestRelay --> join code " + joinCode);
                codeToShow.text = joinCode;
                //       NetworkManager.Singleton.StartClient();
                //   NetworkManager.Singleton.StartHost();
                SetPlayerID();



            }
            catch (RelayServiceException e)
            {
                Debug.Log(e.Message);
            }

        }

        public async void JoinRelayCode()
        {
            try
            {
                string joinCode = codeToRetrieve.text;
                joinCode = joinCode.Substring(0, 6);
                Debug.Log("joined with this code : " + joinCode);
                // JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

                RelayServerData relayServerData = await JoinRelayServerFromJoinCode(joinCode);

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
                //    NetworkManager.Singleton.StartClient();
                codeToShow.text = joinCode;
                SetPlayerID();
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e.Message);
            }

        }
        GameObject gameManager;
        public void SetPlayerID()
        {
            //       gameManager.GetComponent<GameManager>().SetPlayerID();
        }

        const int m_MaxConnections = 4;

        public string RelayJoinCode;

        //Create an allocation and request a join code
        public static async Task<string> AllocateRelayServerAndGetJoinCode(int maxConnections, string region = null)
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
                Debug.Log($"server: createJoinCode THE REAL " + createJoinCode);
            }
            catch
            {
                Debug.LogError("Relay create join code request failed");
                throw;
            }
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
            return createJoinCode;
        }

        //Configure the transport and start NGO


        //Join an allocation
        public static async Task<RelayServerData> JoinRelayServerFromJoinCode(string joinCode)
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

            return new RelayServerData(allocation, "dtls");
        }

        /// <summary>
        /// RelayHostData represents the necessary information
        /// for a Host to host a game on a Relay
        /// </summary>
        public struct RelayHostData
        {
            public string JoinCode;
            public string IPv4Address;
            public ushort Port;
            public Guid AllocationID;
            public byte[] AllocationIDBytes;
            public byte[] ConnectionData;
            public byte[] Key;
        }

        /// <summary>
        /// HostGame allocates a Relay server and returns needed data to host the game
        /// </summary>
        /// <param name="maxConn">The maximum number of peer connections the host will allow</param>
        /// <returns>A Task returning the needed hosting data</returns>
        public static async Task<RelayHostData> HostGame(int maxConn)
        {
            //Initialize the Unity Services engine
            await UnityServices.InitializeAsync();
            //Always autheticate your users beforehand
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                //If not already logged, log the user in
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            //Ask Unity Services to allocate a Relay server
            Allocation allocation = await Unity.Services.Relay.RelayService.Instance.CreateAllocationAsync(maxConn);

            RelayServerEndpoint ServerEndpoint22 = null;
            foreach (RelayServerEndpoint ServerEndpoint in allocation.ServerEndpoints)
            {
                if (ServerEndpoint.ConnectionType == "dtls")
                {
                    ServerEndpoint22 = ServerEndpoint;

                }
            }
            //Populate the hosting data
            RelayHostData data = new RelayHostData
            {
                // WARNING allocation.RelayServer is deprecated
                IPv4Address = ServerEndpoint22.Host,
                Port = (ushort)allocation.RelayServer.Port,

                AllocationID = allocation.AllocationId,
                AllocationIDBytes = allocation.AllocationIdBytes,
                ConnectionData = allocation.ConnectionData,
                Key = allocation.Key,
            };

            //Retrieve the Relay join code for our clients to join our party
            data.JoinCode = await Unity.Services.Relay.RelayService.Instance.GetJoinCodeAsync(data.AllocationID);
            //      codeToShow.text = data.JoinCode;
            Debug.Log("REAL 99999:    " + data.JoinCode);
            return data;
        }

        /// <summary>
        /// RelayHostData represents the necessary information
        /// for a Host to host a game on a Relay
        /// </summary>
        public struct RelayJoinData
        {
            public string IPv4Address;
            public ushort Port;
            public Guid AllocationID;
            public byte[] AllocationIDBytes;
            public byte[] ConnectionData;
            public byte[] HostConnectionData;
            public byte[] Key;
        }

        /// <summary>
        /// Join a Relay server based on the JoinCode received from the Host or Server
        /// </summary>
        /// <param name="joinCode">The join code generated on the host or server</param>
        /// <returns>All the necessary data to connect</returns>
        public static async Task<RelayJoinData> JoinGame(string joinCode)
        {
            //Initialize the Unity Services engine
            await UnityServices.InitializeAsync();
            //Always authenticate your users beforehand
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                //If not already logged, log the user in
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            //Ask Unity Services for allocation data based on a join code
            JoinAllocation allocation = await Unity.Services.Relay.RelayService.Instance.JoinAllocationAsync(joinCode);
            RelayServerEndpoint ServerEndpoint22 = null;
            foreach (RelayServerEndpoint ServerEndpoint in allocation.ServerEndpoints)
            {
                if (ServerEndpoint.ConnectionType == "dtls")
                {
                    ServerEndpoint22 = ServerEndpoint;

                }
            }

            //Populate the joining data
            RelayJoinData data = new RelayJoinData
            {
                // WARNING allocation.RelayServer is deprecated. It's best to read from ServerEndpoints.
                IPv4Address = ServerEndpoint22.Host,
                Port = (ushort)allocation.RelayServer.Port,

                AllocationID = allocation.AllocationId,
                AllocationIDBytes = allocation.AllocationIdBytes,
                ConnectionData = allocation.ConnectionData,
                HostConnectionData = allocation.HostConnectionData,
                Key = allocation.Key,
            };
            return data;
        }

    }
}