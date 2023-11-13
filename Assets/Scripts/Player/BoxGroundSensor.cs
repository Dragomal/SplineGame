using UnityEngine;

public class BoxGroundSensor : GroundSensor
{
    [SerializeField] private LayerMask _groundLayerMask = 0;
    private RaycastHit2D _hit;
    public override bool IsGrounded(){
        if(_hit.collider != null){
            return true;
        }
        else{
            return false;
        }
    }
    public override float GetGroundDistance(){
        if(_hit.collider != null){
            return _hit.distance;
        }
        else{
            return Mathf.Infinity;
        }
    }
    public override void UpdateGroundDetection(){
        _hit = Physics2D.BoxCast(transform.position, 
        new Vector2(0.5f, 0.5f), 
        0,
        Vector2.down, 
        1.3f, 
        _groundLayerMask);
    }
}
