using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamScript : MonoBehaviour
{
    private CallResult<NumberOfCurrentPlayers_t> m_NumberOfCurrentPlayers;
    private CallResult<LobbyMatchList_t> OnLobbyMatchListCallResult;

    private CallResult<LobbyCreated_t> OnLobbyCreatedCallResult;

    private CallResult<LobbyEnter_t> OnLobbyEnterCallResult;

    private CallResult<LobbyDataUpdate_t> OnLobbyDataUpdateCallResult;

    private uint numLobbies;

    private int numLobbyPlayers;

    private CSteamID lobbyId;

    private List<CSteamID> lobbyMates = new List<CSteamID>();

    // Start is called before the first frame update
    void Start()
    {
        if (SteamManager.Initialized)
        {
            string name = SteamFriends.GetPersonaName();
            Debug.Log(name);

            uint purchase_time = SteamApps.GetEarliestPurchaseUnixTime(new AppId_t(658160));
            Debug.Log(purchase_time);
        }
    }

    protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;

    private void OnEnable()
    {
        if (SteamManager.Initialized)
        {
            m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
            m_NumberOfCurrentPlayers = CallResult<NumberOfCurrentPlayers_t>.Create(OnNumberOfCurrentPlayers);
            OnLobbyMatchListCallResult = CallResult<LobbyMatchList_t>.Create(OnLobbyMatchList);
            OnLobbyCreatedCallResult = CallResult<LobbyCreated_t>.Create(OnLobbyCreated);
            OnLobbyEnterCallResult = CallResult<LobbyEnter_t>.Create(OnLobbyEnter);
            OnLobbyDataUpdateCallResult = CallResult<LobbyDataUpdate_t>.Create(OnLobbyDataUpdate);
        }
    }

    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    {
        if (pCallback.m_bActive != 0)
        {
            Debug.Log("Steam Overlay has been activated");
        }
        else
        {
            Debug.Log("Steam Overlay has been closed");
        }
    }

    private void OnNumberOfCurrentPlayers(NumberOfCurrentPlayers_t pCallback, bool bIOFailure)
    {
        if (pCallback.m_bSuccess != 1 || bIOFailure)
        {
            Debug.Log("There was an error retrieving the NumberOfCurrentPlayers.");
        }
        else
        {
            Debug.Log("The number of players playing your game: " + pCallback.m_cPlayers);
        }
    }


    void OnLobbyMatchList(LobbyMatchList_t pCallback, bool bIOFailure)
    {
        Debug.Log("[" + LobbyMatchList_t.k_iCallback + " - LobbyMatchList] - " + pCallback.m_nLobbiesMatching);
        numLobbies = pCallback.m_nLobbiesMatching;

        if (numLobbies == 0) {
            SteamAPICall_t handle = SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 5);
            OnLobbyCreatedCallResult.Set(handle);
            print("SteamMatchmaking.CreateLobby() : " + handle);   
         } else {
            lobbyId = SteamMatchmaking.GetLobbyByIndex(0);            
            SteamAPICall_t handle = SteamMatchmaking.JoinLobby(lobbyId);
            OnLobbyEnterCallResult.Set(handle);
            // OnLobbyDataUpdateCallResult.Set(handle);
            print("SteamMatchmaking.JoinLobby() : " + handle);
        }
    }

    private void OnLobbyDataUpdate(LobbyDataUpdate_t pCallback, bool bIOFailure) {
        Debug.Log("[" + LobbyDataUpdate_t.k_iCallback + " - Lobby Data Updated] - " + pCallback.m_ulSteamIDLobby + " - " + pCallback.m_ulSteamIDMember);
        lobbyId = (CSteamID) pCallback.m_ulSteamIDLobby;
      
        numLobbyPlayers = SteamMatchmaking.GetNumLobbyMembers((CSteamID) pCallback.m_ulSteamIDLobby);
        Debug.Log("num players: " + numLobbyPlayers);

        lobbyMates = new List<CSteamID>();

        for (int i = 0; i < numLobbyPlayers; i++) {
            CSteamID mateId = SteamMatchmaking.GetLobbyMemberByIndex(lobbyId, i);

            lobbyMates.Add(mateId);
        }
    }

    private void OnLobbyCreated(LobbyCreated_t pCallback, bool bIOFailure) {
        Debug.Log("[" + LobbyCreated_t.k_iCallback + " - LobbyCreated] - " + pCallback.m_ulSteamIDLobby);
        lobbyId = (CSteamID) pCallback.m_ulSteamIDLobby;

        numLobbyPlayers = SteamMatchmaking.GetNumLobbyMembers((CSteamID) pCallback.m_ulSteamIDLobby);
        Debug.Log("num players: " + numLobbyPlayers);

        lobbyMates = new List<CSteamID>();

        for (int i = 0; i < numLobbyPlayers; i++) {
            CSteamID mateId = SteamMatchmaking.GetLobbyMemberByIndex(lobbyId, i);

            lobbyMates.Add(mateId);
        }
    
    }

    private void OnLobbyEnter(LobbyEnter_t pCallback, bool bIOFailure) {
        Debug.Log("[" + LobbyEnter_t.k_iCallback + " - LobbyCreated] - " + pCallback.m_ulSteamIDLobby);
        lobbyId = (CSteamID) pCallback.m_ulSteamIDLobby;

        numLobbyPlayers = SteamMatchmaking.GetNumLobbyMembers(lobbyId);
        Debug.Log("num players: " + numLobbyPlayers);

        lobbyMates = new List<CSteamID>();

        for (int i = 0; i < numLobbyPlayers; i++) {
            CSteamID mateId = SteamMatchmaking.GetLobbyMemberByIndex(lobbyId, i);

            lobbyMates.Add(mateId);
        }
    }

    void OnGUI() {
        GUILayout.Label("lobby count: " + numLobbies);
        GUILayout.Label("lobby id: " + lobbyId);
        GUILayout.Label("lobby population: " + numLobbyPlayers);

        foreach (CSteamID mateId in lobbyMates) {
            float mate_x = float.Parse(SteamMatchmaking.GetLobbyMemberData(lobbyId, mateId, "x"), System.Globalization.CultureInfo.InvariantCulture);
            float mate_y = float.Parse(SteamMatchmaking.GetLobbyMemberData(lobbyId, mateId, "y"), System.Globalization.CultureInfo.InvariantCulture);
            GUILayout.Label("lobby mate cursor x: " + mateId.m_SteamID + " " + mate_x);
            GUILayout.Label("lobby mate cursor y: " + mate_y);
            GUILayout.Label("persona name: " + SteamMatchmaking.GetLobbyMemberData(lobbyId, mateId, "personaName"));
            Rect rect = GUILayoutUtility.GetLastRect();
            rect.x = mate_x;
            rect.y = mate_y;
            GUI.Label(rect, mateId + " " + SteamMatchmaking.GetLobbyMemberData(lobbyId, mateId, "personaName"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //SteamAPICall_t handle = SteamUserStats.GetNumberOfCurrentPlayers();
            //m_NumberOfCurrentPlayers.Set(handle);
            //Debug.Log("Called GetNumberOfCurrentPlayers()");

            SteamAPICall_t handle = SteamMatchmaking.RequestLobbyList();
            OnLobbyMatchListCallResult.Set(handle);
            print("SteamMatchmaking.RequestLobbyList() : " + handle);
        }

        SteamMatchmaking.SetLobbyMemberData(lobbyId, "x", Input.mousePosition.x.ToString());
        SteamMatchmaking.SetLobbyMemberData(lobbyId, "y", Input.mousePosition.y.ToString());
        SteamMatchmaking.SetLobbyMemberData(lobbyId, "personaName", SteamFriends.GetPersonaName());

        // foreach (CSteamID mateId in lobbyMates) {
        //     float mate_x = float.Parse(SteamMatchmaking.GetLobbyMemberData(lobbyId, mateId, "x"), System.Globalization.CultureInfo.InvariantCulture);
        //     float mate_y = float.Parse(SteamMatchmaking.GetLobbyMemberData(lobbyId, mateId, "y"), System.Globalization.CultureInfo.InvariantCulture);
        

        // }
    }
}