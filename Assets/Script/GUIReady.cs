using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIReady : MonoBehaviour {

    public int frame;

    void Start() {
        StartCoroutine(Example());
    }

    IEnumerator Example() {
        Debug.Log("Waiting for princess to be rescued...");
        yield return new WaitUntil(() => frame >= 10);
        Debug.Log("Princess was rescued!");
    }

    void Update() {
        if(frame <= 10) {
            Debug.Log("Frame: " + frame);
            frame++;
        }
    }

    public bool CountDown() {
        StartCoroutine(Example());
        return true;
    }
}
