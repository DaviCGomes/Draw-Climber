using UnityEngine;

public class Rotation : MonoBehaviour {
    public Vector3 Angle {
        get;
        set;
    }
    public float Speed {
        get;
        set;
    }
    public bool Rotate {
        get;
        set;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if(Rotate) 
            transform.Rotate(Angle * Speed * Time.deltaTime);
    }
}
