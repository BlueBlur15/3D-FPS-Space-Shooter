using UnityEngine;

public class AlienBeetle : MonoBehaviour
{
    [Header("Follow Settings")]
    public float followRange = 10f;      // how close the player needs to be
    public float moveSpeed = 3f;         // how fast the beetle moves
    public float stopDistance = 1.5f;    // how close it stops before touching you

    [Header("Attack Settings")]
    public float attackCooldown = 1.5f;  // time between attack animations

    [Header("References")]
    public Animator animator;            // assign in Inspector or auto-find

    [Header("Visual")]
    public bool flipForward = false;     // set true if the model is built facing "backwards"

    private Transform player;
    private float lastAttackTime = -999f;
    private bool isDead = false;

    private void Start()
    {
        // Find the player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("AlienBeetle: No object with tag 'Player' found in scene");
        }

        // Grab Animator automatically if not assigned
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogWarning("AlienBeetle: No Animator found on this GameObject");
            }
        }
    }

    private void Update()
    {
        if (isDead) return;
        if (player == null || animator == null) return;

        // Direction (flat on Y) from beetle to player
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;

        float distance = toPlayer.magnitude;

        // --- MOVEMENT ---
        bool shouldFollow = distance <= followRange && distance > stopDistance;

        if (shouldFollow && distance > 0.001f)
        {
            // Normalize direction
            Vector3 direction = toPlayer / distance;

            // Move toward the player
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Face the player
            Quaternion targetRot = Quaternion.LookRotation(direction, Vector3.up);

            // If this model is authored facing the opposite way, flip 180 degrees
            if (flipForward)
            {
                targetRot *= Quaternion.Euler(0f, 180f, 0f);
            }

            transform.rotation = targetRot;
        }

        // Tell the Animator whether we're moving
        animator.SetBool("IsMoving", shouldFollow);
    }

    // Called by HurtZone when it damages the player
    public void TryPlayAttack()
    {
        if (isDead || animator == null) return;

        if (Time.time - lastAttackTime < attackCooldown)
            return; // too soon, don't restart animation

        lastAttackTime = Time.time;
        animator.SetTrigger("Attack");   // Animator must have a Trigger called "Attack"
    }

    // Called by EnemyHealth when HP reaches 0
    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // 🔹 Tell the level manager that an enemy has been killed
        if (LevelEnemyManager.instance != null)
        {
            LevelEnemyManager.instance.EnemyKilled();
        }

        if (animator == null)
            animator = GetComponent<Animator>();

        Debug.Log("AlienBeetle.Die(): forcing death state");

        // Stop run/idles from fighting the state machine
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsDead", true);    // optional, but fine to keep

        // Force-play the 'death' state on Base Layer (layer 0)
        animator.CrossFade("death", 0.05f, 0);
        // or: animator.Play("death", 0, 0f);

        // 🔹 Optional but nice: stop collisions/AI and destroy after anim
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        this.enabled = false; // stop Update / AI logic on this script

        // Destroy after ~2 seconds (tweak to match your death anim length)
        Destroy(gameObject, 2f);
    }
}
