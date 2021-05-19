using UnityEngine;


public class DrawClimber : MonoBehaviour {
    //variáveis não encapsuladar para alteração no editor
    public Player player;
    public float finishLine;
    public Shader shader;
    public float vertexPrecision;
    public GameObject leftL, rightL;
    public GameObject drawObj;

    //Inicializa as variáveis do player
    void Awake () {
        //Inicializar o Rigidbody e a velocidade do jogador
        player.Speed = 5f;
        player.Rigidbody = player.GetComponent<Rigidbody>();
        player.Rigidbody.useGravity = false;

        //Inicializa o shader, cor do traço e os objetos3d
        player.Shader = shader;
        player.VertexPrecision = vertexPrecision;
        player.DrawObject = drawObj;
        player.Color = new Color(0, 0, 0, 1);

        //Inicializa o eixo de rotação e velocidade da perna esquerda
        player.LeftL = leftL;
        player.LeftL.GetComponent<Rotation>().Angle = new Vector3(0, 0, -1);
        player.LeftL.GetComponent<Rotation>().Speed = 500.0f;
        player.LeftL.GetComponent<Rotation>().Rotate = false;

        //Inicializa o eixo de rotação e velocidade da perna direita
        player.RightL = rightL;
        player.RightL.GetComponent<Rotation>().Angle = new Vector3(0, 0, -1);
        player.RightL.GetComponent<Rotation>().Speed = 500.0f;
        player.RightL.GetComponent<Rotation>().Rotate = false;
    }

    void Update() {
        //Define a direção do jogador e habilita a gravidade quando o jogador estiver correndo
        if(player.Run) {
            player.Rigidbody.useGravity = true;
            if(player.IsGrounded)
                player.Rigidbody.velocity = transform.TransformVector(player.Speed, 0, 0);
        } else {
            player.Rigidbody.useGravity = false;
            player.Rigidbody.velocity = transform.TransformVector(0, 0, 0);
        }

        //O jogador para ao chegar na linha de chegada
        if(player.transform.position.x == finishLine) {
            player.Rigidbody.useGravity = false;
            player.Rigidbody.velocity = transform.TransformVector(0, 0, 0);
            player.Run = false;
        }
        
        
    }
}
