using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private new Camera camera;
        private PlayerMovementController _playerMovementController;
        private Vector2 _mousePosition;
        private Transform _transform;
        private bool _facingRight = true;

        private void Awake()
        {
            Cursor.visible = true;
            _playerMovementController = new PlayerMovementController();
            _transform = transform.parent.GetComponentInParent<Transform>();
        }
        
        private void OnEnable()
        {
            _playerMovementController.Enable();
            _playerMovementController.Player.Combat.performed += OnPerformedGetMousePosition;
        }

        private void OnPerformedGetMousePosition(InputAction.CallbackContext context)
        {
            var localMousePosition = context.ReadValue<Vector2>();
            var localMousePosition3d = new Vector3(
                localMousePosition.x, localMousePosition.y, -10.0f);
            Vector3 globalMousePosition = camera.ScreenToWorldPoint(localMousePosition3d);

            if (_transform.position.x < globalMousePosition.x && !_facingRight)
            {
                _facingRight = true;
                _transform.localScale = new Vector3(5.0f, 5.0f, 1.0f);
            }
            else if (_transform.position.x > globalMousePosition.x && _facingRight)
            {
                _facingRight = false;
                _transform.localScale = new Vector3(-5.0f, 5.0f, 1.0f);
            }
            
        }
    }
}




