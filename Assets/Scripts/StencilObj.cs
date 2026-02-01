using System;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.LowLevelPhysics2D;

[Serializable]
public class StencilObj
{
    public float x => position.x;
    public float y => position.y;

    public PhysicsAABB AABB => new PhysicsAABB(bl, tr);
    public Vector2 position;
    public Vector2 bl;
    public Vector2 tr;
    public Vector3 worldPos;
    public int scoreGiven;
    
    public StencilScobj data;
}