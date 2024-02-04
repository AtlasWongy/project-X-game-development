using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        private Rigidbody2D _rb;
        private Vector2 _inputMovement;
        private PlayerMovementController _playerMovementController;

        public float collisionOffset = 0.02f;
        public ContactFilter2D movementFilter;
        List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _playerMovementController = new PlayerMovementController();
        }

        private void OnEnable()
        {
            _playerMovementController.Enable();
            _playerMovementController.Player.Movement.performed += OnMovementPerformed;
            _playerMovementController.Player.Movement.canceled += OnMovementCancelled;
        }

        private void OnDisable()
        {
            _playerMovementController.Disable();
            _playerMovementController.Player.Movement.performed -= OnMovementPerformed;
            _playerMovementController.Player.Movement.canceled -= OnMovementPerformed;
        }

        private void FixedUpdate()
        {
            // Check for Collisions
            int count = _rb.Cast(
               _inputMovement, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
               movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
               castCollisions, // List of collisions to store the found collisions into after the Cast is finished
               speed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset
            if (count == 0)
            {
                _rb.MovePosition(_rb.position + _inputMovement * speed * Time.fixedDeltaTime);
                //_rb.velocity = _inputMovement * speed;
            }
        }

        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            _inputMovement = context.ReadValue<Vector2>();
        }

        private void OnMovementCancelled(InputAction.CallbackContext context)
        {
            _inputMovement = Vector2.zero;
        }
    }
}

