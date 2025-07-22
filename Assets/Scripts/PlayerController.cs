using UnityEngine;

using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;

    //--Двойной прыжок--
    public int maxJumpCount = 2; // для двойного — 2, для тройного — 3
    private int currentJumpCount = 0;

    //-- Скольжение по определённым платформам --

    private bool isOnSlope = false;
    public float slideSpeed = 5f;


    InputAction moveAction;
    InputAction jumpAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }
    // Update is called once per frame
    RaycastHit hit;
    void CheckSlope()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        if (Physics.Raycast(ray, out hit, 1.5f))
        {
            isOnSlope = hit.collider.CompareTag("Slope");
        }
        else
        {
            isOnSlope = false;
        }
    }

    void Update()
    {
        CheckSlope();
        // Проверка, стоит ли персонаж на земле
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        // Небольшое смещение вниз, чтобы персонаж не "зависал" в воздухе
        {
            playerVelocity.y = -2f;
        }

        if (isGrounded)
        {
            currentJumpCount = 0;
        }

        // Получение ввода игрока
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(moveValue.x, 0, moveValue.y);
        move = Vector3.ClampMagnitude(move, 1f);

        if (move != Vector3.zero)
        {
            transform.forward = move;
        }

        // Прыжок
        // if (jumpAction.IsPressed() && controller.isGrounded)
        // {
        //     playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        // }

        if (jumpAction.WasPressedThisFrame() && currentJumpCount < maxJumpCount)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            currentJumpCount++;
        }

        playerVelocity.y += gravity * Time.deltaTime;
        if (isOnSlope && isGrounded)
        {
            Vector3 slideDirection = new Vector3(hit.normal.x, -Mathf.Abs(hit.normal.y), hit.normal.z);
            playerVelocity += slideDirection.normalized * slideSpeed * Time.deltaTime;
        }

        controller.Move((move * speed + playerVelocity.y * Vector3.up) * Time.deltaTime);
    }
}