﻿using Unity.Entities;
using Unity.Mathematics;
 using UnityEngine;

 namespace Game.Component
{
    public struct Movement : IComponentData
    {
        public int2 TargetPosition;
        public int2 CurrentPosition;

        public float Speed;
        public float2 Position;

        public bool Reached(int2 coord)
        {
            var vec = coord - Position;
            return Mathf.Sqrt((vec.x*vec.x) + (vec.y*vec.y)) < .1f;
        }
    }
}