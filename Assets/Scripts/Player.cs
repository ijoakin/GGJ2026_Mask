using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public Vector2 rawInputMovement;
    private PlayerState currentState;
    private readonly float m_interpolation = 10f;
    private Vector3 m_currentDirection = Vector3.zero;
    private float m_currentV = 0;
    private float m_currentH = 0;
    private float Speed = 1.2f;
    public float Framerate = 60f;

    [SerializeField]
    public GameObject target;

    [SerializeField]
    public GameObject dialogBox;

    [SerializeField]
    public ParticleSystem chargeParticleSystem;

    private Rigidbody _rigidbody;
    private CustomAnimatorController animatorController;

    private Vector3 finalDirectionToShoot = Vector3.zero;

    private bool OnGround;

    private float timeRemaining = 0.2f;

    private float timeRemainingDeadAnim = 5f;
    private CapsuleCollider capsuleCollider;
    private Vector3 previousDirection = Vector3.zero;

    CameraHandler cameraHandler;
    public Vector2 rawCameraInput;

    public bool IsActive = true;
    void Awake()
    {
        Instance = this;
    }

    public bool HoldRun = false;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        animatorController = GetComponent<CustomAnimatorController>();
        cameraHandler = CameraHandler.singleton;
        ExecuteState<IdlePlayerState>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        //AudioManager.Instance.PlayMainMusic();
    }

    private void FixedUpdate()
    {
        float delta = Time.deltaTime;
        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, rawCameraInput.x, rawCameraInput.y);
            // cameraHandler.HandleCameraCollision(delta);
        }
    }

    public void StartRun()
    {
        HoldRun = true;
    }

    public void StopRun()
    {
        HoldRun = false;
    }

    public void ExecuteState<T>() where T : PlayerState
    {
        var componentInObject = GetComponent<T>();
        if (componentInObject != null)
        {
            if (currentState != null)
            {
                currentState.OnExitState();
            }

            currentState = componentInObject as PlayerState;
            currentState.Rigidbody = this.GetComponent<Rigidbody>();

            if (currentState.PlayerAnimation != null)
            {

                animatorController.PlayAnimation(currentState.PlayerAnimation);
            }

            currentState.OnEnterState();
            currentState.name = typeof(T).Name;
        }
    }

    private void Update()
    {
        timeRemaining -= Time.deltaTime;

        if (currentState.name == "PlayerDyingState")
        {
            timeRemainingDeadAnim -= Time.deltaTime;

            if (timeRemainingDeadAnim < 0)
            {
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            if (!IsActive)
            {
                return;
            }
            DirectUpdate();
        }
    }

    public bool CheckIfIsLookAtACaharacter()
    {
        float distance = 10f;
        Vector3 hostPos = this.transform.position;
        Vector3 targetPos = target.transform.position;
        Ray ray = new Ray(hostPos, (targetPos - hostPos).normalized * 10);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance))
        {
            if (hit.collider.gameObject == target)
            {
                float angle = Vector3.Angle((targetPos - hostPos), this.transform.forward);
                if (angle < 25f && Vector3.Distance(targetPos, hostPos) < 2f)
                {
                    //Do something
                    return true;
                }
            }
        }
        return false;
    }

    private void DirectUpdate()
    {
        float v = rawInputMovement.y;
        float h = rawInputMovement.x;

        Transform camera = Camera.main.transform;

        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);
        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);

        Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

        float locomotionDelta = 0.01f;
        float myValue = animatorController.animator.GetFloat("Speed");

        if (rawInputMovement.y != 0 || rawInputMovement.x != 0)
        {
            float directionLength = direction.magnitude;
            direction.y = 0;
            direction = direction.normalized * directionLength;

            m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);
            transform.rotation = Quaternion.LookRotation(m_currentDirection);

            var calculatedSpeed = Speed * myValue * (HoldRun ? 2.3f : 1.5f);
            var finalDirection = new Vector3(m_currentDirection.x * calculatedSpeed * (Time.fixedDeltaTime * Framerate), _rigidbody.linearVelocity.y, m_currentDirection.z * calculatedSpeed * (Time.fixedDeltaTime * Framerate));

            _rigidbody.linearVelocity = finalDirection;
            previousDirection = finalDirection;

            if (HoldRun)
            {
                if (myValue <= 1)
                {
                    myValue += locomotionDelta;
                    animatorController.animator.SetFloat("Speed", myValue);
                }
                AudioManager.Instance.PlaySFX(AudioManager.AudioId.StepFast);
            }
            else
            {
                if (myValue <= 0.5)
                {
                    myValue += locomotionDelta;
                }
                else if (myValue > 0.5)
                {
                    myValue -= locomotionDelta;
                }
                animatorController.animator.SetFloat("Speed", myValue);
                AudioManager.Instance.PlaySFX(AudioManager.AudioId.StepSlow);
            }
        }
        else
        {
            if (myValue > 0)
            {
                if (previousDirection != Vector3.zero)
                {
                    _rigidbody.linearVelocity = previousDirection * 0.2f;
                }

                myValue -= locomotionDelta;
                animatorController.animator.SetFloat("Speed", myValue);
            }
        }
    }
}
