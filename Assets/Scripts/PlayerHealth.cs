using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHealth : MonoBehaviour
{
	[SerializeField] private GameHandler gameHandler;
	[SerializeField] private Slider hpSlider;
	
	[Header("Player Health")] 
	[SerializeField] private float maxPlayerHealth = 10f;
	[SerializeField] private float immuneTimeAtGameStart = 3f;
	[SerializeField] private float immuneTimeAtDamage = 3f;
	
	private float _remainingImmuneTime;
	private bool _isImmune;
	private float _playerHealth;
	
    // Start is called before the first frame update
    void Start()
    {
	    MakeImmune(immuneTimeAtGameStart);
	    // Player starts off immune. We can change this.
        _playerHealth = maxPlayerHealth;
        hpSlider.maxValue = maxPlayerHealth;
        hpSlider.value = maxPlayerHealth;
    }

    void Update()
    {
	    hpSlider.value = _playerHealth;
      
	    if (!_isImmune) return;
	    
	    _remainingImmuneTime -= Time.deltaTime;
	    if (_remainingImmuneTime <= 0)
	    {
		    _remainingImmuneTime = 0;
		    _isImmune = false;
		    Debug.Log("Player is no longer immune.");
		    /*
			    // This is if we want to have different layers to allow the player immunity
			    // from collisions with the enemy.
			    int LayerPlayerDefault = LayerMask.NameToLayer("PlayerDefault");
			    gameObject.layer = LayerPlayerDefault;
			    */
	    }
    }

    private void MakeImmune(float duration)
    {
	    _isImmune = true;
	    _remainingImmuneTime = duration;
	    Debug.Log("Player is now immune.");
	    /*
	    // We can use something like this to temporarily set the player
	    // to a layer where they won't collide with the enemy.
	    int LayerPlayerImmune = LayerMask.NameToLayer("PlayerImmune");
	    gameObject.layer = LayerPlayerImmune;
	    Debug.Log(LayerPlayerImmune);
	    */
    }

	public void Damage(float damageAmount, bool makeImmune = false, bool damageAnyway = false)
	{
		if (_isImmune && !damageAnyway) return;
		
		_playerHealth -= damageAmount;
		if (_playerHealth <= 0)
		{
			gameHandler.EndGame("Game Over");
			_playerHealth = 0;
		}
		Debug.Log("Player damaged. Remaining health: " + _playerHealth);

		if (makeImmune)
		{
			MakeImmune(immuneTimeAtDamage);
		}
	}
	
	// Returns true if player received heal (doesn't need to be full heal), otherwise false 
	public bool Heal(float healAmount)
	{
		if (Mathf.Approximately(_playerHealth, maxPlayerHealth)) return false;

		_playerHealth = Mathf.Max(_playerHealth + healAmount, maxPlayerHealth);
		Debug.Log("Player healed. Remaining health: " + _playerHealth);
		return true;
	}
}