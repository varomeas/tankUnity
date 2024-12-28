using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    [SerializeField] private int damage = 10; // Dégâts infligés par ce contrôleur
    [SerializeField] private HealthController playerHealth; // Référence au contrôleur de santé du joueur

    // Méthode appelée lorsqu'un autre collider entre en contact avec ce collider
    private void OnTriggerEnter(Collider other)
    {
        // Si l'objet entrant a le tag "Player" et que playerHealth n'est pas null
        if(other.CompareTag("Player") && playerHealth != null)
        {
            playerHealth.TakeDamage(damage); // Infliger des dégâts au joueur
        }

        // Si l'objet entrant a le tag "Obstacle"
        if(other.CompareTag("Obstacle"))
        {
            other.gameObject.GetComponent<HealthController>().TakeDamage(damage); // Infliger des dégâts à l'obstacle
            Destroy(gameObject); // Détruire cet objet après avoir infligé des dégâts
        }
    }
}
