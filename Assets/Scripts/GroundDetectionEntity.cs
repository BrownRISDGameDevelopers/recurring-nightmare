using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class GroundDetectionEntity : MonoBehaviour
{
    [SerializeField] protected float groundDetectionSensitivity = 1e-3f;

    protected LayerMask GroundMask;
    private Vector2 _sideOffset;
    private Vector2 _heightOffset;
    
    protected virtual void Awake()
    {
        GroundMask = LayerMask.GetMask("Ground");
        
        Vector2 size = transform.localScale;
        _sideOffset = new Vector2(size.x * 0.45f, 0);
        _heightOffset = new Vector2(0, size.y * 0.5f);
    }
    
    private List<RaycastHit2D> RayToGround()
    {
        Vector2 center = transform.position;
        Vector2 bottom = center - _heightOffset;
    
        return new List<RaycastHit2D>()
        {
            Physics2D.Raycast(bottom + _sideOffset, Vector2.down, groundDetectionSensitivity, GroundMask),
            Physics2D.Raycast(bottom - _sideOffset, Vector2.down, groundDetectionSensitivity, GroundMask),
            Physics2D.Raycast(bottom, Vector2.down, groundDetectionSensitivity, GroundMask)
        };
    }

    /// <summary>
    /// Checks what surface the entity is on, if any
    /// </summary>
    /// <returns>
    /// A Tuple containing the following:
    /// bool containing whether it is on anything at all and a
    /// List of hits
    /// </returns>
    protected Tuple<bool, List<RaycastHit2D>> CheckOnGround()
    {
        var groundSurfaces = RayToGround();
        int hitCount = RayToGround().Count(hit2D => hit2D == true);
        return new Tuple<bool, List<RaycastHit2D>>(hitCount > 0, groundSurfaces);
    }
}
