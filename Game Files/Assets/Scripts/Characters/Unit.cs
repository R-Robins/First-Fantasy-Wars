using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour
{
    //*******************************************************
    //Components to keep track of
    //*******************************************************
    public Player owner;
    protected int charNumber;
    protected string characterName;
    protected Unit protector;

    public string CharacterType;
	public int maxHealth = 100;
	public int health = 100;

	//public int agility;
	public int attack = 20;
	public int defense = 0;
	public int move = 3;
	public int xCoordinate;
	public int yCoordinate;

    [SerializeField] protected Ability[] abilities = new Ability[3];
    [SerializeField] protected Ability[] availableAbilities = new Ability[5];
    [SerializeField] private GameObject characterModel;
    [SerializeField] private GameObject dummy;
    //Booleans
    public bool isPlaced = false;
    public bool isMoving = false;
    public bool isAttacking = false;
    public bool isAlive = true;

    //Movement
	public Vector3 destination; //Destination for movement
	public float speed = 4; //Movement Speed
	public float rotationSpeed = 6f; //Rotation Speed While Moving

	public Animator animator; //Animation of Project

	//Tiles accessable by unit
	List<HexagonTile> movable;
	List<HexagonTile> attackable;

	//Tile Unit is On
	public HexagonTile currentTile;

	//Tiles To Move To
	public List<HexagonTile> movementPath = new List<HexagonTile>();


    //*******************************************************
    //Methods
    //*******************************************************

    public Unit(string charName, int maximumHealth, int currentHealth, int attack, int defense, int speed, int charNum)
    {
        characterName = charName;
        maxHealth = maximumHealth;
        health = currentHealth;
        this.attack = attack;
        this.defense = defense;
        move = speed;
        charNumber = charNum;
        //Debug.Log("Calling " + characterName + " constructor and model number is " + charNumber);
    }

    //Adds a path for movement
    public void setPath ( List<HexagonTile> path )
	{
        this.isMoving = true;
		this.movementPath = path;
	}

    public bool Move(HexagonTile destinationTile)
    {
       if(this.movable.Contains(destinationTile))
       {
            isMoving = true;
            currentTile.isWalkable = true;
            currentTile.holdingUnit = null;
            this.movementPath = ActionController.unitPathfinding(currentTile, destinationTile, 1);
            destinationTile.isWalkable = false;
            destinationTile.holdingUnit = this;
            xCoordinate = destinationTile.x;
            yCoordinate = destinationTile.y;
            
            return true;
       }
        return false;
    }

    public bool MoveToAttack(HexagonTile destinationTile)
    {
        if (this.attackable.Contains(destinationTile))
        {
            isMoving = true;
            currentTile.isWalkable = true;
            currentTile.holdingUnit = null;
            this.movementPath = ActionController.unitPathfinding(currentTile, destinationTile, 1);
            destinationTile.isWalkable = false;
            destinationTile.holdingUnit = this;
            xCoordinate = destinationTile.x;
            yCoordinate = destinationTile.y;
            isAttacking = true;
            return true;
        }
        return false;
    }
    //Method for setting destination for movement
    public void MoveOneStep( HexagonTile destinationTile)
	{
		Vector3 hexagonLocation = destinationTile.transform.position;

		destination.x = hexagonLocation.x;
		destination.y = hexagonLocation.y;
		destination.y +=  .5f*(destinationTile.height);
		destination.z = hexagonLocation.z;

		
		currentTile = destinationTile;
	}

    public void PopText(string message)
    {
        GameObject floatingText = (GameObject)Instantiate(Resources.Load("PopText"));
        floatingText.GetComponent<TextMesh>().text = message;
        floatingText.transform.position = transform.gameObject.transform.position;
        //floatingText.transform.LookAt(Camera.main.transform);
    }

    //*******************************************************
    //Movement Tile Functions
    //*******************************************************
    public List<HexagonTile> GetMoveable()
	{
		movable = ActionController.findMovable (currentTile, move);
		return movable;
	}
		
	public void HighlightMoveable()
	{
		foreach (HexagonTile tile in movable)
			tile.Highlight (Color.cyan);
	}

	public void RemoveHighlightMovable()
	{
        if (movable != null)
        {
            foreach (HexagonTile tile in movable)
                tile.removeHighlight();
        }
	}


    //*******************************************************
    //Attacking Tile Functions
    //*******************************************************
    public List<HexagonTile> GetAttackable(int abilityNumber)
    {
        //Debug.Log("You chose ability " + abilities[abilityNumber].getName() + " range: " + abilities[abilityNumber].getRange());
        attackable = ActionController.findAttackable(currentTile, abilities[abilityNumber].getRange());  //pass abilities[abilityNumber] instead of 3
        return attackable;
    }
	public void HighlightAttackable()
	{
		foreach (HexagonTile tile in attackable)
			if (this.currentTile != tile )tile.Highlight (Color.red);
	}

    public void RemoveHighlightAttackable()
    {
        if (attackable != null)
        {
            foreach (HexagonTile tile in attackable)
                tile.removeHighlight();
        }
    }

    //*******************************************************
    //Attacking 
    //*******************************************************

    public bool Attack(HexagonTile targetTile, int abilityNumber)
    {
        if (this.attackable.Contains(targetTile))
        {
            if (this.currentTile == targetTile) return false;
            //THEN CALL THE ATTACK ON THE TARGET TILE
            animator.SetTrigger("IsAttacking");
            transform.LookAt( new Vector3(targetTile.transform.position.x, transform.position.y + .5f, targetTile.transform.position.z ));
            abilities[abilityNumber].activate(targetTile, this);
            animator.SetTrigger("IsIdle");
            isAttacking = true;
            return true;
        }
        return false;
    }
    //*******************************************************
    //Damage Taking
    //*******************************************************
    public int getAttack()
    {
        return attack;
    }

    public string getCharacterName()
    {
        return characterName;
    }

    public Ability[] getAbility()
    {
        return abilities;
    }

    public Ability[] getAvailAbility()
    {
        return availableAbilities;
    }

    public void setAbility(int avail, int chosen)
    {
        abilities[chosen] = availableAbilities[avail];
    }

    public Unit getThisObject()
    {
        return this;
    }

    public int getUnitHealth()
    {
        return health;
    }

    public void decrementEffectDuration()
    {
        for (int i = 0; i < 3; i++)
        {
            abilities[i].decrementDuration(this);
        }
    }

    public void takeDamage(int rawDamage)
    {
        if (protector != null)
        {
            protector.takeDamage(rawDamage);
            return;
        }
        health -= (rawDamage - defense);
        PopText("" + rawDamage);
        if (health <= 0)
        {
            float randomNumber = Random.Range(0, 1);
            {
                if (randomNumber < .5) animator.SetTrigger("Die1");
                else animator.SetTrigger("Die2");
            }
            isAlive = false;
        }
        animator.SetTrigger("IsDamaged");
        animator.SetTrigger("IsIdle");
    }

    public void heal(int healingDamage)
    {
        if ((healingDamage + health) > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health = healingDamage + health;
        }
        PopText("+" + healingDamage);
    }

    public void setProtector(Unit prot)
    {
        protector = prot;
    }

    public Unit getProtector()
    {
        return protector;
    }

    //*******************************************************
    //Placement methods
    //*******************************************************

    public int getUnitNumber()
    {
        return charNumber;
    }

    public GameObject getCharacter()
    {
        return characterModel;
    }

    public void placeUnit()
    {
        animator = GetComponent<Animator>();
        currentTile = MapController.hexagonTiles[xCoordinate, yCoordinate];
        currentTile.holdingUnit = this;
        transform.position = currentTile.transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y + .5f * currentTile.height, transform.position.z);
        destination = transform.position;
        isPlaced = true;
        currentTile.isWalkable = false;
    }


    //*******************************************************
    // Start and update methods
    //*******************************************************
    void Start () 
	{
        protector = null;
    }

    
    // Update is called once per frame
    void Update () 
	{
        if (health <= 0) isAlive = false;
        if(GameController.gameOver == true)
        {
            animator.SetTrigger("Win");
        }
        if (isPlaced == false && GameController.getGameStart() == true)
            placeUnit();
 
        if (isMoving == true)
        {
            //If Destination is reached
            if (destination.Equals(transform.position))
            {
                animator.SetBool("Run", false); //Idle Animation
                if (movementPath.Count != 0)
                {
                    this.MoveOneStep(movementPath[0]);
                    //Debug.Log("Removed" + movementPath[0].x + "|" + movementPath[0].y);
                    movementPath.RemoveAt(0);
                }
                else
                {
                    if (GameController.getAttackPhase())
                    {
                        //abilities[GameController.abilityChosen].deactivate(this);
                        GameController.setAttackPhase(false);
                        GameController.setSelectPhase(true);
                        GameController.abilityChosen = -1;
                        isAttacking = false;
                    }
                    isMoving = false;
                }
            }
            else
            {
                Vector3 differenceVector = (destination - transform.position).normalized;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(differenceVector), 100f);
                animator.SetBool("Run", true); //Run Animation
                Vector3 direction = destination - transform.position;
                Vector3 velocity = direction.normalized * speed * Time.deltaTime;

                //Do not let movement go over distance
                velocity = Vector3.ClampMagnitude(velocity, direction.magnitude);
                transform.Translate(velocity, Space.World);
            }
        }
    }
}
