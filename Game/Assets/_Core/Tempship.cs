using UnityEngine;
using System.Collections;

public class Tempship : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.W)) {
			rigidbody2D.AddForce(new Vector3(100.0f, 100.0f, 100.0f));
		}
	}
}

