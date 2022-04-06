using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBehaviour : MonoBehaviour
{
	[Header("Movement")]
	public float speed = 50f;

	public float bulletLifeTime = 5f;

	public PlayerShooting playerShooting;

	private void OnEnable()
	{
		StartCoroutine(RemoveOverTime(bulletLifeTime));
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	void Update()
	{
		Vector3 movement = transform.forward * speed * Time.deltaTime;
		GetComponent<Rigidbody>().MovePosition(transform.position + movement);
	}

	void OnTriggerEnter(Collider theCollider)
	{
		if (theCollider.tag == "Enemy" || theCollider.tag == "Environment")
			RemoveProjectile();
	}

	void RemoveProjectile()
	{
		playerShooting.DisableObject(gameObject);

	}

	IEnumerator RemoveOverTime(float time)
	{
		yield return new WaitForSeconds(time);
		RemoveProjectile();
	}
}
