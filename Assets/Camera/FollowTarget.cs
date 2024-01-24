using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Camera
{
    public class FollowTarget : MonoBehaviour
    {
        [Header("Transform to Follow")] 
        [SerializeField] private Transform _transform;

        [Header("Configuration")] 
        [SerializeField] private bool followX = true;
        [SerializeField] private bool followY = true;
        [SerializeField] private Vector2 offset = Vector2.zero;

        private Transform _originalTargetTransform;

        private void Start()
        {
            _originalTargetTransform = _transform;
        }
    }
}