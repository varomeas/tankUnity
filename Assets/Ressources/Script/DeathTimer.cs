using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTimer : MonoBehaviour
{
    public float timer = 5.0f;
    public GameObject particles;

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            if (particles != null)
            {
                Instantiate(particles, transform.position, transform.rotation);
            }
            
            Destroy(gameObject);
        }
    }

}
