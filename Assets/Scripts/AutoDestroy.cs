using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
	[SerializeField] private float delay = 5;

	private void Start()
	{
		Invoke("Destroy", delay);
	}

	private void Destroy()
	{
		Destroy(gameObject);
	}
}