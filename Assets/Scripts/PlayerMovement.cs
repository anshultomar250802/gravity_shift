using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float rayDistance;
    [SerializeField] private bool isGrounded;
    [SerializeField] private Animator playerAnimator;
    private float gravityForce = Physics.gravity.magnitude;
    private Rigidbody rb;
    private float xAxis;
    private float yAxis;
    public float mouseSensitivity = 100f;
    private float yRotation = 0f;

    public LayerMask groundLayerMask;

    private GameObject player;
    public GameObject Player => player;
    public bool IsManupilated { get; set; }

    void Start()
    {
        player = this.gameObject;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        PlayerInput();
        MovementAnimation();
        GravitySwitcher();
        Jump();
        if (!HoloIndicator.canManipulate)
        {
            Debug.Log("CAN ROTATE");
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            yRotation += mouseX;
            Camera.main.transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        }
    }

    private void MovementAnimation()
    {
        bool isRunning = xAxis != 0 || yAxis != 0;
        playerAnimator.SetBool("isRunning", isRunning);
    }

    private void PlayerInput()
    {
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundLayerMask);
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");
    }

    void GravitySwitcher()
    {
        Vector3 movement;
        switch (HoloIndicator.GravitySideEnum)
        {
            case GravitySide.UP:
                Debug.Log("UP state");
                rb.AddForce(-Vector3.down * gravityForce, ForceMode.Acceleration);
                movement = Camera.main.transform.right * xAxis + Camera.main.transform.forward * yAxis;
                rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);
                break;
            case GravitySide.LEFT:
                Debug.Log("LEFT state");
                rb.AddForce(Vector3.left * gravityForce, ForceMode.Acceleration);
                movement = -Camera.main.transform.right * xAxis + Camera.main.transform.forward * yAxis;
                rb.velocity = new Vector3(rb.velocity.x, movement.y * speed, movement.z * speed);
                break;
            case GravitySide.RIGHT:
                Debug.Log("RIGHT state");
                rb.AddForce(-Vector3.left * gravityForce, ForceMode.Acceleration);
                movement = Camera.main.transform.right * xAxis + Camera.main.transform.forward * yAxis;
                rb.velocity = new Vector3(rb.velocity.x, movement.y * speed, movement.z * speed);
                break;
            case GravitySide.NORMAL:
                Debug.Log("NORMAL state");
                movement = Camera.main.transform.right * xAxis + Camera.main.transform.forward * yAxis;
                rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);
                break;
            default:
                Debug.Log("Default State");
                break;
        }
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerAnimator.SetBool("isJumping", true);
        }
        else
        {
            playerAnimator.SetBool("isJumping", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Points")
        {
            collision.gameObject.SetActive(false);
        }
    }
}
public enum GravitySide { UP, LEFT, RIGHT, NORMAL }
