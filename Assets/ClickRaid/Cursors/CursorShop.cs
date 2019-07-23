using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClickRaid
{
    public class CursorShop : MonoBehaviour
    {
        [SerializeField]
        private GameObject CursorSprite;

        [SerializeField]
        private List<Sprite> CursorSprites;

        void Start()
        {
            GameObject cursor;

            foreach (Sprite sprite in CursorSprites)
            {
                cursor = Instantiate(CursorSprite, transform);
                cursor.GetComponent<Image>().sprite = sprite;
                Mannequins.Add(cursor);
            }
        }

        private List<GameObject> Mannequins = new List<GameObject>();
    }
}