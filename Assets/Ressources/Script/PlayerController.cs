using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Camera camera;
    public float movementSpeed = 1.0f;
    private Vector3 moveDirection = Vector3.zero;

    private float gravity = 100.0f;

    private float speedUp = 2.0f;

    private float xRotation, yRotation;
    public float lookSpeed = 2.0f;
    private float xLimitView = 45.0f;
    //Pour l'inclinaison du tank
    public Transform tankBody; // La partie du tank à incliner
    public float rayLength = 2f; // Longueur du rayon vers le sol
    public LayerMask terrainLayer; // Le layer du terrain

    public float rotationSpeedFactor = 1.0f;
    
    //shooter
    public Transform shooter;
    public GameObject missile1;
    public GameObject missile2;
    private GameObject currentMissile;
    public float shootPower = 10.0f;

    private float shotTimer = 1.0f;

    // Audio
    private AudioSource audioSource;
    public AudioClip movementSound;

    
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        camera = GetComponentInChildren<Camera>();
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        currentMissile = missile1;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = movementSound;
    }

    // Update is called once per frame
    void Update()
    {
        #region Mouvements
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float xSpeed = Input.GetAxis("Vertical");
        float yRotation = Input.GetAxis("Horizontal");
        if (Input.GetButton("Fire3"))
        {
            xSpeed *= speedUp;
            yRotation *= speedUp;
        }
        float movementDirectionY = moveDirection.y;
        moveDirection = forward * xSpeed;

        if (characterController.isGrounded == false)
        {
            moveDirection.y -= gravity * Time.deltaTime;
            Debug.Log("Je tombe");
        }
        transform.Rotate(0, yRotation * rotationSpeedFactor * Time.deltaTime, 0);
        characterController.Move(moveDirection*Time.deltaTime*movementSpeed);

        // Jouer le son de déplacement
        if (xSpeed != 0 && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (xSpeed == 0 && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        #endregion

        #region Rotation
        xRotation += -Input.GetAxis("Mouse Y") * lookSpeed; //quand on monte ou descend la souris, on fait la rotation de la cam�ra sur x
        xRotation = Mathf.Clamp(xRotation, -xLimitView, xLimitView);
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        #endregion

        #region Inclinaison
        AlignWithTerrain();
        #endregion

        #region Canon Rotation
        Transform canon = tankBody.transform.Find("turret");
        if(Input.GetKey(KeyCode.Q))
        {
            canon.Rotate(0, -rotationSpeedFactor * Time.deltaTime, 0);
        }
        else if(Input.GetKey(KeyCode.E))
        {
            canon.Rotate(0, rotationSpeedFactor * Time.deltaTime, 0);
        }
        #endregion

        #region Tir
        shotTimer -= Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && shotTimer < 0)
        {
            tirerMissile(currentMissile);
            shotTimer = 1.0f;
        }
        if(Input.GetKeyDown(KeyCode.R)){
            SwitchMissile();
        }
        #endregion
    }

    void AlignWithTerrain()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayLength, terrainLayer))
        {
            Vector3 normal = hit.normal;
            Quaternion rotation = Quaternion.FromToRotation(tankBody.up, normal) * tankBody.rotation;
            tankBody.rotation = Quaternion.Lerp(tankBody.rotation, rotation, Time.deltaTime * 5f);

        }
    }

    public void SwitchMissile(){
        if(currentMissile == missile1){
            currentMissile = missile2;
        }
        else{
            currentMissile = missile1;
        }
    }

    void tirerMissile(GameObject prefab)
    {
        GameObject missile = Instantiate(prefab, shooter.position, shooter.rotation);
        missile.GetComponent<Rigidbody>().velocity = shooter.forward * shootPower;
    }
}
