using UnityEngine;
using System.Collections;

/// <summary>
/// Moves the player using hand gestures (Controller mode)
/// or by physical body movement (Hybrid mode).
/// </summary>
public class VRMovementController : MonoBehaviour
{
    public enum LocomotionMode { Controller, Hybrid }
    private Vector3 initialHeadPosition;

    [Header("XR References")]
    public Transform headTransform;
    public Transform leftHand;
    public Transform rightHand;

    [Header("Lane Movement")]
    public float lineDistance = 3.67f;
    public float swipeThreshold = 1.3f;
    public float gestureCooldown = 0.4f;
    private int currentLine = 1;
    private float lastGestureTime;

    [Header("Jump/Slide Interaction")]
    public float jumpBlockAfterSlideDuration = 1f;
    private float lastSlideTimeForJumpBlock = -100f;

    [Header("Jump")]
    public float jumpSpeedThreshold = 1.2f;
    public float jumpCooldown = 1.0f;
    public float jumpHeight = 0.3f;
    public float jumpDuration = 0.3f;
    private float lastJumpTime;
    public static bool isJumping = false;

    [Header("Slide")]
    public float slideThreshold = 1.0f;
    public float slideCooldown = 1.0f;
    private float lastSlideTime;
    public static bool isSliding = false;

    private Vector3 lastLeftHandPos;
    private Vector3 lastRightHandPos;
    private bool isEnabled = false;

    void Start()
    {
        initialHeadPosition = headTransform.position;
        lastLeftHandPos = leftHand.position;
        lastRightHandPos = rightHand.position;
    }

    void Update()
    {
        if (!isEnabled) return;

        LocomotionMode currentMode = (LocomotionMode)PlayerPrefs.GetInt("LocomotionMode", 0);

        DetectJump();
        DetectSlide();

        if (currentMode == LocomotionMode.Controller)
        {
            HandleControllerMode();
        }
        else if (currentMode == LocomotionMode.Hybrid)
        {
            HandleHybridMode();
        }

        UpdateHandPositions();
    }

    void HandleControllerMode()
    {
        if (Time.time - lastGestureTime < gestureCooldown) return;

        Vector3 leftVelocity = (leftHand.position - lastLeftHandPos) / Time.deltaTime;
        Vector3 rightVelocity = (rightHand.position - lastRightHandPos) / Time.deltaTime;

        if (leftVelocity.x < -swipeThreshold && currentLine > 0)
        {
            currentLine--;
            MoveToLine(currentLine);
            lastGestureTime = Time.time;
            Debug.Log("Swipe gauche (Controller)");
        }
        else if (rightVelocity.x > swipeThreshold && currentLine < 2)
        {
            currentLine++;
            MoveToLine(currentLine);
            lastGestureTime = Time.time;
            Debug.Log("Swipe droite (Controller)");
        }
    }

    void HandleHybridMode()
    {
        float deltaX = headTransform.position.x - initialHeadPosition.x;

        if (deltaX < -0.3f && currentLine > 0)
        {
            currentLine--;
            MoveToLine(currentLine);
            Debug.Log("Walk left detected");
            initialHeadPosition = headTransform.position; // reset ref
        }
        else if (deltaX > 0.3f && currentLine < 2)
        {
            currentLine++;
            MoveToLine(currentLine);
            Debug.Log("Walk right detected");
            initialHeadPosition = headTransform.position; // reset ref
        }
    }



    void DetectJump()
    {
        if (Time.time - lastJumpTime < jumpCooldown || isJumping) return;

        // Blocage temporaire du saut juste après un slide
        if (Time.time - lastSlideTimeForJumpBlock < jumpBlockAfterSlideDuration) return;

        Vector3 leftVel = (leftHand.position - lastLeftHandPos) / Time.deltaTime;
        Vector3 rightVel = (rightHand.position - lastRightHandPos) / Time.deltaTime;

        if (leftVel.y > jumpSpeedThreshold && rightVel.y > jumpSpeedThreshold)
        {
            StartCoroutine(JumpRoutine());
            lastJumpTime = Time.time;
            Debug.Log("Jump detected");
        }
    }


    void DetectSlide()
    {
        float headY = headTransform.localPosition.y;

        if (!isSliding && headY < slideThreshold && Time.time - lastSlideTime > slideCooldown)
        {
            isSliding = true;
            lastSlideTime = Time.time;
            lastSlideTimeForJumpBlock = Time.time;
            AudioManager.Instance.PlaySfx(AudioManager.Instance.slideSfx);
            Debug.Log("Slide detected");
        }

        else if (headY > slideThreshold + 0.2f && isSliding)
        {
            isSliding = false;
        }
    }

    void MoveToLine(int line)
    {
        float targetX = (line - 1) * lineDistance;
        Vector3 newPos = new Vector3(targetX, transform.position.y, transform.position.z);
        transform.position = newPos;
        AudioManager.Instance.PlaySfx(AudioManager.Instance.slideSfx);
    }

    IEnumerator JumpRoutine()
    {
        isJumping = true;
        AudioManager.Instance.PlaySfx(AudioManager.Instance.jumpSfx);

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + Vector3.up * jumpHeight;

        float elapsed = 0f;

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

    void UpdateHandPositions()
    {
        lastLeftHandPos = leftHand.position;
        lastRightHandPos = rightHand.position;
    }

    public void EnableMovement()
    {
        isEnabled = true;
    }

    public void DisableMovement()
    {
        isEnabled = false;
    }
}
