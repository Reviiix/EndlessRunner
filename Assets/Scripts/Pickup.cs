using UnityEngine;

public class Pickup : MonoBehaviour
{
    private const int ScoreValue = 1000;
    private static float _waitTime = 3;
    public GameObject particles;
    public GameObject visibleObject;

    private void Awake()
    {
        InitialiseVariables();
    }

    private void InitialiseVariables()
    {
        _waitTime = particles.GetComponentInChildren<ParticleSystem>().main.duration;
    }

    private void OnTriggerEnter(Collider other)
    {
        //There is no need to check the collider since player is the only rigid body in the scene.
        AwardPickup();
    }

    private void AwardPickup()
    {
        EndlessRunnerGameManager.instance.PickupCollision(ScoreValue);
        EnableParticle(true);
        StartCoroutine(EndlessRunnerGameManager.Wait(_waitTime, () =>
        {
            EnableParticle(false);
            gameObject.SetActive(false);
        }));
    }

    public void EnableParticle(bool state)
    {
        particles.SetActive(state);
        visibleObject.SetActive(!state);
    }
}
