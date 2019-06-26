using System.Collections;
using System.Collections.Generic;

using Steamworks;

using UnityEngine;

public class Cursors : MonoBehaviour {
    public GameObject LocalCursor;
    public List<GameObject> RemoteCursors;
    void Start() {
        LocalCursor = Instantiate(LocalCursor);
    }
    void Update() {
        LocalCursor.transform.position = Input.mousePosition;
    }
}