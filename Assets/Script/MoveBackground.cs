﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour 
{
	public float speed;
	private float x;
	public float PontoDeDestino;
	public float PontoOriginal;
		void Update () 
		{
		x = transform.position.x;
		x += speed * Time.deltaTime;
		transform.position = new Vector3 (x, transform.position.y, transform.position.z);

		if (x <= PontoDeDestino)
		{
			x = PontoOriginal;
			transform.position = new Vector3 (x, transform.position.y, transform.position.z);
		}
	}
}
