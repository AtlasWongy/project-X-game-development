using System.Collections.Generic;
using UnityEngine;

public class AStarMovement : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    private Rigidbody2D _rb;
    private Vector2 movement;

    private List<Vector2> currentPath;
    private int currentPathIndex = 0;

    public LayerMask obstacleLayer;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        RecalculatePath();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPath != null && currentPathIndex < currentPath.Count)
        {
            Vector2 nextWaypoint = currentPath[currentPathIndex];
            Vector2 direction = nextWaypoint - (Vector2)transform.position;

            if (direction.magnitude < 0.1f)
            {
                currentPathIndex++;
            }

            direction.Normalize();
            movement = direction;
        }
        else
        {
            RecalculatePath();
        }
    }

    private void FixedUpdate()
    {
        moveCharacter(movement);
    }

    void moveCharacter(Vector2 direction)
    {
        _rb.MovePosition((Vector2)transform.position + (direction * speed * Time.fixedDeltaTime));
    }

    void RecalculatePath()
    {
        currentPath = AStarPathfinding.FindPath(transform.position, player.position, obstacleLayer);
        currentPathIndex = 0;
    }
}