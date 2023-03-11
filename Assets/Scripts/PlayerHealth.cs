using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	[SerializeField] private float maxPlayerHealth = 20f;
	[SerializeField] private GameHandler _gameHandler;
	[SerializeField] private Slider _hpSlider;
	private float _playerHealth;
	private bool _playerAlive; 
	
    // Start is called before the first frame update
    void Start()
    {
        _playerHealth = maxPlayerHealth;
        _playerAlive = true;
        _hpSlider.maxValue = maxPlayerHealth;
        _hpSlider.value = maxPlayerHealth;
    }

    // Update is called once per frame
    void Update()
    {
    }
	
	public void DamagePlayer(float damageAmount)
	{
		_playerHealth -= damageAmount;
		if(_playerHealth <= 0)
		{
			_gameHandler.GameOver("Game Over");
			_playerAlive = false;
			_playerHealth = 0;
		}

		_hpSlider.value = _playerHealth;
	}
	
	public void HealPlayer(float healAmount)
	{
		_playerHealth += healAmount;
		if(_playerHealth >= maxPlayerHealth) _playerHealth = maxPlayerHealth;
	}
	
	public bool IsPlayerAlive()
	{
		return _playerAlive;
	}
}