using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetedVisual : MonoBehaviour
{
    public float persistTime = 1.5f;
    public bool hasEffected = false;


    //Movement
    public Vector3 destination; //Destination for movement
    public HexagonTile destinationTile; //Destination tile


    public void PlaceVisual(HexagonTile tile) //initial placement of projectile
    {
        transform.position = tile.transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y + .5f * tile.height, transform.position.z);
        destination = transform.position;
        this.destinationTile = tile;
    }

    public void Update()
    {
        persistTime -= Time.deltaTime;
        if (persistTime <= 0) Destroy(this.gameObject);
    }
}
