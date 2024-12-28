using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChooseColor : MonoBehaviour
{
    public GameObject player;
    // Références aux parties du tank
    public GameObject corpsTank;  // Le corps principal du tank
    public GameObject tourelleTank; // La tourelle du tank (partie supérieure)

    // Matériaux disponibles pour changer la couleur du tank
    public Material redMaterial;  // Matériau rouge
    public Material greenMaterial; // Matériau vert
    public Material blueMaterial;  // Matériau bleu

    // Référence au panneau d'interface utilisateur pour la sélection de couleurs
    public GameObject uiPanel;


    // Méthode appelée pour appliquer le matériau rouge au tank
    public void SetRedMaterial()
    {
        SetTankMaterial(redMaterial);
    }

    // Méthode appelée pour appliquer le matériau vert au tank
    public void SetGreenMaterial()
    {
        SetTankMaterial(greenMaterial);
    }

    // Méthode appelée pour appliquer le matériau bleu au tank
    public void SetBlueMaterial()
    {
        SetTankMaterial(blueMaterial);
    }

    // Méthode privée qui applique le matériau sélectionné à toutes les parties du tank
    private void SetTankMaterial(Material material)
    {
        // Récupération des rendus (Renderers) de toutes les sous-parties du corps du tank
        Renderer[] corpsRenderers = corpsTank.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in corpsRenderers)
        {
            renderer.material = material; // Appliquer le matériau à chaque renderer du corps
        }

        // Récupération des rendus (Renderers) de toutes les sous-parties de la tourelle du tank
        Renderer[] tourelleRenderers = tourelleTank.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in tourelleRenderers)
        {
            renderer.material = material; // Appliquer le matériau à chaque renderer de la tourelle
        }

        // Masquer le panneau de sélection des couleurs après le choix 
    }

    public void StartGame()
    {
        player.SetActive(true);
        uiPanel.SetActive(false);
    }
}
