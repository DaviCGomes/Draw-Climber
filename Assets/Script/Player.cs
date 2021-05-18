using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour {
    public bool FirstDraw {
        get;
        set;
    }
    public float Speed {
        get;
        set;
    }
    public bool Run {
        get;
        set;
    }
    public Rigidbody Rigidbody {
        get;
        set;
    }
    public Color Color {
        get;
        set;
    }
    public Animator Animator {
        get;
        set;
    }
    public bool IsGrounded {
        get;
        set;
    }
    public Shader Shader {
        get;
        set;
    }
    public float VertexPrecision {
        get;
        set;
    }
    public GameObject LeftL {
        get;
        set;
    }
    public GameObject RightL {
        get;
        set;
    }
    public GameObject DrawObject {
        get;
        set;
    }
    private float Distance {
        get;
        set;
    }

    private void Start() {
        Speed = 5f;
        FirstDraw = false;
        Color = new Color(0, 0, 0, 1);
        Distance = 1f;
        LeftL.GetComponent<Rotation>().Angle = new Vector3(0, 0, -1);
        LeftL.GetComponent<Rotation>().Speed = 500.0f;
        LeftL.GetComponent<Rotation>().Rotate = false;
        RightL.GetComponent<Rotation>().Angle = new Vector3(0, 0, -1);
        RightL.GetComponent<Rotation>().Speed = 500.0f;
        RightL.GetComponent<Rotation>().Rotate = false;
    }

    // Update is called once per frame
    void Update() {
        if(Run) {
            LeftL.GetComponent<Rotation>().Rotate = true;
            RightL.GetComponent<Rotation>().Rotate = true;
        } else {
            LeftL.GetComponent<Rotation>().Rotate = false;
            RightL.GetComponent<Rotation>().Rotate = false;
        }
        if(Input.GetMouseButtonDown(0)) {
            if(EventSystem.current.IsPointerOverGameObject())
                StartCoroutine(draw());
        }
        CheckGround();
        if(transform.position.y < 0f) {
            transform.position = new Vector3(0, 3f, 0);
        }
    }

    Vector3 newVertex;
    Vector3 lastVertex;
    IEnumerator draw() {
        Run = false;
        Distance = 0f;

        DrawnDestroy();

        //Setting up the line render
        LineRenderer r = new GameObject().AddComponent<LineRenderer>();
        r.transform.SetParent(transform);
        r.startWidth = 0.3f;
        r.endWidth = 0.3f;
        r.material = new Material(Shader);
        r.material.color = Color;
        r.gameObject.tag = "Line";

        List<Vector3> positions = new List<Vector3>();

        while(Input.GetMouseButton(0)){ // Adding Mouse Points To Line Render
            newVertex = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 5;
            if(Vector3.Distance(lastVertex, newVertex) >= VertexPrecision){ //Checking distance between vertx
                positions.Add(newVertex);
                r.positionCount = positions.Count;
                r.SetPositions(positions.ToArray());
                lastVertex = newVertex;
            }
            yield return new WaitForEndOfFrame();
        }

        r.useWorldSpace = false;
        Vector3 aux = positions[0];
        for(int i = 0; i < positions.Count; i++) {
            Instantiate(DrawObject, new Vector3(((positions[i].x - aux.x)/2f) + transform.position.x , ((positions[i].y - aux.y) / 2f) + transform.position.y, LeftL.transform.position.z), new Quaternion(), LeftL.transform);
            Instantiate(DrawObject, new Vector3(((aux.x - positions[i].x)/2f) + transform.position.x , (((aux.y - positions[i].y) / 2f) + transform.position.y), RightL.transform.position.z), new Quaternion(), RightL.transform);
            if(positions[i].y < 0)
                if(positions[i].y < Distance)
                    Distance = positions[i].y;
        }

        Distance = ((Distance - aux.y) / 2) + transform.position.y;
        Run = true;
    }

    private void CheckGround() {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(1f,-1f,0);

        Debug.DrawRay(origin, direction * Distance, Color.red);
        if(Physics.Raycast(origin, direction, out RaycastHit hit, Distance)) {
            IsGrounded = true;
        } else {
            IsGrounded = false;
        }
    }

    private void DrawnDestroy() {
        GameObject[] game = GameObject.FindGameObjectsWithTag("DrawnObject");
        if(game != null) {
            foreach(GameObject dO in game) {
                Destroy(dO.gameObject);
            }
        }
        game = GameObject.FindGameObjectsWithTag("Line");
        if(game != null) {
            foreach(GameObject dO in game) {
                Destroy(dO.gameObject);
            }
        }
    }
}
