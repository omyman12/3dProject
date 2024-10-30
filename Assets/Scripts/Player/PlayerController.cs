using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")] //해더: 타이틀이 하나생김
    public float moveSpeed;
    public float runSpeed;
    public float curSpeed;
    private Vector2 movementInput;
    public bool isRunning;
    public float jumpForce;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;

    public Action inventory;
    private Vector2 mouseDelta;
    int power = 250;

    [HideInInspector]
    public bool canLook = true;

    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //마우스커서를 락
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }

    }

    public void OnLookInput(InputAction.CallbackContext context) //마우스 이동량을 받음
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context) //현재상태를 받아옴
    {
        if (context.phase == InputActionPhase.Performed) //눌리는동안
        {
            movementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled) //땔떄
        {
            movementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context) // 누를때 발동, 땅일때, 스태미나가 10이 있을때 점프
    {
        if (context.phase == InputActionPhase.Started && IsGrounded() && CharacterManager.Instance.Player.condition.UseStamina(10))
        {
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        }
    }
    public void OnRunInput(InputAction.CallbackContext context) //쉬프트를 누르면 뛰고있음을 확인
    {
        if (context.phase == InputActionPhase.Performed)
        {
            isRunning = true;
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            isRunning = false;
        }
    }
    
    private void Move()
    {
        Vector3 dir = transform.forward * movementInput.y + transform.right * movementInput.x;
        dir *= isRunning ? runSpeed : moveSpeed; 
        dir.y = rigidbody.velocity.y;

        rigidbody.velocity = dir;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Jump"))
        {
            rigidbody.AddForce(Vector2.up * power, ForceMode.Impulse);
        }
        if (collision.gameObject.CompareTag("MovingObject")) // 땅에 붙어있어도 같이움직이는 문제 해결해야함
        {
            transform.SetParent(collision.transform);  // 부모로 설정
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //내려왔을 때
        if (collision.gameObject.CompareTag("MovingObject"))
        {
            transform.SetParent(null);  // 부모 관계 해제
        }
    }


    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void OnInventoryButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
