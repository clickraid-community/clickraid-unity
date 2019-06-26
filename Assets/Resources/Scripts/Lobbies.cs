using System.Collections;
using System.Collections.Generic;

using Steamworks;

using UnityEngine;

public class Lobbies : MonoBehaviour {
    private CallResult<LobbyMatchList_t> RequestLobbyListCallResult;
    private CallResult<LobbyCreated_t> CreateLobbyCallResult;
    private CSteamID _activeLobbyId;
    void OnEnable() {
        if (SteamManager.Initialized) {
            RequestLobbyListCallResult = CallResult<LobbyMatchList_t>.Create(OnLobbyListReceived);
            CreateLobbyCallResult = CallResult<LobbyCreated_t>.Create(OnLobbyCreated);

            RequestLobbyListCallResult.Set(SteamMatchmaking.RequestLobbyList());
        }
    }
    void Start() {
        Debug.Log(_activeLobbyId.IsLobby());
    }
    void Update() {
        // @TODO throttle
        if (_activeLobbyId.IsLobby()) {
            SteamMatchmaking.SetLobbyMemberData(_activeLobbyId, "x", Input.mousePosition.x.ToString());
            SteamMatchmaking.SetLobbyMemberData(_activeLobbyId, "y", Input.mousePosition.y.ToString());
            SteamMatchmaking.SetLobbyMemberData(_activeLobbyId, "personaName", SteamFriends.GetPersonaName());
        }
    }
    void OnGUI() {
        GUILayout.Label("Lobby Status: " + ((_activeLobbyId.IsLobby()) ? "Connected!" : "Solo X("));
    }
    void OnLobbyListReceived(LobbyMatchList_t pCallback, bool bIOFailure) {
        if (!bIOFailure) {
            if (pCallback.m_nLobbiesMatching == 0) {
                CreateLobbyCallResult.Set(SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 250));
            } else {
                SteamMatchmaking.JoinLobby(SteamMatchmaking.GetLobbyByIndex(0));
            }
        }
    }
    void OnLobbyCreated(LobbyCreated_t pCallback, bool bIOFailure) {
        if (!bIOFailure) {
            _activeLobbyId = (CSteamID) pCallback.m_ulSteamIDLobby;
        }
    }
}