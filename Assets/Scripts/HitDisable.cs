using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDisable : MonoBehaviour
{
    public float durationTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        durationTime -= Time.deltaTime;

        if (durationTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
