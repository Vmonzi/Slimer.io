using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public static CameraControl Instance;

    [Header("FollowTarget")]
    [SerializeField] private Transform _target;
    [SerializeField] private float _smoothSpeed = 5.0f;
    [SerializeField] private float _rotationSpeed = 2.0f;
    [SerializeField] private float _distance = 5.0f;
    [SerializeField] private float _height = 2.0f;
    private float _rotationX = 0.0f;
    private float _rotationY = 0.0f;

    public Transform Target { set { _target = value; } }

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (_target != null)
        {
            // Obtener la entrada del mouse
            float mouseX = Input.GetAxis("Mouse X") * _rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * _rotationSpeed;

            // Actualizar las rotaciones en los ejes X e Y
            _rotationX += mouseY;
            _rotationY += mouseX;

            // Limitar la rotaci�n en el eje X entre -90 y 90 grados para evitar voltear la c�mara
            _rotationX = Mathf.Clamp(_rotationX, -50f, 50f);

            // Aplicar la rotaci�n a la c�mara
            transform.rotation = Quaternion.Euler(-_rotationX, _rotationY, 0);

            // Calcular la nueva posici�n de la c�mara
            Vector3 offset = -transform.forward * _distance;
            Vector3 desiredPosition = _target.position + offset + Vector3.up * _height; // A�adir altura

            // Posici�n suavizada
            transform.position = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);
            transform.LookAt(_target);
        }
    }
}
