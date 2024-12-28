using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    // Référence au script PlayerController pour gérer les événements liés au joueur
    [SerializeField] private PlayerController playerController;

    // Points de vie maximum et points de vie actuels
    [SerializeField] private int maxhealth = 100;
    private int currentHealth;

    // Slider pour afficher les points de vie dans l'interface
    public Slider slider;

    // Image de remplissage du slider et ses couleurs selon le pourcentage de vie
    [SerializeField] private Image fillImage;
    private Color fullColor = Color.green; // Couleur à pleine vie
    private Color midHealthColor = Color.yellow; // Couleur à vie moyenne
    private Color emptyColor = Color.red; // Couleur à faible vie

    // Effet de particules déclenché à la destruction
    [SerializeField] private ParticleSystem deathEffect;

    void Start()
    {
        // Initialisation des points de vie actuels et du slider (si configuré)
        currentHealth = maxhealth;
        if (slider != null)
        {
            slider.maxValue = maxhealth;
            slider.value = currentHealth;
        }

        // Initialisation de la couleur de remplissage de la barre de vie
        if (fillImage != null)
        {
            fillImage.color = fullColor;
        }
    }

    public void TakeDamage(int damage)
    {
        // Réduction des points de vie et mise à jour du slider
        currentHealth -= damage;
        if (slider != null)
        {
            slider.value = currentHealth;
        }

        // Changement dynamique de la couleur de la barre de vie selon le pourcentage de vie
        if (fillImage != null)
        {
            float healthPercentage = (float)currentHealth / maxhealth;

            // Transition de vert à jaune
            if (healthPercentage > 0.6f)
            {
                float t = (healthPercentage - 0.6f) / 0.6f; // Normalisation entre 0 et 1
                fillImage.color = Color.Lerp(midHealthColor, fullColor, t);
            }
            // Transition de jaune à rouge
            else
            {
                float t = healthPercentage / 0.6f; // Normalisation entre 0 et 1
                fillImage.color = Color.Lerp(emptyColor, midHealthColor, t);
            }
        }

        // Si les points de vie tombent à 0 ou moins
        if (currentHealth <= 0)
        {
            // Gérer la destruction du joueur
            if (gameObject.CompareTag("Player"))
            {
                playerController.OnTankDestroyed();
            }
            // Gérer la destruction des ennemis/obstacles
            else
            {
                Destroy(gameObject);
                if (deathEffect != null)
                {
                    Instantiate(deathEffect, transform.position, Quaternion.identity);
                }
            }
        }
    }
}
