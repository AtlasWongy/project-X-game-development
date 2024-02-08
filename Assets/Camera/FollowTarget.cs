using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraTarget
{
    public class FollowTarget : MonoBehaviour
    {
        [Header("Transform to Follow")] 
        [SerializeField]
        private Transform targetTransform;

        [Header("Configuration")] 
        [SerializeField] private bool followX = true;
        [SerializeField] private bool followY = true;
        [SerializeField] private Vector2 offset = Vector2.zero;

        private Transform _originalTargetTransform;

        private void Start()
        {
            _originalTargetTransform = targetTransform;
        }

        private void LateUpdate()
        {
            var position = this.transform.position;
            var newPosX = position.x;
            var newPosY = position.y;
            if (followX)
            {
                newPosX = targetTransform.position.x + offset.x;
            }

            if (followY)
            {
                newPosY = targetTransform.position.y + offset.y;
            }

            var transform1 = this.transform;
            transform1.position = new Vector3(newPosX, newPosY, transform1.position.z);
        }
    }
}