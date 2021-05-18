using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DrawClimber : MonoBehaviour {
    
    public Player player;
    public float finishLine;
    public Shader shader;
    public float vertexPrecision;
    public GameObject leftL, rightL;
    public GameObject drawObj;

    // Start is called before the first frame update
    void Awake () {
        player.Rigidbody = player.GetComponent<Rigidbody>();
        player.Animator = player.GetComponent<Animator>();
        player.Rigidbody.useGravity = false;
        player.Shader = shader;
        player.VertexPrecision = vertexPrecision;
        player.LeftL = leftL;
        player.RightL = rightL;
        player.DrawObject = drawObj;
    }

    // Update is called once per frame
    void Update() {
        if(player.Run) {
            player.Rigidbody.useGravity = true;
            if(player.IsGrounded)
                player.Rigidbody.velocity = transform.TransformVector(player.Speed, 0, 0);
        } else {
            player.Rigidbody.useGravity = false;
            player.Rigidbody.velocity = transform.TransformVector(0, 0, 0);
        }
        if(player.transform.position.x == finishLine) {
            player.Rigidbody.useGravity = false;
            player.Rigidbody.velocity = transform.TransformVector(0, 0, 0);
            player.Run = false;
        }
        
        
    }
}
