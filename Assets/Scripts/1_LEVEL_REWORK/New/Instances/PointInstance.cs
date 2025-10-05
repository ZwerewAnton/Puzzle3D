﻿using _1_LEVEL_REWORK.New.Data;
using UnityEngine;

namespace _1_LEVEL_REWORK.New.Instances
{
    public class PointInstance
    {
        private readonly PointData _data;
        public bool IsInstalled { get; private set; }
        public Vector3 Position => _data.position;
        public Quaternion Rotation => Quaternion.Euler(_data.rotation);

        public PointInstance(PointData data, bool isInstalled = false)
        {
            _data = data;
            IsInstalled = isInstalled;
        }
        
        public void Install() => IsInstalled = true;
        public void Uninstall() => IsInstalled = false;
    }
}