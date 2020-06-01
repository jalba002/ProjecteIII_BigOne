using Player;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera attachedCamera { get; private set; }

    [Header("Settings")] public bool invertMouse = false;
    [Range(0.1f, 10.0f)] public float m_Sensitivity = 1f;
    [Range(0.1f, 10.0f)] public float m_Smoothing = 3f;
    [Range(-100.0f, 100.0f)] public float m_MinPitch = -80f;
    [Range(-100.0f, 100.0f)] public float m_MaxPitch = 70f;

    public Transform m_PitchControllerTransform;
    //public Camera inspectCamera;

    [Space(10)] [Header("Editor Debug")] public KeyCode debugLockAngleKeyCode = KeyCode.I;
    public KeyCode debugLockKeyCode = KeyCode.O;
    public bool angleLocked = true;

    public bool applyRotation { get; set; }
    private bool _cursorLock;
    private bool _lerping = false;

    private Transform _newPos;
    private float _currentTime;
    private float _totalTime;
    private Transform _initPos;
    private bool _endedLerp = false;
    private Transform _initPitchControllerTransform;
    

    private void Start()
    {
        //inspectCamera.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public bool cursorLock
    {
        get { return _cursorLock; }
        set
        {
            if (value)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }

            _cursorLock = value;
        }
    }

    [Header("Private variables")] Vector2 m_MouseLook;
    Vector2 m_SmoothVector;
    PlayerController character;
    private Quaternion originalRotation;

    void Awake()
    {
        character = GetComponentInParent<PlayerController>();
        attachedCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (!this.isActiveAndEnabled) return;
        if (!angleLocked)
        {
            CalculateRotation();
        }

        if (!applyRotation)
            ApplyRotation();
        if (_lerping)
        {
            if(_currentTime < _totalTime)
            {
                _currentTime += Time.deltaTime;
                LerpToNewPosition(_newPos);
            }
            else
            {
                _lerping = false;
                if (!_endedLerp)
                {
                    Debug.Log("Hiya");
                    SetNewPosition(_newPos);
                    _endedLerp = true;
                }
               
            }            
        }

    }

#if UNITY_EDITOR
    void LateUpdate()
    {
        LockCameraAndMouse();
    }
#endif

    void CalculateRotation()
    {
        Vector2 deltaMouse = character.currentBrain.MouseInput;

        // Return to allow outside control.
        //if (deltaMouse.magnitude < 0.1f) return;

        deltaMouse.y = deltaMouse.y * (invertMouse ? 1f : -1f);

        deltaMouse = Vector2.Scale(deltaMouse,
            new Vector2(m_Sensitivity * m_Smoothing, m_Sensitivity * m_Smoothing));

        m_SmoothVector.x = Mathf.Lerp(m_SmoothVector.x, deltaMouse.x, 1f / m_Smoothing);
        m_SmoothVector.y = Mathf.Lerp(m_SmoothVector.y, deltaMouse.y, 1f / m_Smoothing);

        m_MouseLook += m_SmoothVector;

        m_MouseLook.y = Mathf.Clamp(m_MouseLook.y, m_MinPitch, m_MaxPitch);
    }

    public void ApplyRotation()
    {
        m_PitchControllerTransform.localRotation = Quaternion.AngleAxis(-1f * -m_MouseLook.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(m_MouseLook.x, character.transform.up);
    }

    public void ApplyExternalRotation(Quaternion rotation)
    {
        character.transform.rotation = rotation;
    }

    public void SetNewPosition(Transform newPosition, bool disableRotation = true)
    {
        originalRotation = this.gameObject.transform.localRotation;
        this.gameObject.transform.parent = newPosition;
        this.gameObject.transform.position = newPosition.transform.position;
        this.gameObject.transform.rotation = newPosition.transform.rotation;
        if (disableRotation)
        {
            angleLocked = true;
            this.enabled = false;
        }
    }

    public void StartLerpToPos(Transform newPosition, float seconds = 1f)
    {
        _lerping = true;
        _currentTime = 0;
        _totalTime = seconds;
        _initPos = this.gameObject.transform;
        _newPos = newPosition;
        _endedLerp = false;
        _initPitchControllerTransform = m_PitchControllerTransform;
    }
    private void LerpToNewPosition(Transform newPosition, float seconds = 1f)
    {
        Transform cam = this.gameObject.transform.parent;
        cam.position = Vector3.Lerp(_initPos.position, newPosition.position, _currentTime / _totalTime);
        cam.rotation = Quaternion.Lerp(_initPos.rotation, newPosition.rotation, _currentTime / _totalTime);

    }


    public void RestorePosition()
    {
        Debug.Log("Restoring Pos");
        
        m_PitchControllerTransform.localPosition = new Vector3(0, 0.72f, 0);
        this.gameObject.transform.parent = m_PitchControllerTransform;
        this.gameObject.transform.localPosition = Vector3.zero;
        this.gameObject.transform.localRotation = originalRotation;
        angleLocked = false;   
        this.enabled = true;
        _lerping = false;
        _initPos = null;
        _newPos = null;
        _endedLerp = false;
        _initPitchControllerTransform = null ;
    }

    /// <summary>
    /// Debug lock keys.
    /// </summary>
#if UNITY_EDITOR
    void LockCameraAndMouse()
    {
        if (Input.GetKeyDown(debugLockAngleKeyCode))
            angleLocked = !angleLocked;

        if (Input.GetKeyDown(debugLockKeyCode))
            cursorLock = !cursorLock;
    }
#endif
}