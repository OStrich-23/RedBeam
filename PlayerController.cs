using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;
    private float _gravity = -9.81f;

    [SerializeField] private float gravityMultiplier = 3.0f;
    private float _velocity;
    private Camera _mainCamera;

    [SerializeField] private float rotationSpeed = 500f;
    [SerializeField] private float speed;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _mainCamera = Camera.main;

        if (_mainCamera == null)
        {
            Debug.LogError("Main Camera not found. Please ensure there is a camera tagged as MainCamera in the scene.");
        }
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (_mainCamera != null)
        {
            ApplyRotation();
        }
        
        ApplyGravity();
        ApplyMovement();
    }

    private void ApplyRotation()
    {
        if (_input.sqrMagnitude == 0) return;

        _direction = Quaternion.Euler(0.0f, _mainCamera.transform.eulerAngles.y, 0.0f) * new Vector3(_input.x, 0.0f, _input.y);
        var targetRotation = Quaternion.LookRotation(_direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        _direction.y = _velocity;
    }

    private void ApplyMovement()
    {
        _characterController.Move(_direction * speed * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0.0f, _input.y);
    }
}
