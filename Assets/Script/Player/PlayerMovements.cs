using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.iOS;
public class PlayerMovements : MonoBehaviour
{
    // [SerializeField, Range(1, 20)] private float _movementSpeed = 5;
    // private Rigidbody2D _rigidBody2D;
    // private InputAction _moveInputAction, _jumpInmputAction;
    // private Vector2 _moveInput = Vector2.zero;
    // private bool _jumpInput;

    
    // void Start()
    // {
    //     _rigidBody2D = GetComponent<Rigidbody2D>();
    //     PlayerInput playerInput = GetComponent<PlayerInput>();
    //     _moveInputAction = playerInput.actions.FindAction("Move");
    //     _jumpInmputAction = playerInput.actions.FindAction("Jump");
    // }
    // void FixedUpdate(){
    //     _moveInput = _moveInputAction.ReadValue<Vector2>();
    //     Jump();
    //     _rigidBody2D.velocity = new Vector2(_movementSpeed * _moveInput.x, _rigidBody2D.velocity.y);
    // }
    // void Jump(){
    //     RaycastHit2D hit = Physics2D.CircleCast(this.transform.position, 0.5f, Vector2.down, 1f);
        
    //     _jumpInput = _jumpInmputAction.ReadValue<float>() > 0.5f ? true : false;
    //     if(_jumpInput && hit){
    //         _rigidBody2D.AddForce(new Vector2(_rigidBody2D.velocity.x, 1), ForceMode2D.Impulse);
    //     }
    // }
    [SerializeField, Range(0,10)] private float _movementSpeed;
    [SerializeField, Range(0,50)] private float _jumpPower;
    [SerializeField, Range(0,1)] private float _airControl;
    [SerializeField, Range(0,4)] private float _gravityScaleJump;
    [SerializeField, Range(0,4)] private float _gravityScaleRelease;
    [SerializeField, Range(0,4)] private float _gravityScaleNormal;
    
    private bool _isGrounded;
    private bool _jumpAction;
    private Vector2 _moveAction;
    private Rigidbody2D _rigidbody2D;
    private BoxGroundSensor _boxGroundSensor;
    private InputAction _onMoveAction;
    private InputAction _onJumpAction;
    private void Start(){
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxGroundSensor = GetComponent<BoxGroundSensor>();

        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _onMoveAction = _playerInput.actions.FindAction("Move");
        _onJumpAction = _playerInput.actions.FindAction("Jump");
    }
    private void FixedUpdate(){
        ReadValues();
        UpdateHorizontalMovements();
        UpdateGravityScale();
        if(_jumpAction){
            OnJump();
        }
    }

    private void UpdateGravityScale(){
        if (_rigidbody2D.velocity.y < 0){
            _rigidbody2D.gravityScale = _gravityScaleNormal;
        }
        if (_rigidbody2D.velocity.y > 0 && !_jumpAction){
            _rigidbody2D.gravityScale = _gravityScaleRelease;
        }
    }

    private void UpdateHorizontalMovements(){
        if (_boxGroundSensor.IsGrounded()){
            _rigidbody2D.velocity = new Vector2(_moveAction.x * _movementSpeed, _rigidbody2D.velocity.y);
        }
        else{
            float airMoveAction = Mathf.Lerp(_rigidbody2D.velocity.x, _moveAction.x * _movementSpeed, _airControl);
            _rigidbody2D.velocity = new Vector2(airMoveAction, _rigidbody2D.velocity.y);
        }
    }

    private void ReadValues(){
        _jumpAction = _onJumpAction.ReadValue<float>() > 0.5f ? true : false;
        _moveAction = _onMoveAction.ReadValue<Vector2>();
    }

    private void OnJump(){
        if(!_boxGroundSensor.IsGrounded()) return;
        
        _rigidbody2D.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        _rigidbody2D.gravityScale = _gravityScaleJump;
    }
}
