using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    public float persistTime = 0.5f;

	// Update is called once per frame
	void Update ()
    {
        persistTime -= Time.deltaTime;
        if (persistTime <= 0) Destroy(this.gameObject);

    }
}
