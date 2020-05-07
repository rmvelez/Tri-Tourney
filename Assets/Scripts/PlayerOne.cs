using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOne : MonoBehaviour {

    public float speed; //how fast he goes
    public float jumpForce; //how high he can jump
    public bool grounded = false; // checks to see if the player is touching the ground
    public GameObject groundCheck = null; // slot for the ground checker
    private Rigidbody2D MyRb; //need a body to jump
    public bool facingRight; // used to see if player is facing right
    public GameObject otherPlayer; // reference to the other player
    public GameObject thisPlayer; // reference to the current player

    private float horizontal; // used for the horizontal axis on L-stick
    private float vertical; // used for the vertical axis on L-Stick
    private float seventhAxis; // used for vertical axis on d-pad
    private float ninthAxis; // var used for the left trigger button

    public static float playerHealth; // value of player health
    public Text winText; // text field that displays the winner of the match
    public Text messageText; // text that displays info during match
    public GameObject medalOne; // the first medal the player earns
    public GameObject medalTwo; // the second medal the player earns
    private float loseCount; // tracks how often the player loses

    public GameObject slugFestAttack; // the hitbox for the light punch 
    public GameObject slugFestAttackTwo; // the hitbox for the heavy punch
    public GameObject slugFestAttackThree; // the hitbox for the light kick 
    public GameObject slugFestAttackFour; // the hitbox for the heavy kick
    public GameObject festSpawn; // the fist attack
    public GameObject kickSpawn; // the kick attack
    public GameObject whiteArmAttack; // the hitbox for the light hrizontal attack
    public GameObject whiteArmAttack2; // the hitbox for the heavy horizontal attack
    public GameObject whiteArmAttack3; // the hitbox for the light vertical attack
    public GameObject whiteArmAttack4; // the hitbox for the heavy vertical attack
    public GameObject whiteArmSpawn; // spawns the white arm
    public GameObject fireArmAttack; // the cold arm attack
    public GameObject fireArmSpawn; // spawns the fire arm
    public GameObject reverseFireArmAttack; // the bullet that flies in the other direction
    public GameObject spawnPoint; // place where the player spawns

    public GameObject defenseShield; // shield spawned by the player

    private float startingTime; // float to control when the match starts
    public float startTimeBetweenHits; // the initial value of the wait timer
    private float timeBetweenHits; // how long the punch will last for
    public float startTimeBetweenStabs; // the initial value of the wait timer
    private float timeBetweenStabs; // how long the stab will last for
    public float startTimeBetweenShots; // the initial value of the wait timer
    private float timeBetweenShots; // how long the fire arm can shoot for

    private int ammoLimit; // tracks the amount of ammo that the player has
    private int maxAmmo; // the maximum amount of ammo that the player has
    private bool canReload; // allows the player to reload their bullets
    public Text ammoText; // displays the ammo text on screen
    public Button restartButton;
    public Button quitButton;

    private bool usingSlugFest; // bools used to determine which fighting style is being used
    private bool usingWhiteArm;
    private bool usingFireArm;
    private bool isMoving; // checks to see if the player is moving
    private bool isBlocking; // checks to see if the plaer is blocking
    private bool isAttacking; // checks to see if the player is attacking

    public GameObject imageSpawn; // a slot for the object that displays the image for each fighting style
    public GameObject fistImage;
    public GameObject bladeImage;
    public GameObject ammoImage;

    private Animator myAnimator; // reference to the animator component on the player

    // Use this for initialization
    void Start()
    {
        MyRb = GetComponent<Rigidbody2D>(); //connect ref to rigidbody
        myAnimator = GetComponent<Animator>(); // gets the animator

        playerHealth = 5; // player's health from the start
        facingRight = true; // this player is facing right

        canReload = false; // player is unable to reload
        ammoLimit = 6; // the player starts with six bullets
        maxAmmo = 6; // sets max ammo

        isMoving = true; // the player can't move
        isBlocking = false; // the player isn't blocking
        isAttacking = true; // the player can't attack

        defenseShield.SetActive(false); // diables the shield when the game starts

        ammoText.text = "Ammo: " + ammoLimit; // displays how many bullets the player has
        startingTime = 0f;

        medalOne.SetActive(false); // the medals are disabled
        medalTwo.SetActive(false);
        restartButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);

        usingSlugFest = true; // the player starts off with the Slug Fest Style
        usingWhiteArm = false;
        usingFireArm = false;
    }

    void Update()
    {
        ninthAxis = Input.GetAxisRaw("9th Axis");

        if (isAttacking == true)
        {
            StyleSwitch(); // calls on the style switch function
        }

        ReloadCheck(); // runs the reload check function
        PlayerDefense(); // calls on the block function
        GameOver(); // calls on the game over function
        StateControl(); // calls on the state control function

        startingTime += Time.deltaTime;
        
        if (startingTime > 0f)
        {
            messageText.text = "ROUND 1";
        }
        if (startingTime > 1f)
        {
            messageText.text = "FIGHT!";
        }
        if (startingTime > 2f)
        {
            messageText.text = "";
        }
    }

    // function that runs when the player has run out of health
    void GameOver()
    {
        // runs when this player runs out of health
        if (playerHealth <= 0)
        {
            this.gameObject.SetActive(false);
            medalOne.SetActive(true);
            winText.text = "Player Two Wins!";

            restartButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);
            //loseCount++;
            //StartCoroutine(RoundReset(2f));
        }
    }
    
    IEnumerator RoundReset (float time)
    {
        yield return new WaitForSeconds(time / 5);
        Instantiate(thisPlayer, spawnPoint.transform.position, thisPlayer.transform.rotation);
        playerHealth = 5;
        startingTime = 0f;
    }

    // this function calls on the player move function
    void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // sets the var to the horizontal axis
        vertical = Input.GetAxisRaw("Vertical"); // sets var for the vertical axis
        seventhAxis = Input.GetAxisRaw("7th Axis"); // gets the 7th axis

        if (isMoving == true)
        {
            PlayerMove(); // calls on the player move function
        }
    }

    // this function allows the player to block attacks form the other player
    void PlayerDefense()
    {
        // this statement raises the shield when pressing the block button
        if (ninthAxis == 1)
        {
            defenseShield.SetActive(true); // the shield is active
            isBlocking = true; // the player is blocking
        }
        else if (ninthAxis == 0)
        {
            defenseShield.SetActive(false); // the shield is inactive
            isBlocking = false; // the player isn't blocking
        }
    }

    // this function deals with the states of play that the player goes through during a match
    void StateControl()
    {
        // checks to see if the player is blocking
        if (isBlocking == true || startingTime <= 2f)
        {
            isMoving = false; // the player can't move
            isAttacking = false; // the player can't attack
        }
        // checks to see if the player can reload
        else if (canReload == true)
        {
            isAttacking = false; // the player can't attack
        }
        else // when not blocking or reloading
        {
            isMoving = true; // the player can move
            isAttacking = true; // the player can attack
        }
    }

    void StyleSwitch()
    {
        if (Input.GetKey(KeyCode.Joystick1Button4) && horizontal == 1) // command input to use white arm style
        {
            usingWhiteArm = true; // switch to white arm style
            usingSlugFest = false;
            usingFireArm = false;
        }
        else if (Input.GetKey(KeyCode.Joystick1Button4) && seventhAxis == -1 || Input.GetKey(KeyCode.Joystick1Button4) && vertical == -1) // command input to use fire arm style
        {
            usingFireArm = true; // switch to fire arm style
            usingWhiteArm = false;
            usingSlugFest = false;
        }
        else if (Input.GetKey(KeyCode.Joystick1Button4) && horizontal == -1) // command input for slug fest style
        {
            usingSlugFest = true; // switch back to the Slug Fest Style
            usingFireArm = false;
            usingWhiteArm = false;
        }

        if (usingSlugFest == true)
        {
            Instantiate(fistImage, imageSpawn.transform.position, fistImage.transform.rotation);
            SlugFestStyle(); // player is using this style
        }
        else if (usingWhiteArm == true)
        {
            Instantiate(bladeImage, imageSpawn.transform.position, bladeImage.transform.rotation);
            WhiteArmStyle(); // player is using this style
        }
        else if (usingFireArm == true)
        {
            Instantiate(ammoImage, imageSpawn.transform.position, ammoImage.transform.rotation);
            FireArmStyle(); // player is using this style
        }
    }

    // this is the fighting style that uses hand-to-hand-combat
    void SlugFestStyle()
    {
        // runs when the X button is pressed
        if (Input.GetKey(KeyCode.Joystick1Button2) && timeBetweenHits <= 0)
        {
            // throws a light punch
            Instantiate(slugFestAttack, festSpawn.transform.position, slugFestAttack.transform.rotation);
            timeBetweenHits = startTimeBetweenHits; // waits before another punch is thrown
        }
        // runs when the Y button is pressed
        else if (Input.GetKey(KeyCode.Joystick1Button3) && timeBetweenHits <= 0)
        {
            // throws a heavy punch
            Instantiate(slugFestAttackTwo, festSpawn.transform.position, slugFestAttackTwo.transform.rotation);
            timeBetweenHits = startTimeBetweenHits; // waits before another punch is thrown
        }
        // runs when the A button is pressed
        else if (Input.GetKey(KeyCode.Joystick1Button0) && timeBetweenHits <= 0)
        {
            // does a light kick
            Instantiate(slugFestAttackThree, kickSpawn.transform.position, slugFestAttackThree.transform.rotation);
            timeBetweenHits = startTimeBetweenHits; // waits before another punch is thrown
        }
        // runs when the B button is pressed
        else if (Input.GetKey(KeyCode.Joystick1Button1) && timeBetweenHits <= 0)
        {
            // does a heavy kick
            Instantiate(slugFestAttackFour, kickSpawn.transform.position, slugFestAttackFour.transform.rotation);
            timeBetweenHits = startTimeBetweenHits; // waits before another punch is thrown
        }
        // while none of the buttons are being pressed
        else
        {
            // length of delay between attacks
            timeBetweenHits -= Time.deltaTime;
        }
    }

    // this is the fighting style that features the use of weapons that dosen't use bullets
    void WhiteArmStyle()
    {
        // runs when the X button is pressed
        if (Input.GetKey(KeyCode.Joystick1Button2) && timeBetweenStabs <= 0)
        {
            // does a light horizontal attack
            Instantiate(whiteArmAttack, whiteArmSpawn.transform.position, whiteArmAttack.transform.rotation);
            timeBetweenStabs = startTimeBetweenStabs; // waits before the weapon can be used again
        }
        // runs when the Y button is pressed
        else if (Input.GetKey(KeyCode.Joystick1Button3) && timeBetweenStabs <= 0)
        {
            // does a heavy horizontal attack
            Instantiate(whiteArmAttack2, whiteArmSpawn.transform.position, whiteArmAttack.transform.rotation);
            timeBetweenStabs = startTimeBetweenStabs; // waits before the weapon can be used again
        }
        // runs when the A button is pressed
        else if (Input.GetKey(KeyCode.Joystick1Button0) && timeBetweenStabs <= 0)
        {
            // does a light vertical attack
            Instantiate(whiteArmAttack3, whiteArmSpawn.transform.position, whiteArmAttack.transform.rotation);
            timeBetweenStabs = startTimeBetweenStabs; // waits before the weapon can be used again
        }
        // runs when the B button is pressed
        else if (Input.GetKey(KeyCode.Joystick1Button1) && timeBetweenStabs <= 0)
        {
            // does a heavy horizontal attack
            Instantiate(whiteArmAttack4, whiteArmSpawn.transform.position, whiteArmAttack.transform.rotation);
            timeBetweenStabs = startTimeBetweenStabs; // waits before the weapon can be used again
        }
        // while none of the buttons are being pressed
        else
        {
            timeBetweenStabs -= Time.deltaTime; // length of delay between White Arm attacks
        }
    }

    // this is the fighting style that uses bullets
    void FireArmStyle()
    {
        // runs when attack button is pressed and the player is facing right
        if (Input.GetKey(KeyCode.Joystick1Button2) && facingRight == true && timeBetweenShots <= 0)
        {
            // fires a bullet from fire arm
            Instantiate(fireArmAttack, fireArmSpawn.transform.position, fireArmAttack.transform.rotation);
            ammoLimit -= 1; // loses a bullet
            ammoText.text = "Ammo: " + ammoLimit; // displays ammo
            timeBetweenShots = startTimeBetweenShots; // waits before fireing another bullet
        }
        // runs when attack button is pressed and the player isn't facing right
        else if (Input.GetKey(KeyCode.Joystick1Button2) && facingRight == false && timeBetweenShots <= 0)
        {
            // fires a bullet from fire arm
            Instantiate(reverseFireArmAttack, fireArmSpawn.transform.position, reverseFireArmAttack.transform.rotation);
            ammoLimit -= 1; // loses a bullet
            ammoText.text = "Ammo: " + ammoLimit; // displays ammo
            timeBetweenShots = startTimeBetweenShots; // waits before fireing another bullet
        }
        else
        {
            timeBetweenShots -= Time.deltaTime; // time before next shot
        }
    }

    void ReloadCheck()
    {
        // checks to see if the player has less than the maximum amount of ammo
        if (ammoLimit <= 0)
        {
            canReload = true; // the player can reload
            if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                StartCoroutine(AmmoReload(2f)); // calls on the reload function
            }
        }
        else if (ammoLimit == maxAmmo) // when ammo is reloaded
        {
            canReload = false;
            StopCoroutine(AmmoReload(2f));
        }

        // this runs when players wis to cancel the reload function
        //if (Input.GetKey(KeyCode.G) && isMoving == false)
        //
        //StopCoroutine(AmmoReload(0f));
        //isMoving = true;
        //}
    }

    // this is the fucntion that reloads the ammo when player run out of bullets
    IEnumerator AmmoReload(float time)
    {
        if (ammoLimit <= maxAmmo) // runs if the ammoLimit is less than the Max ammo
        {
            for (int i = ammoLimit; i < 6 || i < maxAmmo; i++) // runs a loop to refill ammo
            {
                ammoLimit++; // increases by 1
                ammoText.text = "Ammo: " + ammoLimit; // displays new value
                yield return new WaitForSeconds(time / 5); // delays rate of repelinishment
            }
        }
    }

    // function that lets the player move
    void PlayerMove()
    {
        float currentYVal = MyRb.velocity.y;
        Vector2 characterScale = transform.localScale;

        //lets the player move forward if pressing forward but not pressing the style switch button
        if (horizontal == 1 && !Input.GetKey(KeyCode.Joystick1Button4))
        {
            MyRb.velocity = new Vector2(horizontal * speed, currentYVal);
        }
        // lets the player move if pressing backward but not pressing the style switch button
        else if (horizontal == -1 && !Input.GetKey(KeyCode.Joystick1Button4))
        {
            MyRb.velocity = new Vector2(-horizontal * -speed, currentYVal);
        }
        // when the player isn't pressing either the L-stick or the D-pad
        else
        {
            MyRb.velocity = new Vector2(0, currentYVal);
        }

        // checks to see if the player and groundCheck are touching a platform
        if (Physics2D.Linecast(transform.position, groundCheck.transform.position))
        {
            grounded = true; // allows the player to jump
        }
        else // runs while player is in the air
        {
            grounded = false; // prevents the player from jumping
        }

        // checks to see if the player can jump
        if (grounded == true)
        {
            MyRb.AddForce(new Vector2(0, vertical * jumpForce)); // L-Stick Version
            MyRb.AddForce(new Vector2(0, seventhAxis * jumpForce)); // D-Pad version
        }

        // checks to see if this player's x-position is less than the other player's x-position
        if (this.gameObject.transform.position.x > otherPlayer.transform.position.x)
        {
            imageSpawn.transform.position = new Vector2(-6.6f, 6f);
            facingRight = false; // they are now facing right
            characterScale.x = -2.5f;
            characterScale.y = 2.5f;
        }
        else
        {
            imageSpawn.transform.position = new Vector2(-6.6f, 6f);
            facingRight = true; // they are not facing right
            characterScale.x = 2.5f;
            characterScale.y = 2.5f;
        }

        transform.localScale = characterScale;
    }


    // this code runs when the player is attacked
    void OnCollisionEnter2D(Collision2D other)
    {
        // runs when the player is hit with a light punch and is not blocking
        if (other.gameObject.tag == "PunchTwo" && isBlocking == false)
        {
            playerHealth -= 0.3f;
        }
        // runs when the player is blocking while being hit with a light punch
        else if (other.gameObject.tag == "PunchTwo" && isBlocking == true)
        {
            playerHealth -= 0.03f;
        }
        // runs when the player is hit with a heavy punch and is not blocking
        else if (other.gameObject.tag == "OtherPunchTwo" && isBlocking == false)
        {
            playerHealth -= 0.6f;

            // these two if statements impliment knockback when hit by a heavy punch
            if (facingRight == true)
            {
                transform.position = new Vector2(this.transform.position.x - 1, this.transform.position.y);
                imageSpawn.transform.position = new Vector2(-6.6f, 6f);
            }
            else if (facingRight == false)
            {
                transform.position = new Vector2(this.transform.position.x + 1, this.transform.position.y);
                imageSpawn.transform.position = new Vector2(-6.6f, 6f);
            }
        }
        // runs when the player is blocking while being hit with a heavy punch
        else if (other.gameObject.tag == "OtherPunchTwo" && isBlocking == true)
        {
            playerHealth -= 0.06f;
        }
        // runs when the player is hit with a light kick and is not blocking
        else if (other.gameObject.tag == "OtherKick" && isBlocking == false)
        {
            playerHealth -= 0.4f;
        }
        // runs when the player is blocking while being hit with a light kick
        else if (other.gameObject.tag == "OtherKick" && isBlocking == true)
        {
            playerHealth -= 0.04f;
        }
        // runs when the player is hit with a heavy kick and is not blocking
        else if (other.gameObject.tag == "OtherKickTwo" && isBlocking == false)
        {
            playerHealth -= 0.8f;

            // these two if statements impliment knockback when hit by a heavy kick
            if (facingRight == true)
            {
                transform.position = new Vector2(this.transform.position.x - 1, this.transform.position.y);
                imageSpawn.transform.position = new Vector2(-6.6f, 6f);
            }
            else if (facingRight == false)
            {
                transform.position = new Vector2(this.transform.position.x + 1, this.transform.position.y);
                imageSpawn.transform.position = new Vector2(-6.6f, 6f);
            }
        }
        // runs when the player is blocking while being hit with a heavy kick
        else if (other.gameObject.tag == "OtherKickTwo" && isBlocking == true)
        {
            playerHealth -= 0.08f;
        }
        // runs when the player is hit by a light horizontal attack when not blocking
        else if (other.gameObject.tag == "SwordTwo" && isBlocking == false)
        {
            playerHealth -= 0.6f;
        }
        // runs when the player is hit by a light horizontal attack object while blocking
        else if (other.gameObject.tag == "SwordTwo" && isBlocking == true)
        {
            playerHealth -= 0.06f;
        }
        // runs when the player is hit by a heavy horizontal attack when not blocking
        else if (other.gameObject.tag == "OtherSwordTwo" && isBlocking == false)
        {
            playerHealth -= 0.9f;

            // these two if statements impliment knockback when hit by a heavy horizontal attack
            if (facingRight == true)
            {
                transform.position = new Vector2(this.transform.position.x - 2, this.transform.position.y);
                imageSpawn.transform.position = new Vector2(-6.6f, 6f);
            }
            else if (facingRight == false)
            {
                transform.position = new Vector2(this.transform.position.x + 2, this.transform.position.y);
                imageSpawn.transform.position = new Vector2(-6.6f, 6f);
            }
        }
        // runs when the player is hit by a heavy horizontal attack while blocking
        else if (other.gameObject.tag == "OtherSwordTwo" && isBlocking == true)
        {
            playerHealth -= 0.09f;
        }
        // runs when the player is hit by a light vertical attack when not blocking
        else if (other.gameObject.tag == "OtherSwordThree" && isBlocking == false)
        {
            playerHealth -= 0.4f;
        }
        // runs when the player is hit by a light vertical attack while blocking
        else if (other.gameObject.tag == "OtherSwordThree" && isBlocking == true)
        {
            playerHealth -= 0.04f;
        }
        // runs when the player is hit by a heavy vertical attack when not blocking
        else if (other.gameObject.tag == "OtherSwordFour" && isBlocking == false)
        {
            playerHealth -= 0.7f;

            // these two if statements impliment knockback when hit by a heavy vertical attack
            if (facingRight == true)
            {
                transform.position = new Vector2(this.transform.position.x - 1, this.transform.position.y);
                imageSpawn.transform.position = new Vector2(-6.6f, 6f);
            }
            else if (facingRight == false)
            {
                transform.position = new Vector2(this.transform.position.x + 1, this.transform.position.y);
                imageSpawn.transform.position = new Vector2(-6.6f, 6f);
            }
        }
        // runs when the player is hit by a heavy vertical attack while blocking
        else if (other.gameObject.tag == "OtherSwordFour" && isBlocking == true)
        {
            playerHealth -= 0.07f;
        }
        // runs when the player is shot and not blocking
        else if (other.gameObject.tag == "BulletTwo" && isBlocking == false)
        {
            playerHealth -= 0.2f;
        }
        // runs when the player is shot while blocking
        else if (other.gameObject.tag == "BulletTwo" && isBlocking == true)
        {
            playerHealth -= 0.02f;
        }
    }
}
