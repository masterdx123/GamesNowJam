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
    public InputAction dashAction;
    public InputAction teleportAction;
    public InputAction cloneAction;
    [HideInInspector] public Vector2 differenceMouseToPlayerNormalized;
    [HideInInspector] public PlayerStates currentPlayerGameState;

    [SerializeField] private float oxygenTankLevel;
    [SerializeField] private float maxOxygenTankLevel;
    [SerializeField] private float health;
    private bool isInOxygenArea;

    // Dash variables
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    private float dashCooldownTimer;
    private bool isDashing;

    // Teleport variables
    [SerializeField] private Transform teleportTarget;
    [SerializeField] private float teleportCooldown;
    private float teleportCooldownTimer;
    private bool canTeleport;

    // Clone variables
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneCooldown; 
    private float cloneCooldownTimer;
    private bool canClone;

    private void Start()
    {
        moveAction.Enable();
        aimAction.Enable();
        dashAction.Enable();
        teleportAction.Enable();
        cloneAction.Enable();

        currentPlayerGameState = PlayerStates.InGame;
        isInOxygenArea = false;
        maxOxygenTankLevel = 10;
        oxygenTankLevel = 10;
        health = 100;

        dashSpeed = 20f;
        dashDuration = 0.2f;
        dashCooldown = 5f;

        teleportCooldown = 60f;
        cloneCooldown = 120f;

        dashCooldownTimer = dashCooldown;
        teleportCooldownTimer = teleportCooldown;
        cloneCooldownTimer = cloneCooldown;
        canTeleport = true;
        canClone = true;
    }

    private void Update()
    {
        HandleDash();
        HandleTeleport();
        HandleClone();

        Movement();
        SpriteFlip();
        HandleOxygenAndHealth();

        string state = rb.linearVelocity != Vector2.zero ? MOVE : IDLE;
        ChangeAnimationState(state);

        if (health <= 0)
        {
            // Handle player death logic here
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

        Vector2 dashDirection = moveAction.ReadValue<Vector2>().normalized;

        if (dashDirection == Vector2.zero)
        {
            differenceMouseToPlayerNormalized = (Camera.main.ScreenToWorldPoint(aimAction.ReadValue<Vector2>()) - transform.position).normalized;
            dashDirection = differenceMouseToPlayerNormalized;
        }

        rb.linearVelocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
    }

    private void HandleTeleport()
    {
        if (!canTeleport)
        {
            teleportCooldownTimer += Time.deltaTime;
            if (teleportCooldownTimer >= teleportCooldown)
            {
                canTeleport = true;
                teleportCooldownTimer = teleportCooldown;
            }
        }

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

    private void HandleClone()
    {
        if (!canClone)
        {
            cloneCooldownTimer += Time.deltaTime;
            if (cloneCooldownTimer >= cloneCooldown)
            {
                canClone = true;
                cloneCooldownTimer = cloneCooldown;
            }
        }

        if (cloneAction.triggered && canClone && clonePrefab != null)
        {
            SpawnClone();
        }
    }

    private void SpawnClone()
    {
        Instantiate(clonePrefab, transform.position, transform.rotation);
        canClone = false;
        cloneCooldownTimer = 0f;
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
