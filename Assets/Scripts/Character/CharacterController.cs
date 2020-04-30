using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Handles player movement and state
/// </summary>
public class CharacterController : MonoBehaviour
{
    #region Variables
    [SerializeField] private bool hideSpeedVariables = true;
    [HideIfGroup("hideSpeedVariables")]
    [BoxGroup("hideSpeedVariables/Speed Variables")]
    [SerializeField] private float walkSpeed = 15;
    [BoxGroup("hideSpeedVariables/Speed Variables")]
    [SerializeField] private float airSpeed = 10;
    [BoxGroup("hideSpeedVariables/Speed Variables")]
    [SerializeField] private float climbSpeed = 1;
    [BoxGroup("hideSpeedVariables/Speed Variables")]
    [SerializeField] private float jumpForce = 600;
    [BoxGroup("hideSpeedVariables/Speed Variables")]
    [SerializeField] private float movementSmoothing = 0.05f;

    [SerializeField] private bool hideCheckVariables = true;
    [HideIfGroup("hideCheckVariables")]
    [BoxGroup("hideCheckVariables/Check Variables")]
    [SerializeField] private Transform groundCheck;
    [BoxGroup("hideCheckVariables/Check Variables")]
    [SerializeField] private Transform ceilingCheck;
    [BoxGroup("hideCheckVariables/Check Variables")]
    [SerializeField] private LayerMask whatIsGround;
    [BoxGroup("hideCheckVariables/Check Variables")]
    public bool canClimb;

    [SerializeField] private bool hideBowObjects = true;
    [HideIfGroup("hideBowObjects")] [BoxGroup("hideBowObjects/Bow Objects")] [SerializeField]
    private GameObject backBowArm;
    [BoxGroup("hideBowObjects/Bow Objects")] [SerializeField]
    private GameObject frontBowArm;
    #endregion
    
    public Animator animator;
    private Rigidbody2D _playerRigidbody2D;
    private StateMachine _characterStateMachine;
    private CollisionChecker _collisionChecker;
    private CharacterMotor _characterMotor;

    public StandingState standingState;
    public JumpingState jumpingState;
    public ClimbingState climbingState;
    public DamagedState damagedState;
    public ShootingState shootingState;

    private void OnEnable()
    {
        CharacterHealth.DamagedFromDirection += TakeDamage;
    }

    private void OnDisable()
    {
        CharacterHealth.DamagedFromDirection -= TakeDamage;
    }

    private void Start()
    {
        _characterStateMachine = new StateMachine();
        _collisionChecker = new CollisionChecker(groundCheck, ceilingCheck, whatIsGround, this);
        _playerRigidbody2D = GetComponent<Rigidbody2D>();
        _characterMotor = new CharacterMotor(this, _playerRigidbody2D, jumpForce, movementSmoothing);
        standingState = new StandingState(_characterStateMachine, walkSpeed, _collisionChecker, _characterMotor, this);
        jumpingState = new JumpingState(_characterStateMachine, airSpeed, _collisionChecker, _characterMotor, this);
        climbingState = new ClimbingState(_characterStateMachine, _characterMotor, climbSpeed, this);
        damagedState = new DamagedState(_characterStateMachine, this, _characterMotor);
        shootingState = new ShootingState(_characterStateMachine, this, _characterMotor, frontBowArm, backBowArm);
        
        _characterStateMachine.Initialize(standingState);
    }

    private void TakeDamage(Vector2 damageDirection)
    {
        damagedState.damageDirection = damageDirection;
        _characterStateMachine.ChangeState(damagedState);
    }

    private void Update()
    {
        _characterStateMachine.currentState.HandleInput();
        _characterStateMachine.currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        _characterStateMachine.currentState.PhysicsUpdate();
    }
}
