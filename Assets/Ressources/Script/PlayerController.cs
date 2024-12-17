using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Camera camera;
    public float movementSpeed = 1.0f;

    private float gravity = 5.0f;

    private float speedUp = 2.0f;

    private float xRotation, yRotation;
    public float lookSpeed = 2.0f;
    private float xLimitView = 45.0f;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        camera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        #region Mouvements
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float xSpeed = Input.GetAxis("Vertical");
        float ySpeed = Input.GetAxis("Horizontal");
        if (Input.GetButton("Fire3"))
        {
            xSpeed *= speedUp;
            ySpeed *= speedUp;
        }
        Vector3 moveDirection = forward * xSpeed + right * ySpeed;
        float movementDirectionY = moveDirection.y;

        // if (characterController.isGrounded == false)
        // {
        //     moveDirection.y -= gravity * Time.deltaTime;
        //     Debug.Log("Je tombe");
        // }
        characterController.Move(moveDirection*Time.deltaTime*movementSpeed);
        #endregion

        #region Rotation
        xRotation += -Input.GetAxis("Mouse Y") * lookSpeed; //quand on monte ou descend la souris, on fait la rotation de la cam�ra sur x
        xRotation = Mathf.Clamp(xRotation, -xLimitView, xLimitView);
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        #endregion
    }
}
