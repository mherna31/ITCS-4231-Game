using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float minVertical = -80f;
    public float maxVertical = 80f;
    public float minHorizontal = -360f;
    public float maxHorizontal = 360;

    public float mouseSensitivity = 20f;

    public Camera cam;

    private float verticalRotation;
    private float horizontalRotation;

    Vector3 cameraOffset;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cameraOffset = cam.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.GetComponent<PlayerMove>().endGame)
        {
            horizontalRotation += Input.GetAxis("Mouse X") * mouseSensitivity;
            verticalRotation += Input.GetAxis("Mouse Y") * mouseSensitivity;

            verticalRotation = Mathf.Clamp(verticalRotation, minVertical, maxVertical);

            transform.localEulerAngles = new Vector3(0, horizontalRotation, 0);

            cam.transform.localEulerAngles = new Vector3(-verticalRotation, horizontalRotation, 0);
        }
    }

    private void LateUpdate()
    {
        cam.transform.position = transform.position + cameraOffset;
    }

}
