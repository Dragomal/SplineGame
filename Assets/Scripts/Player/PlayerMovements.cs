using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
public class PlayerMovements : MonoBehaviour
{
    [SerializeField, Range(0,20)] private float _movementSpeed;
    [SerializeField, Range(0,50)] private float _jumpPower;
    [SerializeField, Range(0,1)] private float _airControl;
    [SerializeField, Range(0,4)] private float _gravityScaleJump;
    [SerializeField, Range(0,4)] private float _gravityScaleRelease;
    [SerializeField, Range(0,4)] private float _gravityScaleNormal;
    [SerializeField] private ParticleSystem _particleSystem;
    
    //Touche un mur ou non
    private bool _isWalled = false, _isWallJumpable;
    private bool _jumpAction;
    public bool _CanDoubleJump{
        get {return _canDoubleJump;}
        set {
            _canDoubleJump = value;
            PlayerState playerState = GetComponent<PlayerState>();
            playerState.ChangeJetpack(_canDoubleJump);
        }
    }
    private bool _canDoubleJump;
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
        if(_rigidbody2D.velocity.y == 0 && _boxGroundSensor.IsGrounded()){
            if(_isWalled){
                _isWalled = false;
            }
        }
        if(_jumpAction){
            if(_isWalled && _isWallJumpable && !_boxGroundSensor.IsGrounded()){
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
            //! A voir François : Pk il est trop vénère sur le WallJump
            float airMoveAction = Mathf.Lerp(_rigidbody2D.velocity.x, _moveAction.x * _movementSpeed, _airControl);
            _rigidbody2D.velocity = new Vector2(airMoveAction, _rigidbody2D.velocity.y);
        }
    }
    private void ReadValues(){
        _jumpAction = _onJumpAction.ReadValue<float>() > 0.5f ? true : false;
        _moveAction = _onMoveAction.ReadValue<Vector2>();
    }

    public void OnCollisionEnter2D(Collision2D collision){
        float xPos = collision.contacts[0].point.x;
        float yPos = collision.contacts[0].point.y;
        Vector2 direction = collision.contacts[0].normal;
        Quaternion rotation = Quaternion.Euler(direction.x, direction.y, 0);
        Instantiate(_particleSystem, new Vector3(xPos, yPos, 0), rotation);

        //Récupère le mur touché et sa normal
        _normalOfWall = collision.contacts[0].normal;
        if(_normalOfWall == Vector2.up) return;
        
        //Pose le joueur sur le mur et le fait glisser lentement
        if(collision.gameObject.CompareTag("Wall")){
            _isWalled = true;
            _isWallJumpable = true;
            
            float xVelocity = _rigidbody2D.velocity.x;
            float yVelocity = _rigidbody2D.velocity.y;
            _rigidbody2D.velocity = new Vector2(0, (xVelocity * 1.5f) + yVelocity);

            _rigidbody2D.gravityScale /= 2;
            // StartCoroutine(WaitToMove());
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
        if(!_boxGroundSensor.IsGrounded()){
            if(_canDoubleJump && !_jumpAction && !_isWalled){
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
                _rigidbody2D.AddForce(Vector2.up * _jumpPower * 10, ForceMode2D.Impulse);
                _rigidbody2D.gravityScale = _gravityScaleJump;
                _CanDoubleJump = false;
                return;
            }
            else{
                return;
            }
        }
        _rigidbody2D.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        _rigidbody2D.gravityScale = _gravityScaleJump;
    }
    private void OnWallJump(Vector2 normalOfWall){
        //Fait un saut en fonction de la direction du mur
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
        _rigidbody2D.AddForce(new Vector2(normalOfWall.x * 20, 15) * _jumpPower, ForceMode2D.Impulse);
        _rigidbody2D.gravityScale *= 2;
        _isWalled = false;
    }
    IEnumerator WaitToMove(){
        yield return new WaitForSeconds(0.5f);
        _isWalled = false;
    }
}
