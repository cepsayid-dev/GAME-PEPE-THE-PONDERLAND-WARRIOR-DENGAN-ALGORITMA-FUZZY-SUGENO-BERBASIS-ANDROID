using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using TMPro;


public class MeleeEnemy : NPCAIController
{
    [Header("SFX")]
    [SerializeField] AudioClip hurtSfx;
    [SerializeField] AudioClip dedSfx;

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private int maxHealth = 3; // Enemy's max health
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float idleTime = 2f;

    [SerializeField] private bool groundType = true;

    private Transform player;
    protected Rigidbody2D rb;
    private Animator animator;
    private int currentPatrolIndex = 0;
    private bool isGrounded = false;
    [SerializeField] private bool isJumping = false;
    private bool isChasingPlayer = false;
    [SerializeField] private bool isIdle = false;
    private bool isDead = false;
    private float idleTimer = 0f;
    private float lastAttackTime;
    private int currentHealth; // Current health of the enemy

    private MobileControl mobileControl;

    [Header("UI Settings")]
    public TextMeshProUGUI statusText;
    public Vector2 textOffset = new Vector2(0.5f, 1.5f);
    public bool showDebugInfo = true;

    private void Awake()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on the enemy. Add a Rigidbody2D component.");
        }

        if (animator == null)
        {
            Debug.LogError("Animator not found on the enemy. Add an Animator component.");
        }

        currentHealth = maxHealth;
    }

    private void Update()
    {
        UpdateStatusText();
        HandleTextPosition();

        if (player == null || isDead) return;

        isGrounded = groundType ? Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance) : false;
        //isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance);

        DetectPlayer();

        if (isChasingPlayer)
        {
            //ChasePlayer();
            MakeMovementDecission();
        }
        else if (!isIdle)
        {
            Patrol();
        }

        HandleIdleState();

        if (isGrounded && !isJumping && !isIdle && !isDead)
        {
            Jump();
        }

        if(groundType) UpdateWalkingAnimation();
    }

    private void FixedUpdate()
    {
        if (!isDead && !isIdle && !isJumping && !isChasingPlayer)
        {
            MoveTowardsPatrolPoint();
        }
    }

    void UpdateStatusText()
    {
        if (!showDebugInfo)
        {
            statusText.text = "";
            return;
        }

        distanceToPlayer = Vector2.Distance(transform.position, player.position);

        mobileControl = GameObject.FindGameObjectWithTag("Player").GetComponent<MobileControl>();


        string distanceToPlayerText;
        string npcHHealthText;
        string playerDamagePotentialText;
        string walkSpeedText;

        statusText.text = $"<align=right>" +
                         $"<color=#FF0000>Jarak: {distanceToPlayer}\n" +
                         $"<color=#00FFFF>HP: {npcHealth}\n" +
                         $"<color=#00FF00>Dmg: {playerDamagePotential}\n" +
                         $"<color=#FFFF00>Speed: {mobileControl.walkSpeed}";
    }

    void HandleTextPosition()
    {
        if (!statusText) return;

        // Convert enemy position to screen space
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        statusText.rectTransform.position = screenPosition +
                                          new Vector3(textOffset.x, textOffset.y, 0);
    }

    protected void MakeMovementDecission()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.position);


        mobileControl = GameObject.FindGameObjectWithTag("Player").GetComponent<MobileControl>();

        //Debug.Log("distance" + distanceToPlayer);
        //Debug.Log("speed" + mobileControl.walkSpeed);
        float fuzzyOutput = fuzzySystem.Evaluate(
            distanceToPlayer,
            npcHealth,
            playerDamagePotential,
            mobileControl.walkSpeed);

        if (fuzzyOutput > 0f)
        {
            moveTowardsPlayer(Mathf.Abs(fuzzyOutput));
        }
        else
        {
           moveAwayFromPlayer(Mathf.Abs(fuzzyOutput));
        }
    }

    private void moveTowardsPlayer(float intensity)
    {
        Vector2 direction = (player.position - transform.position).normalized;
        direction.Normalize();
        rb.linearVelocity = new Vector2(
            direction.x * npcSpeed,
            groundType ? rb.linearVelocity.y : direction.y * npcSpeed);

        FlipSprite(direction.x);
    }

    private void moveAwayFromPlayer(float intensity)
    {
        //Vector2 tpos = (transform.position.x > patrolPoints[0].position.x) ? patrolPoints[0].position : transform.position;
        //tpos = (transform.position.x < patrolPoints[1].position.x) ? patrolPoints[1].position : transform.position

        Vector2 currentPos = transform.position;
        Vector2 direction = (currentPos - (Vector2)player.position).normalized;

        float minX = Mathf.Infinity;
        float maxX = -Mathf.Infinity;
        float minY = Mathf.Infinity;
        float maxY = -Mathf.Infinity;

        foreach (var point in patrolPoints)
        {
            Vector2 pos = point.position;
            minX = Mathf.Min(minX, pos.x);
            maxX = Mathf.Max(maxX, pos.x);
            minY = Mathf.Min(minY, pos.y);
            maxY = Mathf.Max(maxY, pos.y);
        }

        Vector2 target = new Vector2(
            direction.x > 0 ? maxX : minX,
            direction.y > 0 ? maxY : minY
        );

        Vector2 moveThreshold = (target - currentPos).normalized;
        moveThreshold.Normalize();
        rb.linearVelocity = new Vector2(
            moveThreshold.x * npcSpeed,
            groundType ? rb.linearVelocity.y : direction.y * npcSpeed);

        FlipSprite(moveThreshold.x);
    }

    private void Patrol()
    {
        Vector2 direction = patrolPoints[currentPatrolIndex].position - transform.position;
        direction.Normalize();
        rb.linearVelocity = new Vector2(direction.x * npcSpeed, groundType ? rb.linearVelocity.y : direction.y * npcSpeed);

        FlipSprite(direction.x);
        //Debug.Log("X: " + rb.position);
        //Debug.Log("XX: " + (Vector2)patrolPoints[currentPatrolIndex].position);
        //Debug.Log("XXX: " + Vector2.Distance(transform.position, (Vector2)patrolPoints[currentPatrolIndex].position));
        if (Vector2.Distance(transform.position, patrolPoints[currentPatrolIndex].position) < 0.1f)
        {
            //Debug.Log("stopped");
            rb.linearVelocity = Vector2.zero;
            isIdle = true;
        }
    }

    private void HandleIdleState()
    {
        if (isIdle)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleTime)
            {
                idleTimer = 0f;
                isIdle = false;
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }
        }
    }

    private void MoveTowardsPatrolPoint()
    {
        Vector2 direction = patrolPoints[currentPatrolIndex].position - transform.position;
        direction.Normalize();
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, groundType ? rb.linearVelocity.y : direction.y * npcSpeed);

        FlipSprite(direction.x);
    }

    private void DetectPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            isChasingPlayer = true;
        }
        else
        {
            isChasingPlayer = false;
        }
    }

    private void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, groundType ? rb.linearVelocity.y : direction.y * npcSpeed);

        FlipSprite(direction.x);
    }

    private void Jump()
    {
        isJumping = true;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            HandlePlayerCollision(collision);
        }
    }

    private void HandlePlayerCollision(Collision2D collision)
    {
        Vector2 collisionPoint = collision.GetContact(0).point;
        float topOfEnemy = transform.position.y + GetComponent<Collider2D>().bounds.extents.y;

        if (collisionPoint.y > topOfEnemy - 0.1f)
        {
            TakeDamage(1); // Enemy takes 1 damage when hit by the player from above
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, jumpForce); // Bounce the player up
        }
        else
        {
            Damageable playerDamageable = collision.gameObject.GetComponent<Damageable>();
            if (playerDamageable != null)
            {
                playerDamageable.TakeDamage(damageAmount); // Enemy damages the player
            }
        }
    }

    private void FlipSprite(float direction)
    {
        transform.localScale = new Vector3(direction < 0 ? 1 : -1, 1, 1);
    }
    private void UpdateWalkingAnimation()
    {
        if (groundType) // Ground enemy animation
        {
            bool isWalking = Mathf.Abs(rb.linearVelocity.x) > 0.1f && isGrounded && !isIdle && !isDead;
            animator.SetBool("walking", isWalking);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        Debug.Log("Take {}");

        currentHealth -= damage;
        animator.SetTrigger("hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        animator.SetTrigger("death");

        // Optionally destroy the enemy after the death animation
        Destroy(gameObject, 0.5f);


    }
}