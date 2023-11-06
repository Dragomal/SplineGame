using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.iOS;
public class PlayerMovements : MonoBehaviour
{
    [SerializeField, Range(0,20)] private float _movementSpeed;
    [SerializeField, Range(0,50)] private float _jumpPower;
    [SerializeField, Range(0,1)] private float _airControl;
    [SerializeField, Range(0,4)] private float _gravityScaleJump;
    [SerializeField, Range(0,4)] private float _gravityScaleRelease;
    [SerializeField, Range(0,4)] private float _gravityScaleNormal;
    
    //Touche un mur ou non
    private bool _isWalled = false, _isWallJumpable;
    private bool _jumpAction;
    private Vector2 _moveAction, _normalOfWall;
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
        if(!_isWalled){
            UpdateHorizontalMovements();
        }
        UpdateGravityScale();
        if(_jumpAction){
            if(_isWalled && _isWallJumpable){
                OnWallJump(_normalOfWall);
            }
            else{
                OnJump();
            }
        }
    }

    private void UpdateGravityScale(){
        if(_isWalled) return;
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

    public void OnCollisionEnter2D(Collision2D collision){
        //Récupère le mur touché et sa normal
        _normalOfWall = collision.contacts[0].normal;
        if(_normalOfWall == Vector2.up) return;
        
        //Pose le joueur sur le mur et le fait glisser lentement
        if(collision.gameObject.CompareTag("Wall")){
            _isWalled = true;
            _isWallJumpable = true;
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.gravityScale /= 2;
            StartCoroutine(WaitToMove());
        }
    }
    public void OnCollisionExit2D(Collision2D collision){
        //Réactive les mouvements du joueur si il part du mur
        if(collision.gameObject.CompareTag("Wall")){
            _isWalled = false;
            _rigidbody2D.gravityScale *= 2;
        }
    }
    private void OnJump(){
        if(!_boxGroundSensor.IsGrounded()) return;
        _rigidbody2D.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        _rigidbody2D.gravityScale = _gravityScaleJump;
    }
    private void OnWallJump(Vector2 normalOfWall){
        //Fait un saut en fonction de la direction du mur
        _rigidbody2D.AddForce(new Vector2(normalOfWall.x, 1) * _jumpPower * 15f, ForceMode2D.Impulse);
        _rigidbody2D.gravityScale *= 2;
        _isWalled = false;
    }
    IEnumerator WaitToMove(){
        yield return new WaitForSeconds(0.5f);
        _isWalled = false;
    }
}
