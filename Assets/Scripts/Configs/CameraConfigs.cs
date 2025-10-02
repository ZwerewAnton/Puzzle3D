﻿using System;
using UnityEngine;

namespace Configs
{
    [Serializable]
    public class CameraConfigs
    {
        [Header("Orbit Settings")]
        public Quaternion startRotation;
        public float distance = 30f;
        public float minDistance = 20f;
        public float maxDistance = 40f;
        
        public float xSpeed = 300f;
        public float ySpeed = 300f;

        public float yMinLimit = 5f;
        public float yMaxLimit = 80f;

        [Header("Offset Settings")]
        public Vector3 targetOffset = Vector3.zero;
        
        [Header("Zoom Settings")]
        public float zoomSpeed = 1f;
        
        [Header("Smooth Settings")]
        public float mouseSmooth = 10f;
        public float zoomDampening = 5f;
    }
}