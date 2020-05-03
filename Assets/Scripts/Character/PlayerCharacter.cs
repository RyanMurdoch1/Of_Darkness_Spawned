using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Handles player movement and state
/// </summary>
public class PlayerCharacter : MonoBehaviour
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
    [SerializeField] private LayerMask whatIsGround;
    [BoxGroup("hideCheckVariables/Check Variables")]
    public bool canClimb;

    [SerializeField] private bool hideBowObjects = true;
    [HideIfGroup("hideBowObjects")] [BoxGroup("hideBowObjects/Bow Objects")] [SerializeField]
    private GameObject backBowArm;
    [BoxGroup("hideBowObjects/Bow Objects")] [SerializeField]
    private GameObject frontBowArm;
    [BoxGroup("hideBowObjects/Bow Objects")] [SerializeField]
    private ProjectileLauncher launcher;
    #endregion
    
    public Animator animator;
    public CharacterMotor characterMotor; 
    public StateMachine characterStateMachine;
    
    public StandingState standingState;
    public JumpingState jumpingState;
    public ClimbingState climbingState;
    public DamagedState damagedState;
    public BowState shootingState;
    
    private Rigidbody2D _playerRigidbody2D;
    private CollisionChecker _collisionChecker;

    private void OnEnable() => CharacterHealth.DamagedFromDirection += TakeDamage;

    private void OnDisable() => CharacterHealth.DamagedFromDirection -= TakeDamage;
    
    private void Start()
    {
        characterStateMachine = new StateMachine();
        _collisionChecker = new CollisionChecker(groundCheck, whatIsGround, this);
        _playerRigidbody2D = GetComponent<Rigidbody2D>();
        characterMotor = new CharacterMotor(this, _playerRigidbody2D, jumpForce, movementSmoothing);
        standingState = new StandingState(walkSpeed, _collisionChecker,this);
        jumpingState = new JumpingState(airSpeed, _collisionChecker, this);
        climbingState = new ClimbingState(climbSpeed, this, _collisionChecker);
        damagedState = new DamagedState(this);
        shootingState = new BowState(this, frontBowArm, backBowArm, launcher);
        
        characterStateMachine.Initialize(standingState);
    }

    private void TakeDamage(Vector2 damageDirection)
    {
        damagedState.damageDirection = damageDirection;
        characterStateMachine.ChangeState(damagedState);
    }

    private void Update()
    {
        characterStateMachine.currentState.HandleInput();
        characterStateMachine.currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        characterStateMachine.currentState.PhysicsUpdate();
    }
}
