using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace ClickRaid.Cursors
{
    public class ApplySprite : MonoBehaviour
    {
        public void OnMouseUpAsButton()
        {
            Image image = GetComponent<Image>();

            Cursor.SetCursor(image.sprite.texture, image.sprite.pivot, CursorMode.Auto);

            Debug.Log("mouse clicked");
        }

        public void SetCursor(Image image)
        {
            Cursor.SetCursor(image.sprite.texture, image.sprite.pivot, CursorMode.Auto);
        }
    }
}