using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public bool IsEmpty { get; private set; }
    
    private float _healValue;
    private PlayerHealth _playerHealth;
    private Image _image;

    private void Start()
    {
        _healValue = float.NaN;
        IsEmpty = true;
        _playerHealth = GameManager.Player.GetComponent<PlayerHealth>();
        _image = gameObject.GetComponent<Image>();
    }

    public void Fill(float healValue, Sprite sprite)
    {
        IsEmpty = false;
        _healValue = healValue;
        _image.sprite = sprite;
        _image.enabled = true;
        Debug.Log("Fill");
    }

    public void Use()
    {
        // If the player is at full health, don't heal them
        if (!_playerHealth.Heal(_healValue)) return;
        
        _healValue = float.NaN;
        IsEmpty = true;
        _image.enabled = false;
    }
}
