using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Lesson01 : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    public float maxCla = 50f;
    public float sensitivity =0.6f;
    private float mouseY;
    private PlayerController playerController;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 moveDir;
    private float rotateInput;
    [FormerlySerializedAs("speed")] public float WalkSpeed = 2f;
    private float currentSpeed;
    public float rotateSpeed = 100f;
    private float runSpeed = 4f;
    private bool isRunning;
    // Start is called before the first frame update
    private void Awake()
    {
        playerController = new PlayerController();
    }

    private void Start()
    {
        currentSpeed = WalkSpeed;
        runSpeed = WalkSpeed*2;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        playerController.Enable();
        //回调函数 unity会自动填入对应的信息参数
        playerController.Player.Move.performed += OnMove;
        playerController.Player.Move.canceled += OnMove;
        
        playerController.Player.Run.performed += OnRun;
        playerController.Player.Run.canceled += OnRun;
        
        playerController.Player.Rotate.performed += OnRotate;
        playerController.Player.Rotate.canceled += OnRotate;
        
        playerController.Player.Look.performed += OnLook;
        playerController.Player.Look.canceled += OnLook;
    }

    private void OnLook(InputAction.CallbackContext ctx)
    {
        lookInput = ctx.ReadValue<Vector2>();
    }

    private void OnDisable()
    {
        playerController.Disable();
        playerController.Player.Move.performed -= OnMove;
        playerController.Player.Move.canceled -= OnMove;
        
        playerController.Player.Run.performed -= OnRun;
        playerController.Player.Run.canceled -= OnRun;
        
        playerController.Player.Rotate.performed -= OnRotate;
        playerController.Player.Rotate.canceled -= OnRotate;
        
        playerController.Player.Look.performed -= OnLook;
        playerController.Player.Look.canceled -= OnLook;
    }

    // Update is called once per frame
   
    #region 新版输入系统
    
    
    private void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnRun(InputAction.CallbackContext ctx)
    {
        isRunning = ctx.performed;
        if (isRunning)
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = WalkSpeed;
        }
        print(isRunning);
    }
    private void OnRotate(InputAction.CallbackContext ctx)
    {
        rotateInput = ctx.ReadValue<float>();
    }
    #endregion
    void Update()
    {
        transform.Rotate(Vector3.up*sensitivity*lookInput.x*Time.deltaTime);
        // mouseX += lookInput.x*sensitivity;
        mouseY -= lookInput.y*sensitivity*Time.deltaTime;
        mouseY = Mathf.Clamp(mouseY, -maxCla, maxCla);
        cameraTransform.transform.localRotation = Quaternion.Euler(mouseY, 0, 0);
        transform.Translate(new Vector3(moveInput.x,0,moveInput.y) * (currentSpeed * Time.deltaTime));
        transform.Rotate(Vector3.up * (rotateInput * rotateSpeed * Time.deltaTime));
        #region 旧版输入系统
        /*if (Input.GetKey(KeyCode.W))
        {
            gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            gameObject.transform.Translate(Vector3.back * speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Translate(Vector3.right * speed * Time.deltaTime);
        }*/
        #endregion
    }
    
}
