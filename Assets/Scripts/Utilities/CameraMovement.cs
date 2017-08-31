using UnityEngine;

/*
** Class which controls the camera movement.
*/
public class CameraMovement : MonoBehaviour {
    
    #region FIELDS
    public float speed = 2.0f;              // Camera movement speed.
    private float _rotate_speed = 0f;       // Camera rotate speed.           
    private float _acceleration = 0f;       // Camera acceleration.
	private float _friction = 0.95f;        // Camera friction.
    private float _minFov = 15f;            // Camera min focal view. 
    private float _maxFov = 90f;            // Camera max focal view.
    private float _sensitivity = 10f;       // Camera sensitivity when scrolling.
    #endregion
    
    #region UNITY_METHODS
    /// <summary>
    /// Update is called once per frame.
    /// </summary>
	void Update () 
    {
        // Pan the camera depending on the arrows pressed.
        if (Input.GetKey(KeyCode.D)){
			transform.position += Vector3.right * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.A)){
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.W)){
			transform.position += Vector3.forward * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.S)){
			transform.position += Vector3.back * speed * Time.deltaTime;
		}
        // Rotate the camera.
        if(Input.GetKey(KeyCode.Space)){
			_acceleration += 0.01f;
			_rotate_speed = _rotate_speed + _acceleration;

			if(_rotate_speed > 3f){
				_rotate_speed = 3f;
			}
		}
        // Rotate the camera around
		transform.RotateAround(new Vector3(3f,9f,3f), Vector3.up, _rotate_speed);
        // Set the acceleration and rotate speed depending on the friction.
		_acceleration *= _friction;
		_rotate_speed *= _friction;
        
        // Set the focal view.
        float fov = Camera.main.fieldOfView;
        // Get the mouse wheel
        fov += Input.GetAxis("Mouse ScrollWheel") * _sensitivity;
        // Change the focal point according to min and max values.
        fov = Mathf.Clamp(fov, _minFov, _maxFov);
        // Change the fieldOfView of the camera.
        Camera.main.fieldOfView = fov;
	}
    #endregion
}
