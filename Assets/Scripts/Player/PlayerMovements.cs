using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
public class PlayerMovements : MonoBehaviour
{
    [SerializeField, Range(0, 20)] private float _movementSpeed;
    [SerializeField, Range(0, 50)] private float _jumpPower;
    [SerializeField, Range(0, 100)] private float _airControl;
    [SerializeField, Range(0, 4)] private float _gravityScaleJump;
    [SerializeField, Range(0, 4)] private float _gravityScaleRelease;
    [SerializeField, Range(0, 4)] private float _gravityScaleNormal;
    [SerializeField] private Transform _particleSystemAnchor;
    [SerializeField] private ParticleSystem _particleSystem;

    //Touche un mur ou non
    private bool _isWalled = false, _isWallJumpable;
    private bool _jumpPressed;
    public bool _CanDoubleJump
    {
        get { return _canDoubleJump; }
        set
        {
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

    private bool _canJump = false;
    private float _isGroundedJumpTimeRemaining = 0;
    [SerializeField] private float _isGroundedJumpTime = 0.1f;


    private bool _canJumpAfterWall = false;
    private float _isWallJumpTimeRemaining = 0;
    [SerializeField] private float _isWallJumpTime = 0.1f;

    private bool _isJumping = false;
    private float _jumpTimeRemaining = 0;
    [SerializeField] private float _jumpTime = 0.2f;


    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxGroundSensor = GetComponent<BoxGroundSensor>();

        PlayerInput _playerInput = GetComponent<PlayerInput>();
        _onMoveAction = _playerInput.actions.FindAction("Move");
        _onJumpAction = _playerInput.actions.FindAction("Jump");
    }
    private void FixedUpdate()
    {
        ReadValues();
        UpdateCanJump();
        UpdateCanWallJump();
        UpdateJumpTime();

        UpdateHorizontalMovements();

        UpdateGravityScale();
        if (_rigidbody2D.velocity.y == 0 && _boxGroundSensor.IsGrounded())
        {
            if (_isWalled)
            {
                _isWalled = false;
            }
        }
    }

    private void UpdateCanJump()
    {
        if (_boxGroundSensor.IsGrounded())
        {
            _canJump = true;
            _isGroundedJumpTimeRemaining = _isGroundedJumpTime;
            return;
        }

        _isGroundedJumpTimeRemaining -= Time.fixedDeltaTime;
        _canJump = _isGroundedJumpTimeRemaining > 0;
    }

    private void UpdateJumpTime()
    {
        _jumpTimeRemaining -= Time.fixedDeltaTime;
        _isJumping = _jumpTimeRemaining > 0;
    }

    private void UpdateCanWallJump()
    {
        if (_isWalled)
        {
            _canJumpAfterWall = true;
            _isWallJumpTimeRemaining = _isWallJumpTime;
            return;
        }

        _isWallJumpTimeRemaining -= Time.fixedDeltaTime;
        _canJumpAfterWall = _isWallJumpTimeRemaining > 0;
    }

    private void UpdateGravityScale()
    {
        if (_isWalled) return;
        if (_rigidbody2D.velocity.y < 0)
        {
            _rigidbody2D.gravityScale = _gravityScaleNormal;
        }
        if (_rigidbody2D.velocity.y > 0 && !_jumpPressed)
        {
            _rigidbody2D.gravityScale = _gravityScaleRelease;
        }
    }

    private void UpdateHorizontalMovements()
    {
        if (_boxGroundSensor.IsGrounded())
        {
            _rigidbody2D.velocity = new Vector2(_moveAction.x * _movementSpeed, _rigidbody2D.velocity.y);
            return;
        }


        float airMoveAction = Mathf.MoveTowards(_rigidbody2D.velocity.x, _moveAction.x * _movementSpeed, _airControl * Time.fixedDeltaTime); 
        //float airMoveAction = Mathf.Lerp(_rigidbody2D.velocity.x, _moveAction.x * _movementSpeed, _airControl);
        _rigidbody2D.velocity = new Vector2(airMoveAction, _rigidbody2D.velocity.y);

    }
    private void ReadValues()
    {
        _jumpPressed = _onJumpAction.ReadValue<float>() > 0.5f ? true : false;
        _moveAction = _onMoveAction.ReadValue<Vector2>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        float xPos = collision.contacts[0].point.x;
        float yPos = collision.contacts[0].point.y;
        Vector2 direction = collision.contacts[0].normal;
        Quaternion rotation = Quaternion.Euler(direction.x, direction.y, 0);
        Instantiate(_particleSystem, new Vector3(xPos, yPos, 0), rotation);

        //Récupère le mur touché et sa normal
        _normalOfWall = collision.contacts[0].normal;
        if (_normalOfWall == Vector2.up) return;

        //Pose le joueur sur le mur et le fait glisser lentement
        if (collision.gameObject.CompareTag("Wall"))
        {
            _isWalled = true;
            _isWallJumpable = true;
        }
    }
    public void OnCollisionExit2D(Collision2D collision)
    {
        //Réactive les mouvements du joueur si il part du mur
        if (collision.gameObject.CompareTag("Wall"))
        {
            _isWalled = false;
        }
    }
    private void OnJump()
    {
        if(_isJumping)
            return;

        if (_canJump)
        {
            GroundJump();
            return;
        }

        if (_canJumpAfterWall)
        {
            WallJump();
            return;
        }


        if (_canDoubleJump)
        {
            DoubleJump();
            return;
        }

    }

    private void StartJump()
    {
        _isJumping = true;
        _jumpTimeRemaining = _jumpTime;
    }

    private void DoubleJump()
    {
        StartJump();
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
        _rigidbody2D.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        _rigidbody2D.gravityScale = _gravityScaleJump;
        _CanDoubleJump = false;
    }

    private void WallJump()
    {
        StartJump();
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
        _rigidbody2D.AddForce(new Vector2(Mathf.Sign(_normalOfWall.x) * 0.5f, 1f).normalized * _jumpPower, ForceMode2D.Impulse);
        _rigidbody2D.gravityScale = _gravityScaleJump;
        _isWalled = false;
    }

    private void GroundJump()
    {
        StartJump();
        _rigidbody2D.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        _rigidbody2D.gravityScale = _gravityScaleJump;
    }

    public void InstantiateParticle()
    {
        if (!_boxGroundSensor.IsGrounded()) return;
        Instantiate(_particleSystem, _particleSystemAnchor.position, _particleSystemAnchor.rotation);
    }
}
