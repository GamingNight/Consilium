using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMouseDetector : MonoBehaviour {

    private bool mouseIsOver;
    public bool MouseIsOver { get { return mouseIsOver; } }

    void Start() {
        mouseIsOver = false;
    }

    private void OnMouseOver() {
        mouseIsOver = true;
    }

    private void OnMouseExit() {
        mouseIsOver = false;
    }

    private void OnDisable() {

        mouseIsOver = false;
    }
}
