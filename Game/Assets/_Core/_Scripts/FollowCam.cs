using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowCam : MonoBehaviour 
{
	//Vector3 velocity = Vector3.zero;
	//float dampTime = 0.3f; //offset from the viewport center to fix damping
	
	public Transform[] targets = new Transform[2];
	public float offset = 0.0f;
	
	public float interpSpeed = 1.0f;
	
	public Vector3 positionOffset = new Vector3(0,0,0);
	
	public float speedSensitivity = 1.0f;
	
	public GameObject PlayerPrefab;
	
	private float _moveTime = 5.0f;
	
	//private Vector3 velocity = Vector3.zero;
	private Vector3 scaledOffset;
	
	//Points of Influence code
	private float minZoom;
	private float maxZoom;
	
	//bool fixedFrame = false;
	
	private Transform _transform;
	
	PID horizontalMoveController = null;
	PID verticalMoveController = null;
	PID zoomMoveController = null;
	
	//PlaneOfPlay _playPlane = null;
	
	public enum CAMERA_STATE {
		NORMAL,
		INTERIOR
	}
	
	public CAMERA_STATE state;
	
	void Start()
	{
		state = CAMERA_STATE.NORMAL;
		
		//320
		_transform = transform;
		minZoom = _transform.localPosition.y;
		maxZoom = _transform.localPosition.y + 150.0f;
		
		if (targets != null && targets.Length > 0) {
			_transform.position = targets[0].transform.position;	
			Vector3 local = transform.localPosition; local.y = minZoom; transform.localPosition = local;
			
			horizontalMoveController = gameObject.AddComponent<PID>();
			verticalMoveController = gameObject.AddComponent<PID>();
			zoomMoveController = gameObject.AddComponent<PID>();
			
			horizontalMoveController.Kp = 0.1f;
			horizontalMoveController.Ki = 0.0f;
			horizontalMoveController.Kd = 0.0f;
			
			verticalMoveController.Kp = 0.1f;
			verticalMoveController.Ki = 0.0f;
			verticalMoveController.Kd = 0.0f;
			
			zoomMoveController.Kp = 0.1f;
			zoomMoveController.Ki = 0.0f;
			zoomMoveController.Kd = 0.0f;
		}
		
		//_playPlane = GameObject.Find(Constants.PLANE_OF_PLAY).GetComponent<PlaneOfPlay>();
	}
	
	void Awake()
	{}
	
	public IEnumerator InteriorView(Transform target) {
		yield return StartCoroutine(MoveToTarget(target, 10.0f));
		//yield return StartCoroutine(ZoomFOV(10.0f, 1.0f));
	}
	
	IEnumerator MoveToTarget(Transform target, float endfov) {
		Vector3 toPosition = target.position;toPosition.y = transform.position.y;
		Vector3 fromPosition = transform.position;
		float currentFOV = camera.fov;
		
		//float speed = 10.0f;
		float elapsed = 0.0f;
		float time = 1.0f;
		while (elapsed < time) {
			transform.position 	= Vector3.Lerp (fromPosition, toPosition, elapsed / time);
			camera.fov 			= Mathf.Lerp(currentFOV, endfov, elapsed/time);
			//elapsed += SceneManager.AnimationDeltaTime();
			yield return null;
		}
	}
	
	void updateAlphaComplete() {
		
	}
	
	void FUpdate()
	{
		//if (SceneManager._gameState == SceneManager.GAME_STATE.IN_GAME_PAUSE) {
		//	Vector3 point = camera.WorldToViewportPoint(targets[0].position);
		//    Vector3 delta = targets[0].position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
		//    Vector3 destination = transform.position + delta;
				
		//	transform.position = Vector3.SmoothDamp(transform.position, destination + positionOffset, ref velocity, 0.5f, 10000000.0f, SceneManager.AnimationDeltaTime());
		//}
		//if (state == CAMERA_STATE.INTERIOR) {
			//Move to the 	
		//}
	}
	
	void Update() {
		if (Input.GetKey(KeyCode.DownArrow)) {
			//maxZoom -= 1.0f;
			minZoom -= 10.0f;
			maxZoom = minZoom + 150.0f;
		}
		else if (Input.GetKey(KeyCode.UpArrow)) {
			minZoom += 10.0f;
			maxZoom = minZoom + 150.0f;
		}
		//gameObject.camera.farClipPlane = _transform.position.y;
	}
	
	//void FixedUpdate()
	//{
	//	fixedFrame = true;	
	//}
	
	void FixedUpdate() {
		if(targets != null && targets.Length > 0)
		{
	    	if(targets[0]) 
	    	{
				float speedScalar = 1.0f;
				
				GameObject veh = targets[0].gameObject;
				Vector3 targetPos = targets[0].transform.position; targetPos.y = _transform.position.y;
				
				//Offset due to velocity
				Vector3 velocityOffset = targets[0].rigidbody.velocity; velocityOffset.y = 0.0f;
				scaledOffset = Vector3.Scale(velocityOffset, new Vector3(0.2f, 0.2f, 0.2f));
					
				//Offset due to mouse position
				Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); mousePos.y = targets[0].transform.position.y;
		        Vector3 delta = targetPos - mousePos;
				
				//Final Calculation
		        Vector3 destination = transform.position + delta + scaledOffset;
				
				if (veh != null && horizontalMoveController != null && verticalMoveController != null) {
					//speedScalar = speedSensitivity * pc.rigidbody.velocity.magnitude;
					//speedScalar = Mathf.Clamp(speedScalar, 1.0f, 2.0f);
					
					//ShipMotor m = veh.Motor;
					float max = 30.0f;
					float currentVelocity = Mathf.Abs (rigidbody.velocity.magnitude);
					float ratio = (currentVelocity > Mathf.Epsilon ? currentVelocity / max : 0.0f);
					
					//transform.position = Vector3.SmoothDamp(transform.position, destination + positionOffset, ref velocity, 0.5f, 10000000.0f, Time.deltaTime * interpSpeed * speedScalar);
					//(x^2) = z^2
					float xError = _transform.position.x - destination.x;
					float zError = _transform.position.z - destination.z;
					
					float dt = Time.fixedDeltaTime;
					
					float xFix = horizontalMoveController.GetOutput(xError, dt);
					float zFix = verticalMoveController.GetOutput(zError, dt);
					
					_transform.position = new Vector3(_transform.position.x - (xFix * Time.fixedDeltaTime * 50.0f), _transform.position.y, _transform.position.z - (zFix * Time.fixedDeltaTime * 50.0f));
					
					Vector3 local = _transform.localPosition; 
					float target = Mathf.Max(Mathf.Min(((maxZoom - minZoom) * ratio) + minZoom, maxZoom), minZoom);
					float error = local.y - target;
					float zoomFix = zoomMoveController.GetOutput(error, dt);
					local.y -= zoomFix;
					_transform.localPosition = local;
				}
				else {
					//NPCVehicle veh = targets[0].GetComponent<NPCVehicle>();
					
					/*speedScalar = speedSensitivity * veh.Speedometer.Speed;
					speedScalar = Mathf.Clamp(speedScalar, 1.0f, 2.0f);
					
					ShipMotor m = veh.Motor;
					float max = ShipMotor.NORMAL_SPEED + ShipMotor.SPEED_BOOST;
					float currentVelocity = m._currentVelocity;
					float ratio = (currentVelocity > Mathf.Epsilon ? currentVelocity / max : 0.0f);
					
					//transform.position = Vector3.SmoothDamp(transform.position, destination + positionOffset, ref velocity, 0.5f, 10000000.0f, Time.deltaTime * interpSpeed * speedScalar);
					transform.position = destination;
					
					Vector3 local = transform.localPosition; 
					local.y = Mathf.Max(Mathf.Min(((maxZoom - minZoom) * ratio) + minZoom, maxZoom), minZoom);
					transform.localPosition = local;*/
				}
	    	}
		}
		
		//fixedFrame = false;
   }
	
	public Vector3 calculatePOIFocus() {
		Transform player = targets[0];
		//foreach (MapPOI p in influences) {
		//	float normalized = Utilities.PointInRadius(player.transform.position, p.transform.position, p.Radius());
		//	if (normalized >= 0.0f) {
				
		//	}
		//}
		
		return Vector3.zero;	
	}
	
	public IEnumerator MoveToTarget() {
		if (targets[0]) {
			float elapsed = 0.0f;
			
			while (elapsed < _moveTime) {
				Vector3 forward = targets[0].transform.forward;
				Vector3 forwardOffset = forward * offset;// new Vector3 (offset, 0.0f, offset);
				transform.position = new Vector3(targets[0].transform.position.x + forwardOffset.x, transform.position.y, targets[0].transform.position.z + forwardOffset.z);
				//elapsed += SceneManager.AnimationDeltaTime();
				yield return null;
			}
		}
	}
	
	void SetMoveTime(float movetime) {
		_moveTime = movetime;	
	}
	
	public void SetTarget(Transform target) {
		targets[0] = target;	
	}
	
	public Transform GetTarget() {
		return targets[0];
	}
	
	public void ShakeCamera() {
		//iTween.ShakePosition(gameObject, new Vector3(10.0f, 0.0f, 10.0f), 1.0f);
	}
		
	
	public void OnDrawGizmos() {
		Vector3 pos = transform.position; pos.y-=100.0f;
		Gizmos.DrawWireCube(pos, new Vector3(20.0f, 20.0f, 20.0f));	
		//Gizmos.DrawWireCube(pos + scaledOffset, new Vector3(20.0f, 20.0f, 20.0f));
	}
}