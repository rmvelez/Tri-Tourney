using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTwoHealthManager : MonoBehaviour
{
    Vector3 localScale; // variable for player two HealthBar

    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale; // current scale of player two health bar
    }

    // Update is called once per frame
    void Update()
    {
        // sets localscale equal to player two health in Player Two script
        localScale.x = PlayerTwo.playerTwoHealth;
        transform.localScale = localScale;

        // once player two health is zero, the object destroys itself
        if (PlayerTwo.playerTwoHealth <= 0)
        {
            Destroy (this.gameObject);
        }
    }
}
