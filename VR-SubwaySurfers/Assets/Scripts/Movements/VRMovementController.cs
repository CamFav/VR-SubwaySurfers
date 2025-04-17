using UnityEngine;
using System.Collections;

/// <summary>
/// Moves the player using hand gestures.
/// This script detects left/right swipes for lane changes,
/// upward hand movement for jumping, and head lowering for sliding.
/// </summary>
public class VRMovementController : MonoBehaviour
{
    [Header("XR References")]
    public Transform headTransform; 
    public Transform leftHand;
    public Transform rightHand;

    [Header("Lane Movement")]
    public float lineDistance = 1.5f; // Distance between lanes
    public float swipeThreshold = 1.0f; // Threshold for swipe detection
    public float gestureCooldown = 0.4f; // Cooldown time between gestures
    private int currentLine = 1; // 0 = left, 1 = center, 2 = right
    private float lastGestureTime; // Last time a gesture was detected

    [Header("Jump")]
    public float jumpSpeedThreshold = 1.2f; // Speed threshold for jump detection
    public float jumpCooldown = 1.0f; // Cooldown time between jumps
    public float jumpHeight = 0.3f;
    public float jumpDuration = 0.3f;
    private float lastJumpTime; // Last time jump was detected
    private bool isJumping = false;

    [Header("Slide")]
    public float slideThreshold = 1.0f; // Threshold for slide detection
    public float slideCooldown = 1.0f;
    private float lastSlideTime; // Last time slide was detected
    private bool isSliding = false;

    private Vector3 lastLeftHandPos;
    private Vector3 lastRightHandPos;

    void Start()
    {
        // Store initial hand positions
        lastLeftHandPos = leftHand.position;
        lastRightHandPos = rightHand.position;
    }

    void Update()
    {
        DetectLaneChange();
        DetectJump();
        DetectSlide();
        UpdateHandPositions();
    }

    // Detects left/right swipe gestures for lane change
    void DetectLaneChange()
    {
        if (Time.time - lastGestureTime < gestureCooldown) return;

        Vector3 leftVelocity = (leftHand.position - lastLeftHandPos) / Time.deltaTime;
        Vector3 rightVelocity = (rightHand.position - lastRightHandPos) / Time.deltaTime;

        if (leftVelocity.x < -swipeThreshold && currentLine > 0)
        {
            currentLine--;
            MoveToLine(currentLine);
            lastGestureTime = Time.time;
            Debug.Log("Left swipe detected");
        }
        else if (rightVelocity.x > swipeThreshold && currentLine < 2)
        {
            currentLine++;
            MoveToLine(currentLine);
            lastGestureTime = Time.time;
            Debug.Log("Right swipe detected");
        }
    }

    // Detects upward hand movement for jump
    void DetectJump()
    {
        if (Time.time - lastJumpTime < jumpCooldown || isJumping) return;

        Vector3 leftVel = (leftHand.position - lastLeftHandPos) / Time.deltaTime;
        Vector3 rightVel = (rightHand.position - lastRightHandPos) / Time.deltaTime;

        if (leftVel.y > jumpSpeedThreshold && rightVel.y > jumpSpeedThreshold)
        {
            StartCoroutine(JumpRoutine());
            lastJumpTime = Time.time;
            Debug.Log("Jump detected");
        }
    }

    // Detects head lowering for slide
    void DetectSlide()
    {
        float headY = headTransform.localPosition.y;

        if (!isSliding && headY < slideThreshold && Time.time - lastSlideTime > slideCooldown)
        {
            isSliding = true;
            lastSlideTime = Time.time;
            Debug.Log("Slide detected");
        }
        else if (headY > slideThreshold + 0.2f && isSliding)
        {
            isSliding = false;
        }
    }

    // Moves player to the specified lane
    void MoveToLine(int line)
    {
        float targetX = (line - 1) * lineDistance;
        Vector3 newPos = new Vector3(targetX, transform.position.y, transform.position.z);
        transform.position = newPos;
    }

    // Handles jump animation
    IEnumerator JumpRoutine()
    {
        isJumping = true;
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + Vector3.up * jumpHeight;

        float elapsed = 0f;
    
        // Jump up
        while (elapsed < jumpDuration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / jumpDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        yield return new WaitForSeconds(0.1f);

        elapsed = 0f;
        while (elapsed < jumpDuration)
        {
            transform.position = Vector3.Lerp(targetPos, startPos, elapsed / jumpDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos;
        isJumping = false;
    }

    // Updates last hand positions for velocity calculation
    void UpdateHandPositions()
    {
        lastLeftHandPos = leftHand.position;
        lastRightHandPos = rightHand.position;
    }
}
