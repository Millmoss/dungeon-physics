using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerCam : MonoBehaviour
{
	private enum CAMERA_STATE
	{
		FREE_CAM,
		LOCK_CAM,
		LERPING_TO_LOCK_CAM,
		LERPING_TO_FREE_CAM
	};

	private enum CURRENT_INPUT
	{
		INTERACT_DOWN,
		INTERACT_HELD,
		INTERACT_UP,
		CANCEL_INTERACT,
		NONE
	}

	// For moving the camera around in Free cam mode
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -47F;
    public float MaximumX = 47F;
    public bool smooth;
    public float smoothTime = 5f;
    public bool lockCursor = true;
	public Camera cam;

    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;
    private bool m_cursorIsLocked = true;
	
	private PlayerMove playerMove;

	// For lerping:
	private const float LERP_SPEED = 2.0f;
	private Transform lockPos;
	private CAMERA_STATE cameraState;
	private float startLerpTime;
	private float lerpTimer;
	private Vector3 lerpOrigin;
	private Quaternion lerpRotationOrigin;

	private CURRENT_INPUT inputState;
	private float deltaX;
	private float deltaY;
    

    
	void Start ()
    {
        m_CharacterTargetRot = gameObject.transform.rotation;
        m_CameraTargetRot = cam.transform.rotation;
        smooth = false;
        cameraState = CAMERA_STATE.FREE_CAM;
		playerMove = gameObject.GetComponent<PlayerMove>();
		inputState = CURRENT_INPUT.NONE;
		deltaX = 0f;
		deltaY = 0f;
        Application.targetFrameRate = 90;
	}

	void FixedUpdate() {
		if (Input.GetMouseButtonDown(0)) {
			inputState = CURRENT_INPUT.INTERACT_DOWN;
		} else if (Input.GetMouseButton(0)) {
			inputState = CURRENT_INPUT.INTERACT_HELD;
		} else if (Input.GetMouseButtonUp(0)) {
			inputState = CURRENT_INPUT.INTERACT_UP;
		} else if (Input.GetMouseButtonDown(1)) {
			inputState = CURRENT_INPUT.CANCEL_INTERACT;
		} else {
			inputState = CURRENT_INPUT.NONE;
		}
		deltaX = CrossPlatformInputManager.GetAxis ("Mouse X");
		deltaY = CrossPlatformInputManager.GetAxis ("Mouse Y");

		// We want lerping to occur in fixed intervals
		if (cameraState == CAMERA_STATE.LERPING_TO_LOCK_CAM)
		{
			SetCursorLock();
		}
		else if (cameraState == CAMERA_STATE.LERPING_TO_FREE_CAM)
		{
			SetCursorLock();
		}
		else if (cameraState == CAMERA_STATE.FREE_CAM)
		{
			LookRotation(gameObject.transform, cam.transform);
		}
		else if (cameraState == CAMERA_STATE.LOCK_CAM)
		{
			SetCursorLock();
		}

		inputState = CURRENT_INPUT.NONE;
	}

	private void SetCursorLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void LookRotation(Transform character, Transform camera)
    {
        float yRot = deltaX * XSensitivity;
        float xRot = deltaY * YSensitivity;

        m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
        m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

        if (clampVerticalRotation)
            m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

        if (smooth)
        {
            character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                smoothTime * Time.deltaTime);
            camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                smoothTime * Time.deltaTime);
        }
        else
        {
            character.localRotation = m_CharacterTargetRot;
            camera.localRotation = m_CameraTargetRot;
        }

        UpdateCursorLock();
    }

    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if (!lockCursor)
        {//we force unlock the cursor if the user disable the cursor locking helper
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        //if the user set "lockCursor" we check & properly lock the cursos
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_cursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_cursorIsLocked = true;
        }

        if (m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}