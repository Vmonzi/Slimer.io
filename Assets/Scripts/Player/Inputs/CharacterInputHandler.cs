using UnityEngine;

//Clase que va a tomar los inputs del jugador siempre y cuando tenga autoridad para hacerlo
public class CharacterInputHandler : MonoBehaviour
{
    Vector3 _move;
    bool _isJumpPressed;
    bool _isFirePressed;
    bool _isDashPressed;

    NetworkInputData _inputData;

    void Start()
    {
        _inputData = new NetworkInputData();
    }

    void Update()
    {
        //Tomo todos los Inputs

        _move = Input.GetAxis("Horizontal") * CameraControl.Instance.transform.right + Input.GetAxis("Vertical") * CameraControl.Instance.transform.forward;
        _move.y = 0;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isJumpPressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _isFirePressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isDashPressed = true;
        }
    }

    public NetworkInputData GetNetworkInputs()
    {
        _inputData.move = _move.normalized;

        _inputData.isJumpPressed = _isJumpPressed;
        _isJumpPressed = false;

        _inputData.isFirePressed = _isFirePressed;
        _isFirePressed = false;

        _inputData.isDashPressed = _isDashPressed;
        _isDashPressed = false;

        return _inputData;
    }
}
