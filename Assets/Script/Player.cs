using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour {
    public float vertexPrecision = 0.1f;
    public Shader shader;

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
    public GameObject leftL, rightL;
    public Animator animator;


    //public DrawScript drawScript;

    private void Start() {
        Speed = 5f;
        FirstDraw = false;
        Color = new Color(0, 0, 0, 1);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if(Run)
            animator.SetBool("Run", true);
        else
            animator.SetBool("Run", false);
        if(Input.GetMouseButtonDown(0)) {
            
            if(EventSystem.current.IsPointerOverGameObject()) // Avoid Draw On Ui Element            
                StartCoroutine(draw());
        }
    }

    Vector3 newVertex;
    Vector3 lastVertex;
    public float colliderThickness;

    IEnumerator draw() {
        //Setting up the line render
        LineRenderer r = new GameObject().AddComponent<LineRenderer>();
        r.transform.SetParent(transform);
        r.startWidth = 1f;
        r.endWidth = 1f;
        r.material = new Material(shader);
        r.material.color = Color;

        List<Vector3> posiciones = new List<Vector3>();

        while(Input.GetMouseButton(0)) // Adding Mouse Points To Line Render
        {
            newVertex = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 5;
            if(Vector3.Distance(lastVertex, newVertex) >= vertexPrecision) //Checking distance between vertx
            {
                posiciones.Add(newVertex);
                r.positionCount = posiciones.Count;
                r.SetPositions(posiciones.ToArray());
                lastVertex = newVertex;
            }
            yield return new WaitForEndOfFrame();
        }

        r.useWorldSpace = false;

        //if (usePhysics){ // Add Physics to line render
            List<Vector2> posiciones2 = new List<Vector2>();

            for (int i = 0; i < posiciones.Count; i++) // Forward position to add
                posiciones2.Add(new Vector2(posiciones[i].x, posiciones[i].y));

            for (int i = posiciones.Count - 1; i > 0; i--) //Backward position to add
                posiciones2.Add(new Vector2(posiciones[i].x, posiciones[i].y + colliderThickness));

            leftL = r.gameObject;
        rightL = r.gameObject;
        //PolygonCollider2D col = r.gameObject.AddComponent<PolygonCollider2D>();
        //col.points = posiciones2.ToArray();
        //col.gameObject.AddComponent<Rigidbody2D>();
        //}
        Run = true;
    }
}
