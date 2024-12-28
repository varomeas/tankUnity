using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private HealthController playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        if(other.CompareTag("Obstacle"))
        {
            other.gameObject.GetComponent<HealthController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
