using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactProjectile : MonoBehaviour
{
    //public ParticleSystem particleEffect;
    public string impactPath;
    public Object impactSystem;
    public float persistTime = 1.5f;
    public bool hasImpacted = false;

    public int damage = 40;

    //Movement
    public Vector3 destination; //Destination for movement
    float speed = 5; //Movement Speed

    
    public HexagonTile currentTile; //Tile projectile starts at
    public HexagonTile destinationTile; //Destination tile

    //Tiles To Move To
    public List<HexagonTile> movementPath = new List<HexagonTile>();

    public bool Move(HexagonTile destinationTile) //Sets movement path
    {
        this.destinationTile = destinationTile;
        this.movementPath = ActionController.projectilePathfinding(currentTile, destinationTile, 4);
        return true;
    }

    //Method for setting destination for next step
    public void MoveOneStep(HexagonTile destinationTile)
    {
        Vector3 hexagonLocation = destinationTile.transform.position;

        destination.x = hexagonLocation.x;
        destination.y = hexagonLocation.y + .5f;
        destination.z = hexagonLocation.z;

        if (destinationTile.height > currentTile.height)
            destination.y += .5f*(destinationTile.height - currentTile.height) + .5f * (destinationTile.height);
        else
            destination.y += .5f * (destinationTile.height);
        


        currentTile = destinationTile;
    }


    public void PlaceProjectile( HexagonTile tile) //initial placement of projectile
    {
        transform.position = tile.transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y + .5f * tile.height, transform.position.z);
        destination = transform.position;
        this.currentTile = tile;
    }

    public void Impact() //Called on impact
    {
        Destroy(this.gameObject.GetComponent<ParticleSystem>());
        if(destinationTile.holdingUnit != null)
            destinationTile.holdingUnit.takeDamage(damage);
        impactSystem = Resources.Load(impactPath);
        GameObject projectile = Instantiate(impactSystem, gameObject.transform) as GameObject;
        hasImpacted = true;
    }

	
	// Update is called once per frame
	void Update ()
    {
        if (destination.Equals(transform.position))
        {
            if (movementPath.Count != 0)
            {
                this.MoveOneStep(movementPath[0]);
                movementPath.RemoveAt(0);
            }
            else if(!hasImpacted) Impact();
        }

        else
        {
            Vector3 differenceVector;
            if (destination.y > transform.position.y)
                differenceVector = new Vector3(0, (destination.y - transform.position.y), 0).normalized;
            else if (destination.y < transform.position.y)
                differenceVector = new Vector3(destination.x - transform.position.x, 0, destination.z - transform.position.z).normalized;
            else differenceVector = (destination - transform.position).normalized;
            

            Vector3 direction = destination - transform.position;
            Vector3 velocity = direction.normalized * speed * Time.deltaTime;

            //Do not let movement go over distance
            velocity = Vector3.ClampMagnitude(velocity, direction.magnitude);
            transform.Translate(velocity, Space.World);
        }
        if(hasImpacted)
        {
            persistTime -= Time.deltaTime;
            if (persistTime <= 0) Destroy(this.gameObject);
        }
    }
}
