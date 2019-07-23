using UnityEngine;

namespace ClickRaid
{
    [CreateAssetMenu]
    [System.Serializable]
    public class CursorSprite : ScriptableObject
    {
        [SerializeField]
        private Sprite Sprite;
    }
}