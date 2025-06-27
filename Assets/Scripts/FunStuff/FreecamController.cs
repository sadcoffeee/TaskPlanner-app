using UnityEngine;
using UnityEngine.EventSystems;

public class FreeCamController : MonoBehaviour
{
    public GameObject freeCamUI;
    public float rotationSpeed = 2.0f;
    public float movementSpeed = 5.0f;

    private bool isFreeCamActive = false;
    private Vector2 swipeStart;

    private Vector3 movementDirection;

    void Update()
    {
        if (isFreeCamActive)
        {
            HandleSwipeRotation();
            HandleMovement();
        }
    }

    public void EnableFreeCam()
    {
        isFreeCamActive = true;
        freeCamUI.SetActive(true);
    }

    public void DisableFreeCam()
    {
        isFreeCamActive = false;
        freeCamUI.SetActive(false);
    }

    private void HandleSwipeRotation()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                swipeStart = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector2 swipeDelta = touch.deltaPosition;

                // Calculate the desired pitch (x-axis) and yaw (y-axis) rotations.
                float pitch = -swipeDelta.y * rotationSpeed * Time.deltaTime; // Rotation around world x-axis
                float yaw = swipeDelta.x * rotationSpeed * Time.deltaTime;   // Rotation around world y-axis

                // Get the current camera rotation.
                Quaternion currentRotation = Camera.main.transform.rotation;

                // Apply yaw (rotation around the world y-axis).
                Quaternion yawRotation = Quaternion.AngleAxis(yaw, Vector3.up);

                // Apply pitch (rotation around the camera's local right axis).
                Quaternion pitchRotation = Quaternion.AngleAxis(pitch, Camera.main.transform.right);

                // Combine the rotations, keeping the camera aligned to the world up-axis.
                Camera.main.transform.rotation = yawRotation * currentRotation * pitchRotation;
            }
        }
    }


    private void HandleMovement()
    {
        if (movementDirection != Vector3.zero)
        {
            // Get the camera's forward vector, constrained to the horizontal plane (y = 0).
            Vector3 forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;

            // Get the camera's right vector, constrained to the horizontal plane (y = 0).
            Vector3 right = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized;

            // Combine the constrained forward and right vectors with the movement direction.
            Vector3 worldMovement = movementDirection.z * forward + movementDirection.x * right + movementDirection.y * Vector3.up;

            // Move the camera.
            Camera.main.transform.Translate(worldMovement * movementSpeed * Time.deltaTime, Space.World);
        }
    }


    public void StartMovement(string direction)
    {
        Vector3 directionVector = Vector3.zero;
        switch (direction)
        {
            case "right":
                directionVector = new Vector3(1, 0, 0);
                break;
            case "left":
                directionVector = new Vector3(-1, 0, 0);
                break;
            case "up":
                directionVector = new Vector3(0, 1, 0);
                break;
            case "down":
                directionVector = new Vector3(0, -1, 0);
                break;
            case "forward":
                directionVector = new Vector3(0, 0, 1);
                break;
            case "backward":
                directionVector = new Vector3(0, 0, -1);
                break;
        }
        movementDirection = directionVector;
    }

    public void StopMovement()
    {
        movementDirection = Vector3.zero;
    }
}
