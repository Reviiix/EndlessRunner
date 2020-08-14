using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Environment
{
    public class EnvironmentTileContentManager : MonoBehaviour
    {
        public Transform[] potentialSpawnPoints;
        private readonly List<int> _activeSpawnPointIndices = new List<int>();
        private const int MaximumPercent = 100; 
        [Range(0, MaximumPercent)]
        private const int EnemyPercent = 90; //remaining percentage is for pickups.
        private readonly List<GameObject> _enemyAndPickupObjects = new List<GameObject>();
        
        public void CreatePickupsAndEnemies()
        {
            ResetEnemiesAndPickups();
            for (var i = 0; i < potentialSpawnPoints.Length; i++)
            {
                if (Random.Range(-50, PlayerManager.speed) < 4) continue; // This makes for incrementing difficulty, more spawns over time (player speed is constantly increasing to a max of 20).
                if (SpaceAlreadyOccupied(i, _activeSpawnPointIndices)) continue;
                
                CreatePickupOrEnemyAt(potentialSpawnPoints[i].position, i);
            }
        }

        private void CreatePickupOrEnemyAt(Vector3 position, int index)
        {
            var newGameObject = Random.Range(0, MaximumPercent) > EnemyPercent ? CreatePickupAt(position) : CreateEnemyAt(position);
            newGameObject.transform.parent = gameObject.transform;
            
            _activeSpawnPointIndices.Add(index);
            _enemyAndPickupObjects.Add(newGameObject);
        }

        private static GameObject CreatePickupAt(Vector3 position)
        {
            return ObjectPooling.ReturnObjectFromPool(1, position, Quaternion.identity);
        }
        
        private static GameObject CreateEnemyAt(Vector3 position)
        {
            return ObjectPooling.ReturnObjectFromPool(2, position, Quaternion.identity);
        }

        private static bool SpaceAlreadyOccupied(int indexToSearchFor, IEnumerable<int> array)
        {
            return array.Any(value => indexToSearchFor == value);
        }

        private void ResetEnemiesAndPickups()
        {
            ResetSpawnIndices();
            ResetAllObjects();
        }

        private void ResetSpawnIndices()
        {
            _activeSpawnPointIndices.Clear();
        }
        
        private void ResetAllObjects()
        {
            foreach (var v in _enemyAndPickupObjects)
            {
                if (v.GetComponent<Pickup>() != null)
                {
                    v.GetComponent<Pickup>().EnableParticle(false);
                }
                v.SetActive(false);
            }
            _enemyAndPickupObjects.Clear();
        }
    }
}
