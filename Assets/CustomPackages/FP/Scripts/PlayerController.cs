using UnityEngine;

// [RequireComponent(typeof(Animator))]
// [RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float lookSensitivity = 3f;

	[SerializeField]
	private float thrusterForce = 1000f;

	private PlayerMotor motor;


	void Start ()
	{
		motor = GetComponent<PlayerMotor>();
	}

	void Update ()
	{
		float _xMov = Input.GetAxis("Horizontal");
		float _zMov = Input.GetAxis("Vertical");

		Vector3 _movHorizontal = transform.right * _xMov;
		Vector3 _movVertical = transform.forward * _zMov;

		Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

		motor.Move(_velocity);

		float _yRot = Input.GetAxisRaw("Mouse X");

		Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

		motor.Rotate(_rotation);

		float _xRot = Input.GetAxisRaw("Mouse Y");

		float _cameraRotationX = _xRot * lookSensitivity;

		motor.RotateCamera(_cameraRotationX);

		Vector3 _thrusterForce = Vector3.zero;
		if (Input.GetButton ("Jump"))
		{
			_thrusterForce = Vector3.up * thrusterForce;
		}

		motor.ApplyThruster(_thrusterForce);

	}


}
