using UnityEngine;
using UnityEngine.UI;

public class ColorSelector : MonoBehaviour
{
    public GameObject tank; // Référence au tank
    public Button redButton;
    public Button greenButton;
    public Button blueButton;
    public Material redMaterial;
    public Material greenMaterial;
    public Material blueMaterial;
    public GameObject uiPanel; // Référence au panneau UI

    void Start()
    {
        redButton.onClick.AddListener(() => SetTankMaterial(redMaterial));
        greenButton.onClick.AddListener(() => SetTankMaterial(greenMaterial));
        blueButton.onClick.AddListener(() => SetTankMaterial(blueMaterial));
    }

    void SetTankMaterial(Material material)
    {
        Renderer[] renderers = tank.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = material;
        }
        uiPanel.SetActive(false); // Masquer l'interface utilisateur
    }
}