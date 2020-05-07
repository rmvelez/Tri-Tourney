using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTwo : MonoBehaviour {

    public float speed; //how fast he goes
    public float jumpForce; //how high he can jump
    public bool grounded = false; // checks to see if the player is touching the ground
    public GameObject groundCheck = null; // slot for the ground checker
    private Rigidbody2D MyRb; //need a body to jump
    public bool facingRight; // checks to see if the player is facing right

    private float horizontalTwo; // used for the horizontal axis on L-stick
    private float verticalTwo; // used for the vertical axis on L-Stick
    private float seventhAxisTwo; // used for the seventh axis
    private float ninthAxisTwo; // used for the ninth axis

    public GameObject otherPlayer; // reference to the other player

    public static float playerTwoHealth; // value of player 2 health
    public Text winText; // displays the winner of the match
    public GameObject otherMedalOne; // the medals that the player can earn
    public GameObject otherMedalTwo;

    public GameObject slugFestAttack; // the hitbox for the punch 
    public GameObject slugFestAttackTwo; // the hitboc for the heavy punch
    public GameObject slugFestAttackThree; // the hitbox for the light kick 
    public GameObject slugFestAttackFour; // the hitbox for the heavy kick
    public GameObject festSpawn; // the fist attack
    public GameObject kickSpawn; // the kick attack
    public GameObject whiteArmAttack; // the hitbox for the light horizontal attack
    public GameObject whiteArmAttack2; // the hitbox for the heavy horizontal attack
    public GameObject whiteArmAttack3; // the hitbox for the light vertical attack
    public GameObject whiteArmAttack4; // the hitbox for the heavy vertical attack
    public GameObject whiteArmSpawn; // spawns the white arm
    public GameObject fireArmAttack; // the cold arm attack
    public GameObject reverseFireArmAttack; // the attack in the opposite direction
    public GameObject fireArmSpawn; // spawns the fire arm

    public GameObject otherDefenseShield; // the other shield spawned by the player

    private float startingTime; // timer used to track for rounds
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
    private bool isMoving; // controls whether or not the player can move
    private bool isBlocking; // var that checks to see if the player is blocking
    private bool isAttacking; // checks to see if the player is attacking

    public GameObject imageSpawn; // a slot for the object that displays the image for each fighting style
    public GameObject fistImage;
    public GameObject bladeImage;
    public GameObject ammoImage;

    private Animator myAnimator; // reference to the animator

    // Use this for initialization
    void Start()
    {
        MyRb = GetComponent<Rigidbody2D>(); //connect ref to rigidbody
        myAnimator = GetComponent<Animator>();

        playerTwoHealth = 5; // player 2 health from the start
        facingRight = false; // this player faces left when the game starts

        canReload = false; // player is unable to reload
        isBlocking = false; // the player is not blocking
        isAttacking = true; // the player can attack

        otherDefenseShield.SetActive(false);

        ammoLimit = 6; // the player starts with six bullets
        maxAmmo = 6; // the max ammo is set here
        ammoText.text = "Ammo: " + ammoLimit; // displays how many bullets the player has
        isMoving = true; // the player can move
        startingTime = 0f;

        otherMedalOne.SetActive(false); // the player has earned no medals when the game starts
        otherMedalTwo.SetActive(false);
        restartButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);

        usingSlugFest = true; // the player starts off with the Slug Fest Style
        usingWhiteArm = false;
        usingFireArm = false;
    }

    void Update()
    {
        ninthAxisTwo = Input.GetAxisRaw("9th Axis Two");

        startingTime += Time.deltaTime;

        if (isAttacking == true)
        {
            StyleSwitch(); // calls on the function that allows players to switch fighting styles
        }

        ReloadCheck(); // calls on the function that reloads the bullets
        PlayerDefense(); // calls the function that allows the player to block incoming attacks
        GameOver(); // calls on the game over function
        StateControl(); // calls on the state control function
    }

    // function that runs when the player has run out of health
    void GameOver()
    {
        // runs when this player runs out of health
        if (playerTwoHealth <= 0)
        {
            this.gameObject.SetActive(false);
            otherMedalOne.SetActive(true);
            winText.text = "Player One Wins!";

            restartButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);
        }
    }

    // this function calls the player movement function
    void FixedUpdate()
    {
        horizontalTwo = Input.GetAxisRaw("HorizontalTwo"); // sets the var to the horizontal axis
        verticalTwo = Input.GetAxisRaw("VerticalTwo"); // sets var for the vertical axis
        seventhAxisTwo = Input.GetAxisRaw("7th Axis Two"); // sets var to the seventh axis

        if (isMoving == true)
        {
            PlayerMove(); // calls on the player move function
        }
    }

    // this function allows the player to block attacks form the other player
    void PlayerDefense()
    {
        // this statement raises the shield when pressing the block button and facing right
        if (ninthAxisTwo == 1)
        {
            otherDefenseShield.SetActive(true); // the other shield is inactive
            isBlocking = true; // the player is blocking
        }
        else if (ninthAxisTwo == 0)
        {
            otherDefenseShield.SetActive(false); // the other shield is inactive
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
        if (Input.GetKey(KeyCode.Joystick2Button4) && horizontalTwo == 1) // command input to use white arm style
        {
            usingWhiteArm = true; // switch to white arm style
            usingSlugFest = false;
            usingFireArm = false;
        }
        else if (Input.GetKey(KeyCode.Joystick2Button4) && seventhAxisTwo == -1 || Input.GetKey(KeyCode.Joystick2Button4) && verticalTwo == -1) // command input to use fire arm style
        {
            usingFireArm = true; // switch to fire arm style
            usingWhiteArm = false;
            usingSlugFest = false;
        }
        else if (Input.GetKey(KeyCode.Joystick2Button4) && horizontalTwo == -1) // command input to use slug fest style
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
        if (Input.GetKey(KeyCode.Joystick2Button2) && timeBetweenHits <= 0)
        {
            // throws a light punch
            Instantiate(slugFestAttack, festSpawn.transform.position, slugFestAttack.transform.rotation);
            timeBetweenHits = startTimeBetweenHits; // waits before another punch is thrown
        }
        // runs when the Y button is pressed
        else if (Input.GetKey(KeyCode.Joystick2Button3) && timeBetweenHits <= 0)
        {
            // throws a heavy punch
            Instantiate(slugFestAttackTwo, festSpawn.transform.position, slugFestAttack.transform.rotation);
            timeBetweenHits = startTimeBetweenHits; // waits before another punch is thrown
        }
        // runs when the A button is pressed
        else if (Input.GetKey(KeyCode.Joystick2Button0) && timeBetweenHits <= 0)
        {
            // does a light kick
            Instantiate(slugFestAttackThree, kickSpawn.transform.position, slugFestAttackThree.transform.rotation);
            timeBetweenHits = startTimeBetweenHits; // waits before another punch is thrown
        }
        // runs when the B button is pressed
        else if (Input.GetKey(KeyCode.Joystick2Button1) && timeBetweenHits <= 0)
        {
            // does a heavy kick
            Instantiate(slugFestAttackFour, kickSpawn.transform.position, slugFestAttackFour.transform.rotation);
            timeBetweenHits = startTimeBetweenHits; // waits before another punch is thrown
        }
        // runs when none of the buttons are being pressed
        else
        {
            // length of delay between bunches
            timeBetweenHits -= Time.deltaTime;
        }
    }

    // this is the fighting style that features the use of weapons that dosen't use bullets
    void WhiteArmStyle()
    {
        // runs when the X button is pressed
        if (Input.GetKey(KeyCode.Joystick2Button2) && timeBetweenStabs <= 0)
        {
            // does a light horizontal attack
            Instantiate(whiteArmAttack, whiteArmSpawn.transform.position, whiteArmAttack.transform.rotation);
            timeBetweenStabs = startTimeBetweenStabs; // waits before the weapon can be used again
        }
        // runs when the Y button is pressed
        else if (Input.GetKey(KeyCode.Joystick2Button3) && timeBetweenStabs <= 0)
        {
            // does a heavy horizontal attack
            Instantiate(whiteArmAttack2, whiteArmSpawn.transform.position, whiteArmAttack.transform.rotation);
            timeBetweenStabs = startTimeBetweenStabs; // waits before the weapon can be used again
        }
        // runs when the A button is pressed
        else if (Input.GetKey(KeyCode.Joystick2Button0) && timeBetweenStabs <= 0)
        {
            // does a light vertical attack
            Instantiate(whiteArmAttack3, whiteArmSpawn.transform.position, whiteArmAttack.transform.rotation);
            timeBetweenStabs = startTimeBetweenStabs; // waits before the weapon can be used again
        }
        // runs when the B button is pressed
        else if (Input.GetKey(KeyCode.Joystick2Button1) && timeBetweenStabs <= 0)
        {
            // does a heavy horizontal attack
            Instantiate(whiteArmAttack4, whiteArmSpawn.transform.position, whiteArmAttack.transform.rotation);
            timeBetweenStabs = startTimeBetweenStabs; // waits before the weapon can be used again
        }
        // While none of the buttons are being pressed
        else
        {
            timeBetweenStabs -= Time.deltaTime; // length of delay between White Arm attacks
        }
    }

    // this is the fighting style that features the use of weapons that does use bullets
    void FireArmStyle()
    {
        // runs when attack button is pressed and the player isn't facing right
        if (Input.GetKey(KeyCode.Joystick2Button2) && facingRight == false && timeBetweenShots <= 0)
        {
            // fires a bullet from fire arm
            Instantiate(fireArmAttack, fireArmSpawn.transform.position, fireArmAttack.transform.rotation);
            ammoLimit -= 1;
            ammoText.text = "Ammo: " + ammoLimit;
            timeBetweenShots = startTimeBetweenShots; // waits before fireing another bulleta
        }
        // runs when attack button is pressed and the player is facing right
        if (Input.GetKey(KeyCode.Joystick2Button2) && facingRight == true && timeBetweenShots <= 0)
        {
            // fires a bullet from fire arm
            Instantiate(reverseFireArmAttack, fireArmSpawn.transform.position, reverseFireArmAttack.transform.rotation);
            ammoLimit -= 1;
            ammoText.text = "Ammo: " + ammoLimit;
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
            if (Input.GetKeyDown(KeyCode.Joystick2Button1))
            {
                StartCoroutine(AmmoReload(2f)); // calls on the reload function
            }
        }
        else if (ammoLimit == maxAmmo) // when ammo is reloaded
        {
            canReload = false;
            StopCoroutine(AmmoReload(2f));
        }
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

        //lets the player move forward if pressing forward
        if (horizontalTwo == 1 && !Input.GetKey(KeyCode.Joystick2Button4))
        {
            MyRb.velocity = new Vector2(horizontalTwo * speed, currentYVal);
        }
        // lets the player move if pressing backward
        else if (horizontalTwo == -1 && !Input.GetKey(KeyCode.Joystick2Button4))
        {
            MyRb.velocity = new Vector2(-horizontalTwo * -speed, currentYVal);
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

        //if the player presses space while grounded, he or she jumps
        if (grounded == true)
        {
            MyRb.AddForce(new Vector2(0, verticalTwo * jumpForce)); // L-Stick Version
            MyRb.AddForce(new Vector2(0, seventhAxisTwo * jumpForce)); // D-Pad version
        }

        // checks to see if this player's x-position is less than the other player's x-position
        if (this.gameObject.transform.position.x < otherPlayer.transform.position.x)
        {
            imageSpawn.transform.position = new Vector2(6.6f, 6f);
            facingRight = true; // they are now facing right
            characterScale.x = -2.5f;
            characterScale.y = 2.5f;
        }
        else
        {
            imageSpawn.transform.position = new Vector2(6.6f, 6f);
            facingRight = false; // they are not facing right
            characterScale.x = 2.5f;
            characterScale.y = 2.5f;
        }

        transform.localScale = characterScale;
    }

    // this code runs when the player is attacked
    void OnCollisionEnter2D(Collision2D other)
    {
        // runs when the player is hit with a light punch and isn't blocking
        if (other.gameObject.tag == "Punch" && isBlocking == false)
        {
            playerTwoHealth -= 0.3f;
        }
        // runs when the player is hit with a light punch while blocking
        else if (other.gameObject.tag == "Punch" && isBlocking == true)
        {
            playerTwoHealth -= 0.03f;
        }
        // runs when the player is hit with a heavy punch while blocking
        else if (other.gameObject.tag == "OtherPunch" && isBlocking == false)
        {
            playerTwoHealth -= 0.6f;

            // these two if statements impliment knockback when hit by a heavy punch
            if (facingRight == true)
            {
                transform.position = new Vector2(this.transform.position.x - 1, this.transform.position.y);
                imageSpawn.transform.position = new Vector2(6.6f, 6f);
            }
            else if (facingRight == false)
            {
                transform.position = new Vector2(this.transform.position.x + 1, this.transform.position.y);
                imageSpawn.transform.position = new Vector2(6.6f, 6f);
            }
        }
        // runs when the player is hit with a heavy punch while blocking
        else if (other.gameObject.tag == "OtherPunch" && isBlocking == true)
        {
            playerTwoHealth -= 0.06f;
        }
        // runs when the player is hit with a light kick and is not blocking
        else if (other.gameObject.tag == "Kick" && isBlocking == false)
        {
            playerTwoHealth -= 0.4f;
        }
        // runs when the player is blocking while being hit by a light kick
        else if (other.gameObject.tag == "Kick" && isBlocking == true)
        {
            playerTwoHealth -= 0.04f;
        }
        // runs when the player is hit by a heavy kick and is not blocking
        if (other.gameObject.tag == "KickTwo" && isBlocking == false)
        {
            playerTwoHealth -= 0.8f;

            // these two if statements impliment knockback when hit by a heavy kick
            if (facingRight == true)
            {
                transform.position = new Vector2(this.transform.position.x - 1, this.transform.position.y);
                imageSpawn.transform.position = new Vector2(6.6f, 6f);
            }
            else if (facingRight == false)
            {
                transform.position = new Vector2(this.transform.position.x + 1, this.transform.position.y);
                imageSpawn.transform.position = new Vector2(6.6f, 6f);
            }
        }
        // runs when the player is blocking while being hit by a heavy kick
        else if (other.gameObject.tag == "KickTwo" && isBlocking == true)
        {
            playerTwoHealth -= 0.08f;
        }
        // runs when the player is hit by a light horizontal attack when not blocking
        else if (other.gameObject.tag == "Sword" && isBlocking == false)
        {
            playerTwoHealth -= 0.6f;
        }
        // runs when the player is hit by a light horizontal attack while blocking
        else if (other.gameObject.tag == "Sword" && isBlocking == true)
        {
            playerTwoHealth -= 0.06f;
        }
        // runs when the player is hit by a heavy horizontal attack when not blocking
        else if (other.gameObject.tag == "SecondSwordAttack" && isBlocking == false)
        {
            playerTwoHealth -= 0.9f;

            // these two if statements impliment knockback when hit by a heavy horizontal attack
            if (facingRight == true)
            {
                transform.position = new Vector2(this.transform.position.x - 2, this.transform.position.y);
                imageSpawn.transform.position = new Vector2(6.6f, 6f);
            }
            else if (facingRight == false)
            {
                transform.position = new Vector2(this.transform.position.x + 2, this.transform.position.y);
                imageSpawn.transform.position = new Vector2(6.6f, 6f);
            }
        }
        // runs when the player is hit by a heavy horizontal attack while blocking
        else if (other.gameObject.tag == "SecondSwordAttack" && isBlocking == true)
        {
            playerTwoHealth -= 0.09f;
        }
        // runs when the player is hit by a light vertical attack when not blocking
        else if (other.gameObject.tag == "ThirdSwordAttack" && isBlocking == false)
        {
            playerTwoHealth -= 0.4f;
        }
        // runs when the player is hit by a light vertical attack while blocking
        else if (other.gameObject.tag == "ThirdSwordAttack" && isBlocking == true)
        {
            playerTwoHealth -= 0.04f;
        }
        // runs when the player is hit by a heavy vertical attack when not blocking
        else if (other.gameObject.tag == "FourthSwordAttack" && isBlocking == false)
        {
            playerTwoHealth -= 0.7f;

            // these two if statements impliment knockback when hit by a heavy vertical attack
            if (facingRight == true)
            {
                transform.position = new Vector2(this.transform.position.x - 1, this.transform.position.y);
                imageSpawn.transform.position = new Vector2(6.6f, 6f);
            }
            else if (facingRight == false)
            {
                transform.position = new Vector2(this.transform.position.x + 1, this.transform.position.y);
                imageSpawn.transform.position = new Vector2(6.6f, 6f);
            }
        }
        // runs when the player is hit by a heavy vertical attack while blocking
        else if (other.gameObject.tag == "FourthSwordAttack" && isBlocking == true)
        {
            playerTwoHealth -= 0.07f;
        }
        // runs when the player is shot when not blocking
        else if (other.gameObject.tag == "Bullet" && isBlocking == false)
        {
            playerTwoHealth -= 0.2f;
        }
        // runs when the player is shot while blocking
        else if (other.gameObject.tag == "Bullet" && isBlocking == true)
        {
            playerTwoHealth -= 0.02f;
        }
    }
}

