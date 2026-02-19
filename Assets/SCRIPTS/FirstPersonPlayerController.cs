// Importa el espacio de nombres principal de Unity (MonoBehaviour, Vector3, etc.)
using UnityEngine;

// Obliga a que este GameObject tenga un CharacterController (si no, Unity lo añade automáticamente)
[RequireComponent(typeof(CharacterController))]
// Define la clase FirstPersonPlayerController que hereda de MonoBehaviour
public class FirstPersonPlayerController : MonoBehaviour
{
    // Velocidad de movimiento del jugador al desplazarse por el escenario
    public float moveSpeed = 5f;
    // Altura del salto que podrá alcanzar el jugador
    public float jumpHeight = 2f;
    // Valor de la gravedad aplicada al jugador (negativo para que empuje hacia abajo)
    public float gravity = -9.81f;
    // Sensibilidad del ratón para girar la cámara / jugador
    public float mouseSensitivity = 200f;
    // Referencia al Transform de la cámara que usaremos para rotar la vista en vertical
    public Transform cameraTransform;

    // Referencia al componente CharacterController que mueve físicamente al jugador
    private CharacterController controller;
    // Vector que guarda la velocidad actual del jugador (especialmente la vertical para el salto)
    private Vector3 velocity;
    // Acumulador de rotación en el eje X (para mirar arriba y abajo con la cámara)
    private float xRotation = 0f;

    // Se ejecuta una vez al inicio de la escena
    void Start()
    {
        // Obtiene el componente CharacterController que está en el mismo GameObject
        controller = GetComponent<CharacterController>();
        // Bloquea el ratón en el centro de la pantalla para que no se salga de la ventana del juego
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Se ejecuta una vez por frame
    void Update()
    {
        // Llama al método que se encarga de mover al jugador y gestionar la gravedad/salto
        MoverJugador();
        // Llama al método que controla la rotación del jugador y la cámara con el ratón
        RotarConRaton();
    }

    // Método responsable de leer el input y mover al jugador en el mundo
    void MoverJugador()
    {
        // Lee el eje horizontal (A/D o flechas izquierda/derecha)
        float inputX = Input.GetAxis("Horizontal");
        // Lee el eje vertical (W/S o flechas arriba/abajo)
        float inputZ = Input.GetAxis("Vertical");
        // Calcula la dirección de movimiento relativa al jugador (derecha + adelante)
        Vector3 move = transform.right * inputX + transform.forward * inputZ;
        // Mueve al jugador en la dirección calculada, con la velocidad indicada y teniendo en cuenta el tiempo entre frames
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Si el jugador está tocando el suelo y la velocidad vertical es negativa...
        if (controller.isGrounded && velocity.y < 0)
            // ...forzamos un pequeño valor negativo para mantenerlo pegado al suelo y evitar problemas con isGrounded
            velocity.y = -2f;

        // Si está en el suelo y se pulsa el botón de salto (por defecto la tecla Espacio)
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
            // Calcula la velocidad inicial necesaria para alcanzar la altura de salto deseada
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Aplica la gravedad a la velocidad vertical a lo largo del tiempo
        velocity.y += gravity * Time.deltaTime;
        // Mueve al jugador verticalmente (caída / salto) usando el CharacterController
        controller.Move(velocity * Time.deltaTime);
    }

    // Método encargado de rotar al jugador y la cámara usando el movimiento del ratón
    void RotarConRaton()
    {
        // Si no tenemos una cámara asignada, salimos para evitar errores de referencia nula
        if (cameraTransform == null) return;
        // Captura el movimiento horizontal del ratón y lo escala con la sensibilidad y el deltaTime
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        // Rota el cuerpo del jugador alrededor del eje Y (giro izquierda/derecha)
        transform.Rotate(Vector3.up * mouseX);

        // Captura el movimiento vertical del ratón y lo escala igual que antes
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        // Restamos mouseY para invertir el eje (mirar arriba/abajo de forma natural en un FPS)
        xRotation -= mouseY;
        // Limitamos la rotación vertical para que no pueda girar más de 80 grados hacia arriba o abajo
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        // Aplicamos la rotación acumulada al Transform de la cámara en el eje X
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
