using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class MobileControl : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float jumpImpulse = 10f;
    public float fallMultiplier = 3f; // Increases gravity when falling
    public float groundDeceleration = 20f;
    public float airDeceleration = 10f;
    public float maxFallSpeed = 20f;
    public float jumpEndEarlyGravityModifier = 2f;

    [Header("SFX")]
    [SerializeField] AudioClip jumpSfx;

    private Vector2 moveInput;
    private Vector2 _velocity;

    private bool _isMoving = false;
    private bool _jumpToConsume;
    private bool _endedJumpEarly;
    private bool _isFacingRight = true;

    private Rigidbody2D rb;
    private Animator animator;
    private TouchingDirection touchingDirection;

    public bool IsMoving
    {
        get => _isMoving;
        private set
        {
            _isMoving = value;
            animator.SetBool("isMoving", value);
        }
    }

    public bool IsFacingRight
    {
        get => _isFacingRight;
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            _isFacingRight = value;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirection = GetComponent<TouchingDirection>();
    }

    private void FixedUpdate()
    {
        //Debug.Log($"Velocity Y: {_velocity.y}, IsGrounded: {touchingDirection.IsGrounded}, JumpToConsume: {_jumpToConsume}");

        HandleHorizontalMovement();
        HandleGravity();
        ApplyMovement();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;
        SetFacingDirection(moveInput);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //Debug.Log("OnJump triggered");

        if (context.started)
        {
            if (touchingDirection == null || !touchingDirection.IsGrounded)
            {
               return;
            }

            animator.SetTrigger("Jump");
            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlaySound(jumpSfx);
            }
            
            _jumpToConsume = true;
        }

        if (context.canceled && rb != null && rb.linearVelocity.y > 0)
        {
            _endedJumpEarly = true;
        }
    }

    private void HandleHorizontalMovement()
    {
        if (moveInput.x == 0) // No horizontal input
        {
            if (touchingDirection.IsGrounded) // Stop immediately if grounded
            {
                _velocity.x = 0;
            }
            else // Apply slower deceleration if airborne
            {
                _velocity.x = Mathf.MoveTowards(_velocity.x, 0, airDeceleration * Time.fixedDeltaTime);
            }
        }
        else // Player is moving
        {
            _velocity.x = moveInput.x * walkSpeed; // Maintain constant movement speed
        }
    }

    private void HandleGravity()
    {
        if (touchingDirection.IsGrounded && _velocity.y <= 0f)
        {
            //Debug.Log("Player is grounded. Stopping vertical velocity.");
            _velocity.y = 0f;
        }
        else
        {
            float gravityMultiplier = touchingDirection.IsGrounded ? 1f : fallMultiplier;
            if (_endedJumpEarly && _velocity.y > 0)
                gravityMultiplier *= jumpEndEarlyGravityModifier;

            _velocity.y = Mathf.MoveTowards(_velocity.y, -maxFallSpeed, gravityMultiplier * Mathf.Abs(Physics2D.gravity.y) * Time.fixedDeltaTime);
            //Debug.Log($"Applying Gravity: Velocity Y: {_velocity.y}");
        }
    }
    private void ApplyMovement()
    {
        if (_jumpToConsume)
        {
            ExecuteJump();
            _jumpToConsume = false;
        }
        //Debug.Log($"Applying Movement: Velocity: {_velocity}");
        rb.linearVelocity = _velocity;
    }

    private void ExecuteJump()
    {
        //Debug.Log("Jump Executed!");
        _endedJumpEarly = false;
        _velocity.y = jumpImpulse;
    }

    private void SetFacingDirection(Vector2 input)
    {
        if (input.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (input.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }
}