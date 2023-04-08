using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	[SerializeField] private float maxPlayerHealth = 10f;
	[SerializeField] private GameHandler gameHandler;
	[SerializeField] private Slider hpSlider;
	private float _playerHealth;
	private bool _playerAlive;
	
	// How much time the player is invincible after being damaged.
	[SerializeField] private float immuneMax = 3f;
	// How much time is remaining before the player will no longer be invincible.
	private float _immuneTime;
	// Whether the player is currently immune.
	private bool _immune;
	
    // Start is called before the first frame update
    void Start()
    {
	    MakePlayerImmune();
	    // Player starts off immune. We can change this.
	    _immuneTime = immuneMax;
        _playerHealth = maxPlayerHealth;
        _playerAlive = true;
        hpSlider.maxValue = maxPlayerHealth;
        hpSlider.value = maxPlayerHealth;
    }

    void Update()
    {
	    hpSlider.value = _playerHealth;
      
	    if (!_immune) return;
	    
	    _immuneTime -= Time.deltaTime;
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

    public void MakePlayerImmune()
    {
	    _immune = true;
	    _immuneTime = immuneMax;
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
		if (_immune && !damageAnyway) return;
		
		_playerHealth -= damageAmount;
		if (_playerHealth <= 0)
		{
			gameHandler.EndGame("Game Over");
			_playerAlive = false;
			_playerHealth = 0;
		}
		Debug.Log("Player damaged. Remaining health: " + _playerHealth);

		if (makeImmune)
		{
			MakePlayerImmune();
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