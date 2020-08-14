using UnityEngine;

namespace Environment
{
    public class EnvironmentManager : MonoBehaviour
    {
        public GameObject startingEnvironmentTile;
        private const int StartingTileLifeSpan = 6;
        public GameObject environmentPrefab;
        private GameObject _activeEnvironmentTile;
        private static readonly Vector3 StartPosition = new Vector3(0,0,0);

        private void Start()
        {
            CreateNewEnvironmentTileWithObjectPooling(StartPosition);
            DestroyStartingTile(StartingTileLifeSpan);
        }

        private void DestroyStartingTile(int time)
        {
            StartCoroutine(EndlessRunnerGameManager.Wait(time, () =>
            {
                Destroy(startingEnvironmentTile);
            }));
        }
        
        //There are many ways to do this and this will is will be a big performance influencer so I have demonstrated 2 ways a basic prototyping way and a more optimised, object pooling way which would generate less garbage than instantiating and destroying.
        public void CreateNewEnvironmentTile()
        {
            //CreateNewEnvironmentTile(_activeEnvironmentTile.transform.GetChild(0).transform.position);
            CreateNewEnvironmentTileWithObjectPooling(_activeEnvironmentTile.transform.GetChild(0).transform.position);
        }

        //Unused method. Demonstration only. Using this method would require a different way of removing past environments and generates a lot of garbage. Object pooling is optimal.
        private void CreateNewEnvironmentTile(Vector3 position)
        {
            _activeEnvironmentTile = Instantiate(environmentPrefab, position, Quaternion.identity, gameObject.transform);
            _activeEnvironmentTile.GetComponent<EnvironmentTileContentManager>().CreatePickupsAndEnemies();
            #if UNITY_EDITOR
            EndlessRunnerGameManager.DisplayDebugMessage("New environment tile instantiated.");
            #endif
        }
        
        private void CreateNewEnvironmentTileWithObjectPooling(Vector3 position)
        {
            _activeEnvironmentTile = ObjectPooling.ReturnObjectFromPool(0, position, Quaternion.identity);
            _activeEnvironmentTile.GetComponent<EnvironmentTileContentManager>().CreatePickupsAndEnemies();
            #if UNITY_EDITOR
            EndlessRunnerGameManager.DisplayDebugMessage("New environment tile enabled from object pool.");
            #endif
        }

        public void ResetWorldPositionToZero()
        {
            ParentEnvironmentTileToPlayer(true);
            ResetPlayerPosition();
            ParentEnvironmentTileToPlayer(false);
            #if UNITY_EDITOR
            EndlessRunnerGameManager.DisplayDebugMessage("World position has been reset to 0.");
            #endif
        }

        private static void ResetPlayerPosition()
        {
            EndlessRunnerGameManager.instance.player.transform.position = StartPosition;
        }
        
        private void ParentEnvironmentTileToPlayer(bool state)
        {
            if (state)
            {
                _activeEnvironmentTile.transform.parent = EndlessRunnerGameManager.instance.player.transform;
                return;
            }
            _activeEnvironmentTile.transform.parent = null;
        }
        
    }
}
