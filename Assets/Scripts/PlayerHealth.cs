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
	
	[Header("Health Debug Info")]
	[SerializeField] private float remainingImmuneTime;
	[SerializeField] private bool isImmune;
	[SerializeField] private float playerHealth;
	
    // Start is called before the first frame update
    void Start()
    {
	    MakeImmune(immuneTimeAtGameStart);
	    // Player starts off immune. We can change this.
        playerHealth = maxPlayerHealth;
        hpSlider.maxValue = maxPlayerHealth;
        hpSlider.value = maxPlayerHealth;
    }

    void Update()
    {
	    hpSlider.value = playerHealth;
      
	    if (!isImmune) return;
	    
	    remainingImmuneTime -= Time.deltaTime;
	    if (remainingImmuneTime <= 0)
	    {
		    remainingImmuneTime = 0;
		    isImmune = false;
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
	    isImmune = true;
	    remainingImmuneTime = duration;
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
		if (isImmune && !damageAnyway) return;
		
		playerHealth -= damageAmount;
		if (playerHealth <= 0)
		{
			gameHandler.EndGame("Game Over");
			playerHealth = 0;
		}
		
		if (makeImmune)
		{
			MakeImmune(immuneTimeAtDamage);
		}
	}
	
	// Returns true if player received heal (doesn't need to be full heal), otherwise false 
	public bool Heal(float healAmount)
	{
		if (Mathf.Approximately(playerHealth, maxPlayerHealth)) return false;

		playerHealth = Mathf.Min(playerHealth + healAmount, maxPlayerHealth);
		return true;
	}
}