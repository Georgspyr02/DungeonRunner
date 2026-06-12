using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 10f;
    public float laneDistance = 3f;       // Distance between lanes
    public float laneSwitchSpeed = 10f;   // How fast the player moves horizontally
    public float gravity = -9.81f;

    private CharacterController controller;
    private int currentLane = 1;          // 0 = left, 1 = middle, 2 = right
    private Vector3 targetPosition;
    private float verticalVelocity = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Align player to middle lane and tile surface
        targetPosition = transform.position;
        targetPosition.x = (currentLane - 1) * laneDistance;

        // Align Y to CharacterController height on top of road (assumes tile Y=0)
        targetPosition.y = controller.height / 2f;
        transform.position = targetPosition;
    }

   void Update()
{
    // --- Lane Input (arrow keys + A/D) ---
    if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        MoveLane(-1);
    if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        MoveLane(1);

    // --- Forward/backward input (optional) ---
    if (Input.GetKey(KeyCode.W))
        transform.position += Vector3.forward * forwardSpeed * Time.deltaTime;
    if (Input.GetKey(KeyCode.S))
        transform.position -= Vector3.forward * forwardSpeed * Time.deltaTime;

    // --- ESC to quit ---
    if (Input.GetKeyDown(KeyCode.Escape))
    {
        QuitGame();
    }

    // --- Existing movement code ---
    float targetX = (currentLane - 1) * laneDistance;
    float targetZ = transform.position.z + forwardSpeed * Time.deltaTime;
    targetPosition = new Vector3(targetX, transform.position.y, targetZ);

    // Gravity
    if (controller.isGrounded)
        verticalVelocity = -1f;
    else
        verticalVelocity += gravity * Time.deltaTime;

    targetPosition.y += verticalVelocity * Time.deltaTime;

    Vector3 move = targetPosition - transform.position;
    controller.Move(move);
}

// Quit game method
public void QuitGame()
{
    Debug.Log("Quit Game triggered");
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false; // stops play in editor
#else
    Application.Quit(); // quits build
#endif
}




    private void MoveLane(int direction)
    {
        currentLane += direction;
        currentLane = Mathf.Clamp(currentLane, 0, 2); // ensure we stay in 3 lanes
    }
}


