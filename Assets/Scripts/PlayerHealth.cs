using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	[SerializeField] private float maxPlayerHealth = 20f;
	[SerializeField] private float playerHealth;
	[SerializeField] private bool playerAlive; 
	
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = maxPlayerHealth;
		playerAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
		playerHealth = Mathf.Clamp(playerHealth, 0, maxPlayerHealth);
    }
	
	void DamagePlayer(float damageAmount)
	{
		playerHealth -= damageAmount;
		if(playerHealth <= 0) {
			playerAlive = false;
			playerHealth = 0;
		}
	}
	
	void HealPlayer(float healAmount)
	{
		playerHealth += healAmount;
		if(playerHealth >= maxPlayerHealth) playerHealth = maxPlayerHealth;
	}
	
	bool IsPlayerAlive()
	{
		return playerAlive;
	}
}