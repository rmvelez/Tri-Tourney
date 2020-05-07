using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float bulletSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.right * bulletSpeed * Time.deltaTime);
    }

    // this checks for collisions with other game objects
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "PlayerOne")
        {
            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "PlayerTwo")
        {
            Destroy(this.gameObject);
        }
    }
}
