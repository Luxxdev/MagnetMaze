using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [Range(1,10)]
    [SerializeField] private float smoothFactor;
    [SerializeField] private Vector3 minValues, maxValue;

    void FixedUpdate()
    {
        Follow();    
    }

    void Follow()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minValues.x, maxValue.x),
            Mathf.Clamp(targetPosition.y, minValues.y, maxValue.y),
            Mathf.Clamp(targetPosition.z, minValues.z, maxValue.z));
        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor*Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }
    // [SerializeField] private float speed =  10f;
    // private float currentPosX;
    // private Vector3 velocity = Vector3.zero;

    // void Update()
    // {
    //     transform.position = Vector3.SmoothDamp(transform.position, 
    //     new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed * Time.deltaTime);
    // }

    // public void MoveToNewRoom(float _newRoom)
    // {
    //     currentPosX = _newRoom;
    // }
}
