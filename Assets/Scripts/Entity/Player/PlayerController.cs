using System.Collections;
using Enums;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public const float BASE_SPEED = 7;

    public SpriteRenderer spriteRenderer;
    public Animator animator;
    private string currentState;

    public const string IDLE = "Idle";
    public const string MOVE = "Move";

    public InputAction moveAction;
    public InputAction aimAction;
    public InputAction dashAction; // Dash action input
    public InputAction teleportAction; // Teleport action input
    [HideInInspector] public Vector2 differenceMouseToPlayerNormalized;
    [HideInInspector] public PlayerStates currentPlayerGameState;

    [SerializeField] private float oxygenTankLevel;
    [SerializeField] private float maxOxygenTankLevel;
    [SerializeField] private float health;
    private bool isInOxygenArea;

    // Dash variables
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 5f;
    private float dashCooldownTimer;
    private bool isDashing;

    // Teleport variables
    [SerializeField] private Transform teleportTarget; // Set this in the Inspector to the target teleport location
    [SerializeField] private float teleportCooldown = 60f;
    private float teleportCooldownTimer;
    private bool canTeleport;

    private void Start()
    {
        moveAction.Enable();
        aimAction.Enable();
        dashAction.Enable();
        teleportAction.Enable();

        currentPlayerGameState = PlayerStates.InGame;
        isInOxygenArea = false;
        maxOxygenTankLevel = 10;
        oxygenTankLevel = 10;
        health = 100;

        dashCooldownTimer = dashCooldown;
        teleportCooldownTimer = teleportCooldown;
        canTeleport = true;
    }

    private void Update()
    {
        HandleDash();
        HandleTeleport();

        Movement();
        SpriteFlip();
        HandleOxygenAndHealth();

        string state = rb.linearVelocity != Vector2.zero ? MOVE : IDLE;
        ChangeAnimationState(state);

        if (health <= 0)
        {
            // Handle player death logic here (e.g., Game Over, respawn, etc.)
        }
    }

    private void Movement()
    {
        if (!isDashing)
        {
            var moveDirection = moveAction.ReadValue<Vector2>().normalized;
            rb.linearVelocity = moveDirection * BASE_SPEED;
        }
    }

    private void HandleDash()
    {
        dashCooldownTimer += Time.deltaTime;

        if (dashAction.triggered && dashCooldownTimer >= dashCooldown && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        dashCooldownTimer = 0f;

        Vector2 dashDirection = rb.linearVelocity.normalized;
        rb.linearVelocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
    }

    private void HandleTeleport()
    {
        // Update teleport cooldown timer
        if (!canTeleport)
        {
            teleportCooldownTimer += Time.deltaTime;
            if (teleportCooldownTimer >= teleportCooldown)
            {
                canTeleport = true;
                teleportCooldownTimer = teleportCooldown;
            }
        }

        // Check for teleport action and cooldown
        if (teleportAction.triggered && canTeleport && teleportTarget != null)
        {
            Teleport();
        }
    }

    private void Teleport()
    {
        transform.position = teleportTarget.position;
        canTeleport = false;
        teleportCooldownTimer = 0f;
    }

    private void SpriteFlip()
    {
        differenceMouseToPlayerNormalized = (Camera.main.ScreenToWorldPoint(aimAction.ReadValue<Vector2>()) - transform.position).normalized;
        spriteRenderer.flipX = differenceMouseToPlayerNormalized.x < 0;
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState != newState)
        {
            animator.Play(newState, 0, 0f);
            currentState = newState;
        }
    }

    private void HandleOxygenAndHealth()
    {
        if (isInOxygenArea)
        {
            oxygenTankLevel = Mathf.Min(maxOxygenTankLevel, oxygenTankLevel + Time.deltaTime);
        }
        else
        {
            oxygenTankLevel = Mathf.Max(0, oxygenTankLevel - Time.deltaTime);
            if (oxygenTankLevel <= 0)
            {
                health = Mathf.Max(0, health - 10 * Time.deltaTime);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("OxygenArea"))
        {
            isInOxygenArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("OxygenArea"))
        {
            isInOxygenArea = false;
        }
    }
}
