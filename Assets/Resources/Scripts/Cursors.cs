using System.Collections;
using System.Collections.Generic;

using ClickRaid.Models;

using Steamworks;

using UnityEngine;

public class Cursors {
    public Dictionary<CSteamID, PlayerCursor> PlayerCursors;
    void Update() {
        if (Lobbies.Active.IsLobby()) {
            for (var i = 0; i < SteamMatchmaking.GetNumLobbyMembers(Lobbies.Active); i++) {
                PlayerCursor cursor = PlayerCursor.Builder()
                    .SteamId(SteamMatchmaking.GetLobbyMemberByIndex(Lobbies.Active, i))
                    .X(float.Parse(SteamMatchmaking.GetLobbyMemberData(Lobbies.Active, SteamMatchmaking.GetLobbyMemberByIndex(Lobbies.Active, i), "x")))
                    .Y(float.Parse(SteamMatchmaking.GetLobbyMemberData(Lobbies.Active, SteamMatchmaking.GetLobbyMemberByIndex(Lobbies.Active, i), "y")))
                    .PersonaName(SteamMatchmaking.GetLobbyMemberData(Lobbies.Active, SteamMatchmaking.GetLobbyMemberByIndex(Lobbies.Active, i), "personaName"))
                    .Build();

                PlayerCursors[SteamMatchmaking.GetLobbyMemberByIndex(Lobbies.Active, i)] = cursor;
            }
        } else {
            // render local player only

        }
    }
}