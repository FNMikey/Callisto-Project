using UnityEngine;



public class PlayerBHV : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float sprintMultiplier = 2f;
    public Camera cam;
    public static string save;
    Rigidbody rigidBody;
    Vector3 movement;
    Vector3 mousePos;

    public GameObject heldItem;

    public static string getSave()
    {
        return save;
    }

    public static void setSave(string name)
    {
        Debug.Log(name);
        save = name;
    }

    void Start()
    {
        Transform camTransform = cam.transform;

        rigidBody = GetComponent<Rigidbody>();
        if (camTransform == null)
        {
            camTransform = cam.transform;
        }


    }

    void Update()
    {
        // Input
        float moveX = Input.GetAxisRaw("Horizontal");

        float moveZ = Input.GetAxisRaw("Vertical");

        // Calculate movement vector
        Vector3 moveDirection =
            Quaternion.Euler(0, 45, 0) * new Vector3(moveX, 0f, moveZ);



        // Mouse position
        Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            mousePos = cameraRay.GetPoint(rayLength);

            // Determine the rotation towards the mouse position
            Quaternion newRotation =
                Quaternion
                    .LookRotation(new Vector3(mousePos.x - transform.position.x,
                        0f,
                        mousePos.z - transform.position.z));
            rigidBody.MoveRotation (newRotation);
        }

        // Sprinting
        movement = new Vector3(moveDirection.x, 0f, moveDirection.z).normalized;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movement *= sprintMultiplier;
        }

    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        rigidBody
            .MovePosition(rigidBody.position +
            movement * moveSpeed * Time.fixedDeltaTime);


    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(mousePos, 0.1f);
    }
}
