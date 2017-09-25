using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructor : MonoBehaviour
{

	public float LifeSpan = 1;

	// Use this for initialization
	void Start ()
	{
		Destroy (gameObject, LifeSpan);
	}

}
