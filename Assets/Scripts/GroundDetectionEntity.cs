using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class GroundDetectionEntity : MonoBehaviour
{
    [SerializeField] protected float groundDetectionSensitivity = 0.52f;

    private LayerMask _groundMask;
    private Vector2 _sideOffset;
    private float _rayDist;
    
    protected virtual void Awake()
    {
        _groundMask = LayerMask.GetMask("Ground");
        
        Vector2 size = transform.localScale;
        _sideOffset = new Vector2(size.x * 0.5f, 0);
        _rayDist = size.y * groundDetectionSensitivity;
    }
    
    private List<RaycastHit2D> RayToGround()
    {
        Vector2 center = transform.position;
    
        return new List<RaycastHit2D>()
        {
            Physics2D.Raycast(center + _sideOffset, Vector2.down, _rayDist, _groundMask),
            Physics2D.Raycast(center - _sideOffset, Vector2.down, _rayDist, _groundMask)
        };
    }

    protected Tuple<bool, List<RaycastHit2D>> CheckOnGround()
    {
        var groundSurfaces = RayToGround();
        return new Tuple<bool, List<RaycastHit2D>>(groundSurfaces.Any(s => s), groundSurfaces);
    }
}
