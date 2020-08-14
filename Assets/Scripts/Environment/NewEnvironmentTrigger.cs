using UnityEngine;

namespace Environment
{
    public class NewEnvironmentTrigger : MonoBehaviour
    {
        private static int _amountOfTimesPlayerHasPassedThrough = 0;
            
        //The brief said to reset the player position every 1000 units. I assume units to mean distance travelled.
        //Rather than checking the player positions I just count the environment tile passthroughs and check that.
        //23 == 1000 so im using 20 to be safe and its easier to read / work with round numbers.
        private void OnTriggerEnter(Collider other) 
        {
            //There is no need to check the collider since player is the only rigid body in the scene.
            _amountOfTimesPlayerHasPassedThrough++;
            if (_amountOfTimesPlayerHasPassedThrough >= 20)
            {
                _amountOfTimesPlayerHasPassedThrough = 0;
                EndlessRunnerGameManager.instance.environmentManager.ResetWorldPositionToZero();
            }
            EndlessRunnerGameManager.instance.environmentManager.CreateNewEnvironmentTile();
        }
    }
}
