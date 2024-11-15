using System.Collections;
using Enums;
using Interfaces;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable
{
    public delegate void StatChanged(float currValue, float maxValue);
    public delegate void PlayerDeath();
    
    public event StatChanged OnHealthChanged;
    public event StatChanged OnOxygenChanged;
    public event PlayerDeath OnPlayerDeath;
    
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

    [SerializeField] private float oxygenTankLevel = 10.0f;
    [SerializeField] private float maxOxygenTankLevel = 10.0f;
    [SerializeField] private float oxygenTankDecaySpeed = 1.0f;
    [SerializeField] private float oxygenTankGainSpeed = 1.0f;
    [SerializeField] private float health = 100.0f;
    [SerializeField] private float maxHealth = 100.0f;
    private bool isInOxygenArea;
    private bool _isDead;

    // Dash variables
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 5f;
    private float dashCooldownTimer;
    private bool isDashing;

    // Teleport variables
    [SerializeField] private Transform teleportTarget;
    [SerializeField] private float teleportCooldown = 60f;
    private float teleportCooldownTimer;
    private bool canTeleport;

    // Clone variables
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneCooldown = 120f; 
    private float cloneCooldownTimer;
    private bool canClone;
    [SerializeField] private float cloneTimer = 30f;

    private void Start()
    {
        moveAction.Enable();
        aimAction.Enable();
        dashAction.Enable();
        teleportAction.Enable();
        cloneAction.Enable();

        currentPlayerGameState = PlayerStates.InGame;
        isInOxygenArea = false;
        oxygenTankLevel = maxOxygenTankLevel;
        health = maxHealth;

        dashCooldownTimer = dashCooldown;
        teleportCooldownTimer = teleportCooldown;
        cloneCooldownTimer = cloneCooldown;
        canTeleport = true;
        canClone = true;
        _isDead = false;
    }

    private void Update()
    {
        if (_isDead) return;
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
            Die();
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
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y - 2, transform.position.z);
        GameObject clone = Instantiate(clonePrefab, spawnPosition, transform.rotation);
        Destroy(clone, cloneTimer); // Destroy the clone after 30 seconds
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
            oxygenTankLevel = Mathf.Min(maxOxygenTankLevel, oxygenTankLevel + (Time.deltaTime * oxygenTankGainSpeed));
        }
        else
        {
            oxygenTankLevel = Mathf.Max(0, oxygenTankLevel - (Time.deltaTime * oxygenTankDecaySpeed));
            if (oxygenTankLevel <= 0)
            {
                health = Mathf.Max(0, health - 10 * Time.deltaTime);
            }
        }
        OnHealthChanged?.Invoke(health, maxHealth);
        OnOxygenChanged?.Invoke(oxygenTankLevel, maxOxygenTankLevel);
    }

    private void Die()
    {
        _isDead = true;
        isDashing = false;
        canTeleport = false;
        rb.linearVelocity = Vector2.zero;
        ChangeAnimationState(IDLE);
        OnPlayerDeath?.Invoke();
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

    public void TakeDamage(float damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);

        if (health <= 0)
        {
            Die();
        }
    }
}
