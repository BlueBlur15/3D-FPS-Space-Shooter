using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMonster : MonoBehaviour
{
    [Header("Follow Settings")]
    public float followRange = 10f;         // how close the player needs to be
    public float moveSpeed = 3f;            // how fast the cube moves
    public float stopDistance = 0.2f;         // how close it stops before bumping into you

    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        // Find the player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("CubeMonster: No player found in scene! Make sure Player is tagged with 'Player'.");
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        // Measure distance
        float distance = Vector3.Distance(transform.position, player.position);

        // Only move if within follow range and not too close
        if (distance <= followRange && distance > stopDistance)
        {
            // Direction to player (ignore vertical if you want ground-only)
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;    // prevent floating up/down

            // Move smoothly toward the player
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Face the player
            transform.LookAt(player.position);
        }
    }
}
