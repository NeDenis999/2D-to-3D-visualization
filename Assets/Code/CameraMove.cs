using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] 
    private float speed;
    
    void Update()
    {
        var position = transform.position;
        position.z += speed * Time.deltaTime;
        transform.position = position;
    }
}
