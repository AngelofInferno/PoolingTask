﻿using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
	public bool spreadShot = false;

	[Header("General")]
	public Transform gunBarrel;
	public ParticleSystem shotVFX;
	public AudioSource shotAudio;
	public float fireRate = .1f;
	public int spreadAmount = 20;

	[Header("Bullets")]
	public GameObject bulletPrefab;

	//private Queue<GameObject> disabledPrefabQueue;
	private Queue<GameObject> _disabledPrefabQueue;


	float timer;

	void Start()
	{
		_disabledPrefabQueue = new Queue<GameObject>();
	}

	void Update()
	{
		timer += Time.deltaTime;

		if (Input.GetButton("Fire1") && timer >= fireRate)
		{
			Vector3 rotation = gunBarrel.rotation.eulerAngles;
			rotation.x = 0f;

			if (spreadShot)
				SpawnBulletSpread(rotation);
			else
				SpawnBullet(rotation);
			

			timer = 0f;

			if (shotVFX)
				shotVFX.Play();

			if (shotAudio)
				shotAudio.Play();
		}
	}

	void SpawnBullet(Vector3 rotation)
	{
		//GameObject bullet = Instantiate(bulletPrefab) as GameObject;
		GameObject bullet = AddObject(bulletPrefab);

		bullet.transform.position = gunBarrel.position;
		bullet.transform.rotation = Quaternion.Euler(rotation);
	}

	void SpawnBulletSpread(Vector3 rotation)
	{
		int max = spreadAmount / 2;
		int min = -max;

		Vector3 tempRot = rotation;
		for (int x = min; x < max; x++)
		{
			tempRot.x = (rotation.x + 3 * x) % 360;

			for (int y = min; y < max; y++)
			{
				tempRot.y = (rotation.y + 3 * y) % 360;
				
				GameObject bullet = AddObject(bulletPrefab);

				bullet.transform.position = gunBarrel.position;
				bullet.transform.rotation = Quaternion.Euler(tempRot);
			}
		}
	}
	
	public GameObject AddObject(GameObject prefab)
	{
		GameObject go;
		if (_disabledPrefabQueue.Count != 0)
		{
			go = _disabledPrefabQueue.Dequeue();
			go.SetActive(true);
		}
		else
		{
			go = Instantiate(prefab) as GameObject;
			go.GetComponent<ProjectileBehaviour>().playerShooting = this;
		}
		return go;
	}
	
	public void DisableObject(GameObject go)
	{
		go.SetActive(false);
		_disabledPrefabQueue.Enqueue(go);
	}

}

