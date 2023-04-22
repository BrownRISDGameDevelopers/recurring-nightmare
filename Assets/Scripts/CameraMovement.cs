using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class CameraMovement : MonoBehaviour
{
    // NOTE: This is pretty messy and needs to be cleaned up.
    
    [SerializeField] private Camera cam;
    
    // Type of camera movement.
    // stationary camera doesn't move.
    // centered camera stays centered on player.
    // mario camera won't move until player's deviation reaches a certain value.
    public enum CameraType { stationary, centered, mario };
    [SerializeField] private CameraType cameraType = CameraType.mario;
    
    // Max deviation the player can be from center of cam.
    // Expressed as fraction of half of width or height.
    // E.g. if maxDeviationY = 0.4f, then the player will not move more than
    // 40% of the height of the cam away from the cam's center.
    [SerializeField] private float maxDeviationY = 0.4f;
    [SerializeField] private float maxDeviationX = 0.05f;
    [SerializeField] private Vector2 minViewPortSize = new Vector2(-30.0f, -5.0f);
    [SerializeField] private Vector2 maxViewPortSize = new Vector2(30.0f, 25.0f);

    private float _cameraHeight;
    private float _cameraWidth;
    
    private float _cameraBoundY;
    private float _cameraBoundX;

    private Transform _playerTransform;
    
void Start()
{
    _playerTransform = GameManager.Player.transform;
        
    _cameraHeight = cam.orthographicSize;
    _cameraWidth = _cameraHeight * cam.aspect;
    _cameraBoundY = _cameraHeight * maxDeviationY;
    _cameraBoundX = _cameraWidth * maxDeviationX;
}

    void LateUpdate()
    {
        // Call the update function corresponding to what CameraType the cam is.
        switch (cameraType)
        {
            case CameraType.mario: cameraUpdateMario(); break;
            case CameraType.centered: cameraUpdateCentered(); break;
            case CameraType.stationary: cameraUpdateStationary(); break;
            default: Debug.Log("cameraType not set!"); break;
        }
    }

    void cameraUpdateMario()
    {
        Vector3 newCameraPosition = new Vector3(cam.transform.position.x, cam.transform.position.y,
            cam.transform.position.z);

        // Calculate difference between player and camera position.
        float deviationY = _playerTransform.position.y - cam.transform.position.y;
        float deviationX = _playerTransform.position.x - cam.transform.position.x;
        
        // If player is out of camera bound,
        // correct cam so that player is just on the bound.
        if (deviationY > _cameraBoundY) {
            newCameraPosition.y += deviationY - _cameraBoundY;
        } else if (deviationY < -_cameraBoundY) {
            newCameraPosition.y += deviationY + _cameraBoundY;
        }
        if (deviationX > _cameraBoundX) {
            newCameraPosition.x += deviationX - _cameraBoundX;
        } else if (deviationX < -_cameraBoundX) {
            newCameraPosition.x += deviationX + _cameraBoundX;
        }
        
        if(newCameraPosition.y < minViewPortSize.y) {
            newCameraPosition.y = minViewPortSize.y;
        } else if(newCameraPosition.y > maxViewPortSize.y) {
            newCameraPosition.y = maxViewPortSize.y;
        }

        if (newCameraPosition.x < minViewPortSize.x) {
            newCameraPosition.x = minViewPortSize.x;
        }
        else if (newCameraPosition.x > maxViewPortSize.x) {
            newCameraPosition.x = maxViewPortSize.x;
        }

        cam.transform.position = newCameraPosition;
    }

    void cameraUpdateCentered()
    {
        // Move camera to player's position, but keep original z value.
        Vector3 newCameraPosition = new Vector3(_playerTransform.position.x, _playerTransform.position.y,
            cam.transform.position.z);

        if (newCameraPosition.y < minViewPortSize.y)
        {
            newCameraPosition.y = minViewPortSize.y;
        }
        else if (newCameraPosition.y > maxViewPortSize.y)
        {
            newCameraPosition.y = maxViewPortSize.y;
        }

        if (newCameraPosition.x < minViewPortSize.x)
        {
            newCameraPosition.x = minViewPortSize.x;
        }
        else if (newCameraPosition.x > maxViewPortSize.x)
        {
            newCameraPosition.x = maxViewPortSize.x;
        }

        cam.transform.position = newCameraPosition;
    }
    
    void cameraUpdateStationary()
    {
        // Leave camera where it is.
    }

    public void changeCameraType(CameraType newType)
    {
        cameraType = newType;
    }
}
