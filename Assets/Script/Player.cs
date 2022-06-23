using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour {
    //Vari�veis encapsladar
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

    //Inicializar as vari�veis
    private void Start() {
        //Inicializa a distancia para a verifica��o se o jogador est� no ch�o
        Distance = 1f;
    }

    void Update() {
        //Ativa e desativa a rota��o das pernas quando o jogador est� ou n�o se movendo 
        if(Run) {
            LeftL.GetComponent<Rotation>().Rotate = true;
            RightL.GetComponent<Rotation>().Rotate = true;
        } else {
            LeftL.GetComponent<Rotation>().Rotate = false;
            RightL.GetComponent<Rotation>().Rotate = false;
        }

        //Permite ao jogador desenhar na tela e cirar as pernas
        if(Input.GetMouseButtonDown(0)) {
            if(EventSystem.current.IsPointerOverGameObject())
                StartCoroutine(draw());
        }
        
        //Verifica se o jogador est� no ch�o;
        CheckGround();

        //O jogador volta ao inicio caso ele caia
        if(transform.position.y < 0f) {
            transform.position = new Vector3(0, 3f, 0);
        }
    }

    //Vari�veis para o desenho
    Vector3 newVertex;
    Vector3 lastVertex;
    IEnumerator draw() {
        //Quando o jogador come�a a desenhar o personagem para de correr
        Run = false;

        //Reseta a distancia de detec��o ao ch�o
        Distance = 0f;

        //Destroi as pernas antigas
        DrawnDestroy();

        //Inicializa as vari�veis de desenho
        LineRenderer r = new GameObject().AddComponent<LineRenderer>();
        r.transform.SetParent(transform);
        r.startWidth = 0.3f;
        r.endWidth = 0.3f;
        r.material = new Material(Shader);
        r.material.color = Color;
        r.gameObject.tag = "Line";

        List<Vector3> positions = new List<Vector3>();

        //Adiciona os pontos ao LineRender
        while(Input.GetMouseButton(0)){
            newVertex = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 5;

            //Verifica a distancia entre os verteces
            if(Vector3.Distance(lastVertex, newVertex) >= VertexPrecision){
                positions.Add(newVertex);
                r.positionCount = positions.Count;
                r.SetPositions(positions.ToArray());
                lastVertex = newVertex;
            }
            yield return new WaitForEndOfFrame();
        }

        //Cria objetos para a forma��o das pernas
        r.useWorldSpace = false;
        Vector3 aux = positions[0];
        for(int i = 0; i < positions.Count; i++) {
            
            //Cria a perna esquerda no 3� e 4� quadrande
            Instantiate(DrawObject, new Vector3(((positions[i].x - aux.x)/2f) + transform.position.x , ((positions[i].y - aux.y) / 2f) + transform.position.y, LeftL.transform.position.z), new Quaternion(), LeftL.transform);

            //Cria a perna direita no 1� e 2� quadrande
            Instantiate(DrawObject, new Vector3(((aux.x - positions[i].x)/2f) + transform.position.x , (((aux.y - positions[i].y) / 2f) + transform.position.y), RightL.transform.position.z), new Quaternion(), RightL.transform);
            
            //Procura o ponto mais baixo para criar a distancia do ch�o
            if(positions[i].y < 0)
                if(positions[i].y < Distance)
                    Distance = positions[i].y;
        }

        //Ajeita a distancia do ch�o
        Distance = ((Distance - aux.y) / 2) + transform.position.y;

        //O personagem come�a a correr
        Run = true;
    }

    //Verifica se o personagem est� em contato com o ch�o
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

    //Destroi os objetos relacionados com o desenho
    private void DrawnDestroy() {

        //Destroi os objetos 3d nas pernas
        GameObject[] game = GameObject.FindGameObjectsWithTag("DrawnObject");
        if(game != null) {
            foreach(GameObject dO in game) {
                Destroy(dO.gameObject);
            }
        }

        //Destroi a lina desenhada
        game = GameObject.FindGameObjectsWithTag("Line");
        if(game != null) {
            foreach(GameObject dO in game) {
                Destroy(dO.gameObject);
            }
        }
    }
}
