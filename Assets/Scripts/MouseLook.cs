using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    [SerializeField] float _mouseSensitivity = 1000;

    [SerializeField] Transform _playerBody;
    float _xRotation = 0f;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MouseLock>().MouseLocked(true);
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX =Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        _playerBody.Rotate(Vector3.up * mouseX);

    }
}
