using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerMovement : MonoBehaviour
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
    [HideInInspector] public Vector2 differenceMouseToPlayerNormalized;

    void Start()
    {
        moveAction.Enable();
        aimAction.Enable();
    }

    void Update()
    {
        Movement();
        SpriteFlip();
        
        string state = rb.linearVelocity != Vector2.zero ? MOVE : IDLE;
        ChangeAnimationState(state);
    }

    void Movement()
    {
        var moveDirection = moveAction.ReadValue<Vector2>().normalized;
        rb.linearVelocity = moveDirection * BASE_SPEED;
    }

    void SpriteFlip()
    {
        var normalizedDifferenceX = 0f;
        differenceMouseToPlayerNormalized = (Camera.main.ScreenToWorldPoint(aimAction.ReadValue<Vector2>()) - transform.position).normalized;

        if (differenceMouseToPlayerNormalized.x != 0) normalizedDifferenceX = Mathf.Abs(differenceMouseToPlayerNormalized.x) / differenceMouseToPlayerNormalized.x;

        spriteRenderer.flipX = normalizedDifferenceX != 1;
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState != newState)
        {
            animator.Play(newState, 0, 0f);

            currentState = newState;
        }
    }
}
