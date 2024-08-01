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
        Input();
        MovementAnimation();
        GravitySwitcher();
        Jump();

    }

    private void MovementAnimation()
    {
        bool isRunning = xAxis != 0 || yAxis != 0;
        playerAnimator.SetBool("isRunning", isRunning);
    }

    private void Input()
    {
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundLayerMask);
        xAxis = UnityEngine.Input.GetAxis("Horizontal");
        yAxis = UnityEngine.Input.GetAxis("Vertical");
    }

    void GravitySwitcher()
    {
        Vector3 movement;
        switch (HoloIndicator.GravitySideEnum)
        {
            case GravitySide.UP:
                Debug.Log("UP state");
                rb.AddForce(-Vector3.down * gravityForce, ForceMode.Acceleration);
                movement = player.transform.right * xAxis + player.transform.forward * yAxis;
                rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);
                break;
            case GravitySide.LEFT:
                Debug.Log("LEFT state");
                rb.AddForce(Vector3.left * gravityForce, ForceMode.Acceleration);
                movement = -player.transform.right * xAxis + player.transform.forward * yAxis;
                rb.velocity = new Vector3(rb.velocity.x, movement.y * speed, movement.z * speed);
                break;
            case GravitySide.RIGHT:
                Debug.Log("RIGHT state");
                rb.AddForce(-Vector3.left * gravityForce, ForceMode.Acceleration);
                movement = player.transform.right * xAxis + player.transform.forward * yAxis;
                rb.velocity = new Vector3(rb.velocity.x, movement.y * speed, movement.z * speed);
                break;
            case GravitySide.NORMAL:
                Debug.Log("NORMAL state");
                movement = player.transform.right * xAxis + player.transform.forward * yAxis;
                rb.velocity = new Vector3(movement.x * speed, rb.velocity.y * speed, movement.z * speed);
                break;
            default:
                Debug.Log("Default State");
                break;
        }
    }
    private void Jump()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space) && isGrounded)
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
