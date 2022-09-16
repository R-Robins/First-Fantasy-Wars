using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAbility : Ability
{
    public static Object projectilePrefab;// = Resources.Load("Visual/Projectiles/Fireball");
    public int damage;
    public string projectilePath;

    public ProjectileAbility(string projectilePath, int damage, int range, string abilityName, int abilityID, string description, int dmgrad) :base(abilityName, abilityID, range, description, dmgrad)
    {
        //projectilePrefab = Resources.Load(projectilePath);
        this.projectilePath = projectilePath;
        this.damage = damage;
        this.range = range;
    }

    public override bool activate(HexagonTile targetTile, Unit castingUnit)
    {
        projectilePrefab = Resources.Load(projectilePath);
        GameObject projectile = Instantiate(projectilePrefab) as GameObject;
        ImpactProjectile impactProjectile = projectile.GetComponent<ImpactProjectile>();
        impactProjectile.PlaceProjectile(castingUnit.currentTile);
        impactProjectile.Move(targetTile);
        return true;
    }

    public override void deactivate(Unit unitStats)
    {

    }
}
