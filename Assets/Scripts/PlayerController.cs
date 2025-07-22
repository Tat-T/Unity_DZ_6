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

    //----
    public int maxJumpCount = 2; // для двойного — 2, для тройного — 3
    private int currentJumpCount = 0;

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
    void Update()
    {
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
        controller.Move((move * speed + playerVelocity.y * Vector3.up) * Time.deltaTime);

    }
}