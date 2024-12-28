using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthController : MonoBehaviour
{

    //points de vie
    [SerializeField] private int maxhealth = 100;
    private int currentHealth;
    public Slider slider;

    [SerializeField] private Image fillImage;
    private Color fullColor = Color.green;
     private Color midHealthColor = Color.yellow;
    private Color emptyColor = Color.red;
    // Start is called before the first frame update
    [SerializeField] private ParticleSystem deathEffect;
    void Start()
    {
        currentHealth = maxhealth;
        if(slider != null){
            slider.maxValue = maxhealth;
            slider.value = currentHealth;
        }
        

        if(fillImage != null){
            fillImage.color = fullColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(slider != null){
            slider.value = currentHealth;
        }
        if(fillImage != null){
            float healthPercentage = (float)currentHealth / maxhealth;

            if (healthPercentage > 0.6f) // Vert vers Jaune
            {
                float t = (healthPercentage - 0.6f) / 0.6f; // Normaliser entre 0 et 1
                fillImage.color = Color.Lerp(midHealthColor, fullColor, t);
            } 
            else // Jaune vers Rouge
            {
                float t = healthPercentage / 0.6f; // Normaliser entre 0 et 1
                fillImage.color = Color.Lerp(emptyColor, midHealthColor, t);
            }
        }
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            if(deathEffect != null){
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            }
        }
    }   
}
