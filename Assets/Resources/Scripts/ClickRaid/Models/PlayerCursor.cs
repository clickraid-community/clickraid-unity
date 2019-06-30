using Steamworks;
using UnityEngine;

namespace ClickRaid.Models {
    public class PlayerCursor : MonoBehaviour {
        CSteamID steamId;
        string personaName;
        public static PlayerCursorBuilder Builder() {
            return new PlayerCursorBuilder();
        }
        private PlayerCursor() {}
        public class PlayerCursorBuilder {
            PlayerCursor playerCursor;
            public PlayerCursor Build() {
                return playerCursor;
            }
            public PlayerCursorBuilder() {
                playerCursor = new PlayerCursor();
            }
            public PlayerCursorBuilder X(float x) {
                playerCursor.transform.position = new Vector2(x, playerCursor.transform.position.y);
                return this;
            }
            public PlayerCursorBuilder Y(float y) {
                playerCursor.transform.position = new Vector2(playerCursor.transform.position.x, y);
                return this;
            }
            public PlayerCursorBuilder Position(Vector3 position) {
                playerCursor.transform.position = position;
                return this;
            }
            public PlayerCursorBuilder SteamId(CSteamID steamId) {
                playerCursor.steamId = steamId;
                return this;
            }
            public PlayerCursorBuilder PersonaName(string name) {
                playerCursor.personaName = name;
                return this;
            }
        }
    }
}