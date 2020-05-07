using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneHealthManager : MonoBehaviour
{
    Vector3 localScale; // variable for player one HealthBar
    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale; // current scale of player one health bar
    }

    // Update is called once per frame
    void Update()
    {
        // sets localscale equal to player one health in Player One script
        localScale.x = PlayerOne.playerHealth;
        transform.localScale = localScale;

        // once player one health is zero, the object destroys itself
        if (PlayerOne.playerHealth <= 0)
        {
            Destroy (this.gameObject);
        }
    }
}
