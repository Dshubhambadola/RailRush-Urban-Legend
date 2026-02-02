using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 10f;
    public float laneDistance = 3f;
    public float laneChangeSpeed = 10f;
    
    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public float gravity = -20f;
    public float jumpHeight = 2f;

    private CharacterController controller;
    private Vector3 verticalVelocity;
    private int currentLane = 1; // 0: Left, 1: Middle, 2: Right
    private float targetX;
    
    // Swipe detection
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isSwiping = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        targetX = transform.position.x;
    }

    private void Update()
    {
        HandleInput();
        MovePlayer();
    }

    private void HandleInput()
    {
        // Keyboard Input for testing
        if (Input.GetKeyDown(KeyCode.LeftArrow)) ChangeLane(-1);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) ChangeLane(1);
        else if (Input.GetKeyDown(KeyCode.UpArrow)) Jump();
        else if (Input.GetKeyDown(KeyCode.DownArrow)) StartCoroutine(Slide());

        // Swipe Input (Basic Implementation)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPosition = Input.GetTouch(0).position;
            isSwiping = true;
        }

        if (isSwiping && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouchPosition = Input.GetTouch(0).position;
            Vector2 diff = endTouchPosition - startTouchPosition;

            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
            {
                if (diff.x > 0) ChangeLane(1);
                else ChangeLane(-1);
            }
            else
            {
                if (diff.y > 0) Jump();
                else StartCoroutine(Slide());
            }
            isSwiping = false;
        }
    }

    private void ChangeLane(int direction)
    {
        currentLane += direction;
        currentLane = Mathf.Clamp(currentLane, 0, 2);
        
        // Calculate target X based on lane
        // Assuming Middle is 0, Left is -laneDistance, Right is +laneDistance
        if (currentLane == 0) targetX = -laneDistance;
        else if (currentLane == 1) targetX = 0;
        else if (currentLane == 2) targetX = laneDistance;
    }

    private void Jump()
    {
        if (controller.isGrounded)
        {
            // v = sqrt(h * -2 * g)
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private IEnumerator Slide()
    {
        // Simple scale reduction for sliding
        transform.localScale = new Vector3(1, 0.5f, 1);
        yield return new WaitForSeconds(1.0f); // Slide duration
        transform.localScale = Vector3.one;
    }

    private void MovePlayer()
    {
        // Calculate move direction
        Vector3 moveDir = new Vector3(targetX - transform.position.x, 0, 0); 
        
        // Apply forward movement
        Vector3 forwardMove = transform.forward * forwardSpeed * Time.deltaTime;
        
        // Apply Gravity
        if (!controller.isGrounded)
        {
            verticalVelocity.y += gravity * Time.deltaTime;
        }
        else if(verticalVelocity.y < 0)
        {
             verticalVelocity.y = -2f; // Keep grounded
        }

        // Apply X movement smoothing
        Vector3 xMove = new Vector3((targetX - transform.position.x) * laneChangeSpeed * Time.deltaTime, 0, 0);
        
        // Combine all
        Vector3 finalMove = forwardMove + xMove + (verticalVelocity * Time.deltaTime);
        
        controller.Move(finalMove);
    }
}
