using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine.SceneManagement;

public class TestLobby : MonoBehaviour
{


    private Lobby hostLobby;
    private Lobby joinLobby;
    private float hearthBeatTimer;
    private string randomPlayerName;

    public static TestLobby currentLobby;

    void Awake()
    {
        int numGameSessions = FindObjectsOfType<TestLobby>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        currentLobby = this;
    }

    async void Start()
    {
        randomPlayerName = "Gillo" + UnityEngine.Random.Range(10, 99);
        await UnityServices.InitializeAsync();


        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);


        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

    }

    private void Update()
    {
        HandleLobbyHearthBeat();
        HandleLobbPollForUpdates();
    }

    async void UpdatePlayerName(string newPlayerName)
    {
        try
        {
            randomPlayerName = newPlayerName;
            await LobbyService.Instance.UpdatePlayerAsync(joinLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
            {
                Data = new Dictionary<string, PlayerDataObject>{{
                         "PlayerName",
                          new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, randomPlayerName) }
                        }

            });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }

    }

    async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinLobby.Id, AuthenticationService.Instance.PlayerId);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    async void KickPlayerOnLobby(string playerToKick)
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinLobby.Id, playerToKick);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

   async void MigrateLocateHost()
    {
        try
        {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                HostId = joinLobby.Players[1].Id
            }) ;
            joinLobby = hostLobby;

            PrintPlayers(hostLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    void DeleteLobby()
    {
        LobbyService.Instance.DeleteLobbyAsync(joinLobby.Id);
    }

    Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>{{
                         "Player",
                          new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, randomPlayerName) }
                        }
        };
    }

    async void HandleLobbyHearthBeat()
    {
        if (hostLobby != null)
        {
            hearthBeatTimer -= Time.deltaTime;
            if (hearthBeatTimer < 0f)
            {
                float heartbeatTimeMax = 15f;
                hearthBeatTimer = heartbeatTimeMax;
                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }
    float lobbyUpdateTimer;
    async void HandleLobbPollForUpdates()
    {
        if (joinLobby != null)
        {
            lobbyUpdateTimer -= Time.deltaTime;
            if (lobbyUpdateTimer < 0f)
            {
                float lobbyUpdateTimerMax = 1.15f;
                lobbyUpdateTimer = lobbyUpdateTimerMax;
                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinLobby.Id);
                joinLobby = lobby;

            }
        }
    }



  public  async void CreateLobby()
    {
        try
        {
            string lobbyName = "MyLobby";

            int maxPlayer = 2;

            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject> { { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, "CaptureTheFlag") } }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer);

            hostLobby = lobby;
            joinLobby = hostLobby;

            Debug.Log("Create Lobby!!  " + lobby.Name + " " + lobby.MaxPlayers);

        //    PrintPlayers(hostLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }

    }

    public void JoinGame()
    {
        SceneManager.LoadScene("Game");
    }


    async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 25,
                Filters = new List<QueryFilter> {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT) },
                //   new QueryFilter(QueryFilter.FieldOptions.S1, "BridgeStrike", QueryFilter.OpOptions.EQ), per fare delle modalità
                Order = new List<QueryOrder>
                {
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            Debug.Log("lobbies found: " + queryResponse.Results.Count);

            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }


   public async void JoinLobby()
    {
        try
        {

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(queryResponse.Results[0].Id);
            joinLobby = lobby;
            PrintPlayers(lobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer()
            };

            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
            joinLobby = lobby;
            PrintPlayers(lobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }


  public  async void QuickJoinLobby()
    {

        try
        {
            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            joinLobby = lobby;
            PrintPlayers(lobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }

    }

    void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Lobby name: " + lobby.Name);
        foreach (Player player in lobby.Players)
        {
            Debug.Log(player.Id + "<<< Player in lobby");
        }
    }

    async void UpdateLobbyGameMode(string gameMode)
    {
        try
        {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject> { { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, gameMode) } }
            });
            joinLobby = hostLobby;

            PrintPlayers(hostLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }

    }


}
