using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.iOS;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField, Range(1, 20)] private float _movementSpeed = 5;
    private Rigidbody2D _rigidBody2D;
    private InputAction _moveInputAction, _jumpInmputAction;
    private Vector2 _moveInput = Vector2.zero;
    private bool _jumpInput;
    
    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        PlayerInput playerInput = GetComponent<PlayerInput>();
        _moveInputAction = playerInput.actions.FindAction("Move");
        _jumpInmputAction = playerInput.actions.FindAction("Jump");
    }
    void FixedUpdate(){
        _moveInput = _moveInputAction.ReadValue<Vector2>();
        Jump();
        _rigidBody2D.velocity = new Vector2(_movementSpeed * _moveInput.x, _rigidBody2D.velocity.y);
    }
    void Jump(){
        
        _jumpInput = _jumpInmputAction.ReadValue<float>() > 0.5f ? true : false;
        // _rigidBody2D.AddForce(new Vector2(_rigidBody2D.velocity.x, _jumpInput), ForceMode2D.Impulse);
    }
}
