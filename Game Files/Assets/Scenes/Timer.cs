using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {
    private float timer;
	// Use this for initialization
	void Start () {
        timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        timer += 1.0f*Time.deltaTime;
        transform.Translate(new Vector3(0, 1.0f*Time.deltaTime, 0), Space.World);
        if (timer > 2.0f)
            Destroy(gameObject);
	}
}
