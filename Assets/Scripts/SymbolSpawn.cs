using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolSpawn : MonoBehaviour
{
    public GameObject spawnerOne; // reference for the two spanners that load the images
    public GameObject spawnerTwo;
    public GameObject fistImage; // reference to the images that are shown off
    public GameObject bladeImage;
    public GameObject ammoImage;

    // these are the axes that are called upon by player one
    private float horizontal;
    private float vertical;
    private float seventhAxis;

    // these are the axes that are called upon by player two
    private float horizontalTwo; 
    private float verticalTwo;
    private float seventhAxisTwo;

    //private PlayerOne playerOne;
    //private PlayerTwo playerTwo;

    void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // sets the var to the horizontal axis
        vertical = Input.GetAxisRaw("Vertical"); // sets var for the vertical axis
        seventhAxis = Input.GetAxisRaw("7th Axis"); // gets the 7th axis

        horizontalTwo = Input.GetAxisRaw("HorizontalTwo"); // sets the var to the horizontal axis
        verticalTwo = Input.GetAxisRaw("VerticalTwo"); // sets var for the vertical axis
        seventhAxisTwo = Input.GetAxisRaw("7th Axis Two"); // sets var to the seventh axis
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Joystick1Button4) && horizontal == 1)
        {
            Instantiate(bladeImage, spawnerOne.transform.position, bladeImage.transform.rotation);
        }
        else if (Input.GetKey(KeyCode.Joystick2Button4) && horizontalTwo == 1)
        {
            Instantiate(bladeImage, spawnerTwo.transform.position, bladeImage.transform.rotation);
        }

        if (Input.GetKey(KeyCode.Joystick1Button4) && horizontal == -1)
        {
            Instantiate(fistImage, spawnerOne.transform.position, fistImage.transform.rotation);
        }
        else if (Input.GetKey(KeyCode.Joystick2Button4) && horizontalTwo == -1)
        {
            Instantiate(fistImage, spawnerTwo.transform.position, fistImage.transform.rotation);
        }
    }
}
