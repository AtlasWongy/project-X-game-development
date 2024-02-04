using System.Collections.Generic;
using UnityEngine;


public class BypassObstaclesMovement : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    private Rigidbody2D _rb;

    public float avoidanceRadius = 2f;
    public float playerAvoidanceRadius = 1f; // Distance to keep from the player
    public float enemyAvoidanceRadius = 1.5f; // Distance to keep from other enemies
    public LayerMask obstacleLayer;
    public LayerMask enemyLayer;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        // Apply steering behavior for obstacle and enemy avoidance
        Vector2 avoidanceVector = AvoidObstacles();
        Vector2 enemyAvoidanceVector = AvoidOtherEnemies();
        Vector2 desiredDirection = direction + avoidanceVector + enemyAvoidanceVector;

        // Smoothly interpolate the current direction towards the desired direction
        Vector2 currentDirection = _rb.velocity.normalized;
        Vector2 newDirection = Vector2.Lerp(currentDirection, desiredDirection.normalized, Time.deltaTime * 5f);

        // Apply the new direction to the rigidbody velocity
        _rb.velocity = newDirection * speed;

        // Rotate the enemy to face the player
        float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
        _rb.rotation = angle;
    }

    Vector2 AvoidObstacles()
    {
        Vector2 avoidanceVector = Vector2.zero;

        // Define rays in multiple directions around the enemy
        Vector2[] rayDirections = { Vector2.up, Vector2.down, Vector2.left, Vector2.right, Vector2.up + Vector2.left, Vector2.up + Vector2.right, Vector2.down + Vector2.left, Vector2.down + Vector2.right };

        foreach (Vector2 direction in rayDirections)
        {
            // Perform raycast to detect obstacles
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, avoidanceRadius, obstacleLayer);

            if (hit.collider != null && hit.collider.transform != player)
            {
                // Calculate avoidance strength based on obstacle size
                float obstacleSizeFactor = Mathf.Clamp01(1f - hit.collider.bounds.size.magnitude / avoidanceRadius);

                // Adjust avoidance vector based on obstacle size
                avoidanceVector += (hit.normal * obstacleSizeFactor).normalized;
            }
        }

        return avoidanceVector.normalized;
    }

    Vector2 AvoidOtherEnemies()
    {
        Vector2 enemyAvoidanceVector = Vector2.zero;

        // Detect other enemies within the specified radius
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, enemyAvoidanceRadius, enemyLayer);

        foreach (var enemy in enemies)
        {
            if (enemy.transform != transform)
            {
                // Calculate avoidance strength based on enemy distance
                float enemyDistanceFactor = Mathf.Clamp01(1f - Vector2.Distance(transform.position, enemy.transform.position) / enemyAvoidanceRadius);

                // Adjust avoidance vector based on enemy distance
                enemyAvoidanceVector += ((Vector2)(transform.position - enemy.transform.position) * enemyDistanceFactor).normalized;
            }
        }

        return enemyAvoidanceVector.normalized;
    }
}