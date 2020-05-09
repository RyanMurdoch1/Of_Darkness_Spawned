using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Handles player movement and state
/// </summary>
public class PlayerCharacter : MonoBehaviour
{
    #region Variables
     private const float WalkSpeed = 3;
     private const float AirSpeed = 2;
     private const float ClimbSpeed = 3;
     private const float JumpForce = 350;
     private const float MovementSmoothing = 0.05f;

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

    [SerializeField] private GameObject weaponZone;
    #endregion

    public PlayerControls playerControls;
    public Animator animator;
    public CharacterMotor characterMotor; 
    public StateMachine characterStateMachine;
    
    public StandingState standingState;
    public JumpingState jumpingState;
    public ClimbingState climbingState;
    public DamagedState damagedState;
    public BowState shootingState;
    public AttackState attackState;
    public RollState rollState;
    public DirectionalMovementTracker movementTracker;
    
    private Rigidbody2D _playerRigidbody2D;
    private CollisionChecker _collisionChecker;

    private void OnEnable()
    {
        CharacterHealth.DamagedFromDirection += TakeDamage;
        PlayerInteraction.EnteredAreaClimbing += AbleToClimb;
    }

    private void OnDisable()
    {
        CharacterHealth.DamagedFromDirection -= TakeDamage;
        PlayerInteraction.EnteredAreaClimbing -= AbleToClimb;
    }

    private void Start()
    {
        playerControls = new PlayerControls();
        PlayerSetup();
        SetUpPlayerStates();
        EnableControls();
        characterStateMachine.Initialize(standingState);
    }

    private void PlayerSetup()
    {
        characterStateMachine = new StateMachine();
        movementTracker = new DirectionalMovementTracker(playerControls);
        _collisionChecker = new CollisionChecker(groundCheck, whatIsGround, this);
        _playerRigidbody2D = GetComponent<Rigidbody2D>(); 
    }

    private void SetUpPlayerStates()
    {
        characterMotor = new CharacterMotor(this, _playerRigidbody2D, JumpForce, MovementSmoothing);
        standingState = new StandingState(WalkSpeed, _collisionChecker,this);
        jumpingState = new JumpingState(AirSpeed, _collisionChecker, this);
        climbingState = new ClimbingState(ClimbSpeed, this, _collisionChecker);
        damagedState = new DamagedState(this);
        shootingState = new BowState(this, frontBowArm, backBowArm, launcher);
        attackState = new AttackState(this, characterMotor, weaponZone);
        rollState = new RollState(this, characterMotor);
    }

    private void EnableControls()
    {
        playerControls.Player.Attack.Enable();
        playerControls.Player.Roll.Enable();
        playerControls.Player.Jump.Enable();
        playerControls.Player.ChangeWeapon.Enable();
    }
    
    private void AbleToClimb(bool able)
    {
        canClimb = able;
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

    private void FixedUpdate() => characterStateMachine.currentState.PhysicsUpdate();
}
