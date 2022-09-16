using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneMissiles : ProjectileAbility {
    private float timeBetweenMissiles = .1f;
    public int numberMissiles = 5;

    public ArcaneMissiles() : base("ArcaneMissiles", 40, 3, "Arcane Missiles", 2, "Shoots a volley of arcane power!", 0)
    {

    }

    public override bool activate(HexagonTile targetTile, Unit castingUnit)
    {
        projectilePrefab = Resources.Load(projectilePath);

        GameObject projectile = Instantiate(projectilePrefab) as GameObject;
        ImpactProjectile impactProjectile = projectile.GetComponent<ImpactProjectile>();
        impactProjectile.PlaceProjectile(castingUnit.currentTile);
        impactProjectile.Move(targetTile);

        shootMissiles(targetTile, castingUnit);
        
        
        return true;
    }

    IEnumerator shootMissiles(HexagonTile targetTile, Unit castingUnit)
    {
        for (int i = 0; i < numberMissiles; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab) as GameObject;
            ImpactProjectile impactProjectile = projectile.GetComponent<ImpactProjectile>();
            impactProjectile.PlaceProjectile(castingUnit.currentTile);
            impactProjectile.Move(targetTile);

            yield return .4f;    //Wait one frame
        }
        
    }

}
