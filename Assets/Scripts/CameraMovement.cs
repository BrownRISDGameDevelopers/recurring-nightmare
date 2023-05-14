using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.GameCenter;

public class CameraMovement : MonoBehaviour
{
    // Max deviation the player can be from center of cam.
    // Expressed as fraction of half of width or height.
    // E.g. if maxDeviationY = 0.4f, then the player will not move more than
    // 40% of the height of the cam away from the cam's center.
    [SerializeField] private float maxDeviationY = 0.4f;
    [SerializeField] private float maxDeviationX = 0.05f;
    [SerializeField] private Vector3 offset = new Vector3(0f, -1f, -5f);

    private Transform _player;
    private Vector2 _cameraBound;
    
    void Start()
    {
        _player = GameManager.Player.transform;
        transform.position = _player.position + offset;
        
        _cameraBound = CalcCameraBound(gameObject.GetComponent<Camera>());
    }

    void LateUpdate()
    {
        var newCamPos = transform.position;
        var deviation = _player.position - newCamPos;
        
        newCamPos.x += CalcMoveAmount(deviation.x, _cameraBound.x);
        newCamPos.y += CalcMoveAmount(deviation.y, _cameraBound.y);

        transform.position = newCamPos;
    }

    private static float CalcMoveAmount(float deviation, float bound)
    {
        if (deviation > bound)
        {
            return deviation - bound;
        }

        if (deviation < -bound)
        {
            return deviation + bound;
        }

        return 0f;
    }

    private Vector2 CalcCameraBound(Camera cam)
    {
        var cameraHeight = cam.orthographicSize;
        var cameraWidth = cameraHeight * cam.aspect;
        
        return new Vector2(cameraWidth * maxDeviationX, cameraHeight * maxDeviationY);
    }
}
