using UnityEngine;

public class Enemy : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //There is no need to check the collider since player is the only rigid body in the scene.
        RegisterCollision();
    }

    private void RegisterCollision()
    {
        EndlessRunnerGameManager.instance.EnemyCollision();
        StartCoroutine(EndlessRunnerGameManager.FlashObject(_meshRenderer));
    }
    
}
