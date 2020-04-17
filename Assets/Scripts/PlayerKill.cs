using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKill : MonoBehaviour
{
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private GameObject platformPrefab;
	[SerializeField] private GameObject hitEffect;
	[SerializeField] private ParticleSystem vfxKill;
	[SerializeField] private TrailRenderer playerTrail;
	[SerializeField] private SpriteRenderer playerSprite;
	private Vector3 initPos;
	private bool canKill;

	private void Start()
	{
		initPos = transform.position;
		canKill = true;
	}

	private void Update()
	{
		if (canKill && GameController.Instance.CanConsumePlatform && Input.GetKeyDown(KeyCode.Space))
		{
			Instantiate(platformPrefab, transform.position, Quaternion.identity);
			GameController.Instance.ConsumePlatform();

			Instantiate(vfxKill, transform.position, Quaternion.identity);

			rb.velocity = Vector2.zero;
			transform.position = initPos;

			CameraShake.Instance.ShakeCamera();

			StartCoroutine(ResetDelay());
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Target"))
		{
			GameController.Instance.Victory();
			rb.velocity = Vector2.zero;
			rb.isKinematic = true;
			Instantiate(vfxKill, transform.position, Quaternion.identity);
			playerSprite.enabled = false;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Instantiate(hitEffect, collision.contacts[0].point, Quaternion.identity);
	}

	private IEnumerator ResetDelay()
	{
		canKill = false;
		playerTrail.enabled = false;

		yield return new WaitForSeconds(0.5f);

		canKill = true;
		playerTrail.enabled = true;
	}
}
