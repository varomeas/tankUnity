using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region Variables
    private CharacterController characterController; // Référence au CharacterController pour gérer les mouvements du tank
    private Camera camera; // Référence à la caméra pour suivre le tank
    public float movementSpeed = 1.0f; // Vitesse de déplacement du tank
    private Vector3 moveDirection = Vector3.zero; // Direction du mouvement du tank

    private float gravity = 100.0f; // Force de gravité appliquée au tank
    private float speedUp = 2.0f; // Facteur de vitesse lorsque le joueur appuie sur une touche pour accélérer
    private float xRotation, yRotation; // Variables pour stocker les rotations de la caméra
    public float lookSpeed = 2.0f; // Vitesse de rotation de la caméra
    private float xLimitView = 45.0f; // Limite de la rotation verticale de la caméra

    // Pour l'inclinaison du tank
    public Transform tankBody; // La partie du tank à incliner
    public float rayLength = 2f; // Longueur du rayon vers le sol pour détecter les collisions
    public LayerMask terrainLayer; // Le layer du terrain pour les détections de collision

    public float rotationSpeedFactor = 1.0f; // Facteur de vitesse de rotation du canon

    // Shooter
    public Transform shooter; // Référence au point de tir du tank
    public GameObject missile1; // Référence au premier type de missile
    public GameObject missile2; // Référence au deuxième type de missile
    private GameObject currentMissile; // Référence au missile actuellement sélectionné
    public float shootPower = 10.0f; // Puissance de tir des missiles

    private float shotTimer = 1.0f; // Timer pour gérer le délai entre les tirs

    // Audio
    private AudioSource audioSource; // Référence à l'AudioSource pour jouer les sons
    public AudioClip movementSound; // Clip audio pour le son de déplacement
    private AudioSource engineAudioSource; // Référence à l'AudioSource pour le son du moteur
    public AudioClip engineSound; // Clip audio pour le son du moteur

    [SerializeField] private GameObject endScreen; // Référence à l'écran de fin de jeu
    #endregion
    void Start()
    {
        characterController = GetComponent<CharacterController>(); // Initialiser le CharacterController
        camera = GetComponentInChildren<Camera>(); // Initialiser la caméra
        audioSource = GetComponent<AudioSource>(); // Initialiser l'AudioSource pour le son de déplacement
        audioSource.clip = movementSound; // Assigner le clip audio de déplacement à l'AudioSource

        // Initialiser le son de fond du moteur
        engineAudioSource = gameObject.AddComponent<AudioSource>(); // Ajouter un AudioSource pour le son du moteur
        engineAudioSource.clip = engineSound; // Assigner le clip audio du moteur à l'AudioSource
        engineAudioSource.loop = true; // Activer la boucle pour le son du moteur
        engineAudioSource.Play(); // Jouer le son du moteur

        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        currentMissile = missile1; // Initialiser le missile actuel avec missile1
    }

    void Update()
    {
        #region Mouvements
        Vector3 forward = transform.TransformDirection(Vector3.forward); // Direction avant du tank
        Vector3 right = transform.TransformDirection(Vector3.right); // Direction droite du tank

        float xSpeed = Input.GetAxis("Vertical"); // Vitesse de déplacement avant/arrière
        float yRotation = Input.GetAxis("Horizontal"); // Rotation du tank
        if (Input.GetButton("Fire3"))
        {
            xSpeed *= speedUp; // Augmenter la vitesse de déplacement si le joueur appuie sur une touche spécifique
            yRotation *= speedUp; // Augmenter la vitesse de rotation si le joueur appuie sur une touche spécifique
        }
        float movementDirectionY = moveDirection.y; // Conserver la composante Y de la direction de mouvement
        moveDirection = forward * xSpeed; // Calculer la direction de mouvement

        if (characterController.isGrounded == false)
        {
            moveDirection.y -= gravity * Time.deltaTime; // Appliquer la gravité si le tank n'est pas au sol
            Debug.Log("Je tombe"); // Afficher un message de débogage
        }
        transform.Rotate(0, yRotation * rotationSpeedFactor * Time.deltaTime, 0); // Appliquer la rotation au tank
        characterController.Move(moveDirection * Time.deltaTime * movementSpeed); // Déplacer le tank

        // Jouer le son de déplacement
        if (xSpeed != 0 && !audioSource.isPlaying)
        {
            audioSource.Play(); // Jouer le son de déplacement si le tank se déplace
        }
        else if (xSpeed == 0 && audioSource.isPlaying)
        {
            audioSource.Stop(); // Arrêter le son de déplacement si le tank ne se déplace pas
        }
        #endregion

        #region Rotation
        xRotation += -Input.GetAxis("Mouse Y") * lookSpeed; // Rotation verticale de la caméra
        xRotation = Mathf.Clamp(xRotation, -xLimitView, xLimitView); // Limiter la rotation verticale de la caméra
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0); // Appliquer la rotation verticale à la caméra

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0); // Appliquer la rotation horizontale au tank
        #endregion

        #region Inclinaison
        AlignWithTerrain(); // Aligner le tank avec le terrain
        #endregion

        #region Canon Rotation
        Transform canon = tankBody.transform.Find("turret"); // Trouver la tourelle du tank
        if (Input.GetKey(KeyCode.Q))
        {
            canon.Rotate(0, -rotationSpeedFactor * Time.deltaTime, 0); // Tourner la tourelle vers la gauche
        }
        else if (Input.GetKey(KeyCode.E))
        {
            canon.Rotate(0, rotationSpeedFactor * Time.deltaTime, 0); // Tourner la tourelle vers la droite
        }
        #endregion

        #region Tir
        shotTimer -= Time.deltaTime; // Décrémenter le timer de tir
        if (Input.GetKey(KeyCode.Space) && shotTimer < 0)
        {
            tirerMissile(currentMissile); // Tirer un missile si le timer est écoulé
            shotTimer = 1.0f; // Réinitialiser le timer de tir
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentMissile == missile1)
            {
                SetMissile2(); // Changer pour le missile2 si le missile actuel est missile1
            }
            else
            {
                SetMissile1(); // Changer pour le missile1 si le missile actuel est missile2
            }
        }
        #endregion
    }

    void AlignWithTerrain()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayLength, terrainLayer))
        {
            Vector3 normal = hit.normal; // Normale de la surface du terrain
            Quaternion rotation = Quaternion.FromToRotation(tankBody.up, normal) * tankBody.rotation; // Calculer la rotation pour aligner le tank avec le terrain
            tankBody.rotation = Quaternion.Lerp(tankBody.rotation, rotation, Time.deltaTime * 5f); // Appliquer la rotation de manière lissée
        }
    }

    public void SetMissile1()
    {
        currentMissile = missile1; // Changer pour le missile1
    }

    public void SetMissile2()
    {
        currentMissile = missile2; // Changer pour le missile2
    }

    void tirerMissile(GameObject prefab)
    {
        GameObject missile = Instantiate(prefab, shooter.position, shooter.rotation); // Instancier le missile
        missile.GetComponent<Rigidbody>().velocity = shooter.forward * shootPower; // Appliquer une force au missile pour le faire avancer
    }

    public void OnTankDestroyed()
    {
        // Détacher la caméra du tank
        camera.transform.SetParent(null);

        // Afficher l'écran de fin
        endScreen.SetActive(true);

        // Détruire le tank
        Destroy(gameObject);
    }
}
