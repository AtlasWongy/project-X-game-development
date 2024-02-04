using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    private Rigidbody2D _rb;
    private Vector2 movement;

    public float collisionOffset = 0.02f;
    public ContactFilter2D movementFilter;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();


    // Start is called before the first frame update
    void Start()
    {
        _rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _rb.rotation = angle;
        direction.Normalize();
        movement = direction;
    }
    private void FixedUpdate()
    {
        // Check for Collisions
        int count = _rb.Cast(
           movement, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
           movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
           castCollisions, // List of collisions to store the found collisions into after the Cast is finished
           speed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset
        if (count == 0)
        {
            moveCharacter(movement);
        }
    }

    void moveCharacter(Vector2 direction)
    {
        _rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
    }
}

