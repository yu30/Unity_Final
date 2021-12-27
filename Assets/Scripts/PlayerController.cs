using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 30f;
    public float horizontalSensitivity = 150;
    public float verticalSensitivity = 150;
    public Transform cameraPivot;
    
    private Transform _transform;
    private Rigidbody _rb;
    
    // Start is called before the first frame update
    private void Start()
    {
        _transform = gameObject.transform;
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        PlayerMovement();
        CameraMovement();
    }
    
    private void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        _transform.Translate(vertical * speed * Time.deltaTime * Vector3.forward);
        _transform.Translate(horizontal * speed * Time.deltaTime * Vector3.right);

        // Reset angular rotation
        _rb.angularVelocity = new Vector3(0, 0, 0);
        // Reset x, z rotation
        // TODO : how to stop player from tilting??
        //_transform.rotation = Quaternion.Euler(0, _transform.rotation.y, 0);
    }
    
    private void CameraMovement()
    {
        float rotateHorizontal = Input.GetAxis("Mouse X");
        float rotateVertical   = Input.GetAxis("Mouse Y");
        
        // Player can rotate horizontally, but not vertically
        _transform.RotateAround (_transform.position, Vector3.up, rotateHorizontal * horizontalSensitivity * Time.deltaTime);
        // Only camera can rotate vertically 
        float rotation = cameraPivot.localEulerAngles.x;
        // Restrict the rotation in between (45, -45) 
        if ( (rotation < 45 || rotation > 315) ||
             (rotation > 45 && rotation < 180 && rotateVertical < 0) ||
             (rotation <315 && rotation > 180 && rotateVertical > 0) )
            cameraPivot.Rotate(Vector3.right, rotateVertical * verticalSensitivity * Time.deltaTime, Space.Self);
    }
}
