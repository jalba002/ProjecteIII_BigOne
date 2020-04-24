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
    
    [Space(10)]
    [Header("Editor Debug")]
    public KeyCode debugLockAngleKeyCode = KeyCode.I;
    public KeyCode debugLockKeyCode = KeyCode.O;
    public bool angleLocked = true;
        
    private bool _cursorLock;

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
    
    [Header("Private variables")]
    Vector2 m_MouseLook;
    Vector2 m_SmoothVector;
    PlayerController character;

    void Awake()
    {
        character = GetComponentInParent<PlayerController>();
        attachedCamera = GetComponent<Camera>();
    }

    void Update()
    {
        if (!angleLocked)
            Aiming();

        ApplyRotation();

#if UNITY_EDITOR
        LockCameraAndMouse();
#endif
    }

    void Aiming()
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