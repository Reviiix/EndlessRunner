using UnityEngine;

public class PlayerManager : SwipeController
{
    public GameObject playerObject;
    private MeshRenderer _meshRenderer;
    public GameObject shieldObject;
    private const int StartingLaneIndex = 2;
    private int _activeLaneIndex;
    private const int MaxSpeed = 30;
    private const int StartSpeed = 7;
    public static float speed;
    public static bool shieldActive;
    public static bool canBeDamaged;

    private void Awake()
    {
        InitialiseVariables();
    }
    
    private void Start()
    {
        SetFirstLane();
    }

    protected override void Update()
    {
        base.Update(); //Where swiping happens.
        IncrementSpeed();
    }
    
    private void FixedUpdate()
    {
        MovePlayerForward();
    }

    private void InitialiseVariables()
    {
        speed = StartSpeed;
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        shieldActive = false;
        canBeDamaged = true;
    }

    private void SetFirstLane()
    {
        ChangeLane(EndlessRunnerGameManager.instance.laneManager.lanes[StartingLaneIndex]);
    }

    private void MovePlayerForward()
    {
        transform.Translate (Vector3.forward * (speed * Time.deltaTime));
    }

    private static void IncrementSpeed()
    {
        if (speed >= MaxSpeed) return;

        speed += (speed / 1000);
    }

    public void MoveLeft()
    {
        ChangeLane(EndlessRunnerGameManager.instance.laneManager.ReturnLaneToTheLeft(_activeLaneIndex));
    }
    
    public void MoveRight()
    {
        ChangeLane(EndlessRunnerGameManager.instance.laneManager.ReturnLaneToTheRight(_activeLaneIndex));
    }

    private void ChangeLane(Lane newLane)
    {
        _activeLaneIndex = newLane.laneIndex;
        var position = transform.localPosition;
        PhysicallyChangeLane(new Vector3(newLane.laneXPosition, position.y, position.y));
    }

    private void PhysicallyChangeLane(Vector3 newLanePosition)
    {
        playerObject.transform.localPosition = newLanePosition;
        EndlessRunnerGameManager.instance.audioManager.PlayLaneChangeNoise();
    }

    public void TakeHit()
    {
        if (!shieldActive) return;
        shieldActive = false;
        ActivateShield(false);
        canBeDamaged = false;
        StartCoroutine(EndlessRunnerGameManager.FlashObject(_meshRenderer, () =>
        {
            canBeDamaged = true; //Intentionally take no damage when flashing. Seems fair.
        }));
        
        #if UNITY_EDITOR
        EndlessRunnerGameManager.DisplayDebugMessage("Player received damage, Shields gone!");
        #endif
    }
    
    public void ActivateShield(bool state)
    {
        shieldActive = state;
        shieldObject.SetActive(state);
    }
}
