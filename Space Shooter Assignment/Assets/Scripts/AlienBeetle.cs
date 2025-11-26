using UnityEngine;

public class AlienBeetle : MonoBehaviour
{
    [Header("Follow Settings")]
    public float followRange = 10f;         // how close the player needs to be
    public float moveSpeed = 3f;            // how fast the beetle moves
    public float stopDistance = 1.5f;       // how close it stops before touching you

    [Header("Attack Settings")]
    public float attackRange = 2f;          // distance to start attack
    public float attackCooldown = 1.5f;     // delay between attacks

    [Header("References")]
    public Animator animator;               // drag in Inspector or auto-find

    private Transform player;
    private float lastAttackTime;
    private bool isDead = false;

    private void Start()
    {
        // Find the player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("NightmareBeetle: No object with tag 'Player' found in scene");
        }

        // Grab Animator automatically if not assigned
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
                Debug.LogWarning("NightmareBeetle: No Animator found on this GameObject");
        }
    }

    private void Update()
    {
        if (isDead) return;
        if (player == null || animator == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // --- MOVEMENT ---

        bool shouldFollow = distance <= followRange && distance > stopDistance;

        if (shouldFollow)
        {
            // Direction toward player, staying on level Y
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0f;

            // Move toward the player
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Face the player (keep beetle upright)
            Vector3 lookPos = player.position;
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos);
        }

        // Tell the Animator whether we're moving
        animator.SetBool("IsMoving", shouldFollow);

        // --- ATTACK ---

        bool inAttackRange = distance <= attackRange;
        bool canAttack = Time.time - lastAttackTime >= attackCooldown;

        if (inAttackRange && canAttack)
        {
            lastAttackTime = Time.time;
            animator.SetBool("IsAttacking", true);
        }
        else
        {
            animator.SetBool("IsAttacking", false);
        }
    }

    // Call this when the beetle dies
    public void Die()
    {
        if (isDead) return;
        isDead = true;

        animator.SetBool("IsDead", true);

        // Stop this script from updating movement/attacks - Optional, figure out if I'll use it or not
        // this.enabled = false;
    }
}
