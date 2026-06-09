using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lesson08_CharacterControllerMove : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerController playerController;
    private Vector2 moveInput;
    private Vector3 velocity;
    [SerializeField] private float gravity = -1f;
    [SerializeField] private float walkSpeed;
    private Vector3 horizontalMove;
    private Vector3 finalMove;
    [SerializeField] private float jumpHeight = 10f;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerController = new PlayerController();
    }

    private void OnEnable()
    {
        playerController.Enable();
        playerController.Player.Move.performed += OnWalk;
        playerController.Player.Move.canceled += OnWalk;
        playerController.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        playerController.Player.Move.performed -= OnWalk;
        playerController.Player.Move.canceled -= OnWalk;
        playerController.Player.Jump.performed -= OnJump;
        playerController.Disable();
    }

    private void OnWalk(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        }
    }
    private void Update()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        horizontalMove = (Vector3.forward * moveInput.y + Vector3.right * moveInput.x) * walkSpeed;
        velocity.y += gravity * Time.deltaTime;
        finalMove = horizontalMove + velocity;
        characterController.Move(finalMove*Time.deltaTime);
    }
    
}
