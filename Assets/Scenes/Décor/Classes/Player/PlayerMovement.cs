using UnityEngine;

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    void Start()
    {
        transform.position = new Vector3(0, 0.5f, 0);
    }
    private Vector3 playerMovementInput;
    private Vector2 playerMouseInput;
    private float xRotation;

    [SerializeField] private Transform playerCamera;
    [SerializeField] private CharacterController controller;
    
    [Space]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sensitivity = 2f;

    void Update()
    {
        // Récupération des entrées clavier et souris
        playerMovementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        playerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // Appel des fonctions de mouvement et de rotation
        MovePlayer();
        MovePlayerCamera();
    }

    private void MovePlayer()
    {
        // Conversion du vecteur de mouvement selon la direction du joueur
        Vector3 moveVector = transform.TransformDirection(playerMovementInput);

        // Gestion de la montée et descente (espace et shift)
        float velocityY = 0f;
        if (Input.GetKey(KeyCode.Space))
        {
            velocityY = 1f;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            velocityY = -1f;
        }

        moveVector.y = velocityY;

        // Déplacement avec le CharacterController
        controller.Move(moveVector * speed * Time.deltaTime);
    }

    private void MovePlayerCamera()
    {
        // Rotation de la caméra en fonction de la souris
        xRotation -= playerMouseInput.y * sensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limite de la rotation verticale

        transform.Rotate(Vector3.up * playerMouseInput.x * sensitivity);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
