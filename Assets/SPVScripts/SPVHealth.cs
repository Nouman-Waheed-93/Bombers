using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SPVHealth : MonoBehaviour
{
	public int maxHealth = 100;
    [HideInInspector]
	public int currHealth;


	public abstract void Damage (int amount);
}
