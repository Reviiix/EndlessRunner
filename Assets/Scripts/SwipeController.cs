using UnityEngine;

public class SwipeController : MonoBehaviour
{
    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private bool _isSwiping;

    protected virtual void Update()
    {
        Inputs();
        CalculateSwipeDistance();
        CheckInputs();
    }

    private void Inputs()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isSwiping = true;
            _startPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isSwiping = false;
            Reset();
        }


        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                _isSwiping = true;
                _startPosition = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                _isSwiping = false;
                Reset();
            }
        }
    }

    private void CalculateSwipeDistance()
    {
        _endPosition = Vector2.zero;
        
        if (!_isSwiping) return;
        
        if (Input.touches.Length > 0)
        {
            _endPosition = Input.touches[0].position - _startPosition;
        }
        else if (Input.GetMouseButton(0))
        {
            _endPosition = (Vector2) Input.mousePosition - _startPosition;
        }
    }

    private void CheckInputs()
    {
        if (_endPosition.magnitude > 50)
        {
            var x = _endPosition.x;
            if (Mathf.Abs(x) > Mathf.Abs(_endPosition.y))
            {
                if (x < 0)
                {
                    EndlessRunnerGameManager.instance.player.MoveLeft();
                    #if UNITY_EDITOR
                    EndlessRunnerGameManager.DisplayDebugMessage("Swiped left");
                    #endif
                }
                else
                {
                    EndlessRunnerGameManager.instance.player.MoveRight();
                    #if UNITY_EDITOR
                    EndlessRunnerGameManager.DisplayDebugMessage("Swiped Right");
                    #endif
                }
            }
            Reset();
        }
    }

    private void Reset()
    {
        _startPosition = new Vector2(0,0);
        _startPosition = new Vector2(0,0);
        _isSwiping = false;
    }
}
