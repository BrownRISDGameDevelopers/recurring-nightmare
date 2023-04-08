using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	[SerializeField] private float maxPlayerHealth = 10f;
	[SerializeField] private GameHandler _gameHandler;
	[SerializeField] private Slider _hpSlider;
	private float _playerHealth;
	private bool _playerAlive;
	// How much time the player is invincible after being damaged.
	[SerializeField] private float _immuneMax = 3f;
	// How much time is remaining before the player will no longer be invincible.
	private float _immuneTime;
	// Whether the player is currently immune.
	private bool _immune;
	
    // Start is called before the first frame update
    void Start()
    {
	    makePlayerImmune();
	    // Player starts off immune. We can change this.
	    _immuneTime = _immuneMax;
        _playerHealth = maxPlayerHealth;
        _playerAlive = true;
        _hpSlider.maxValue = maxPlayerHealth;
        _hpSlider.value = maxPlayerHealth;
    }

    void Update()
    {
	    _hpSlider.value = _playerHealth;
	    if (_immune)
	    {
		    _immuneTime = _immuneTime - Time.deltaTime;
		    if (_immuneTime <= 0)
		    {
			    _immuneTime = 0;
			    _immune = false;
			    Debug.Log("Player is no longer immune.");
			    /*
			    // This is if we want to have different layers to allow the player immunity
			    // from collisions with the enemy.
			    int LayerPlayerDefault = LayerMask.NameToLayer("PlayerDefault");
			    gameObject.layer = LayerPlayerDefault;
			    */
		    }
	    }
    }

    public void makePlayerImmune()
    {
	    _immune = true;
	    _immuneTime = _immuneMax;
	    Debug.Log("Player is now immune.");
	    /*
	    // We can use something like this to temporarily set the player
	    // to a layer where they won't collide with the enemy.
	    int LayerPlayerImmune = LayerMask.NameToLayer("PlayerImmune");
	    gameObject.layer = LayerPlayerImmune;
	    Debug.Log(LayerPlayerImmune);
	    */
    }

	public void DamagePlayer(float damageAmount, bool makeImmune = false, bool damageAnyway = false)
	{
		if (!_immune || damageAnyway)
		{
			_playerHealth -= damageAmount;
			if (_playerHealth <= 0)
			{
				_gameHandler.GameOver("Game Over");
				_playerAlive = false;
				_playerHealth = 0;
			}
			Debug.Log("Player damaged. Remaining health: " + _playerHealth);

			if (makeImmune)
			{
				makePlayerImmune();
			}
		}
	}

	public void HealPlayer(float healAmount)
	{
		_playerHealth += healAmount;
		if(_playerHealth >= maxPlayerHealth) _playerHealth = maxPlayerHealth;
		Debug.Log("Player healed. Remaining health: " + _playerHealth);
	}

	public bool IsPlayerImmune()
	{
		return _immune;
	}
	
	public bool IsPlayerAlive()
	{
		return _playerAlive;
	}
}