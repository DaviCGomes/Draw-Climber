using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawClimber : MonoBehaviour {
    //private bool runStart;
    public Player player;
    
    // Start is called before the first frame update
    void Start() {
        player.Rigidbody = player.GetComponent<Rigidbody>();
        player.Rigidbody.useGravity = false;
        //runStart = false;
        //if(!player.Run)
            //StartCoroutine(WaitFirstDraw());
    }

    IEnumerator WaitFirstDraw() {
        yield return new WaitUntil(() => player.FirstDraw);
    }

    // Update is called once per frame
    void Update() {
        if(player.Run) {
            player.Rigidbody.useGravity = true;
            player.Rigidbody.velocity = transform.TransformVector(player.Speed,0,0) ;
        }
        
        
    }
}
