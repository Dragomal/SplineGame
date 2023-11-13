using UnityEngine;

public abstract class GroundSensor : MonoBehaviour
{
    public abstract bool IsGrounded();
    public abstract float GetGroundDistance();
    public abstract void UpdateGroundDetection();
    private void FixedUpdate(){
        UpdateGroundDetection();
    }
}
