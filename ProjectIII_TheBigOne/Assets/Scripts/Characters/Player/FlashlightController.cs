using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    [Header("Cheats")] public bool infiniteCharge = false;
    [Header("Configuration")] public float maxCharge = 120f;
    private float _currentCharge;

    public float currentCharge
    {
        get { return _currentCharge; }
        set { _currentCharge = Mathf.Clamp(value, 0f, maxCharge); }
    }

    public bool IsFlashlightEnabled { get; private set; }

    [Header("Visual Feedback")] public Light attachedLight;
    public GameObject feedbackVisual;
    private bool _isfeedbackVisualNotNull;
    private bool _isattachedLightNull;

    [Range(0f, 1f)] public float lightFlickerThreshold = 0.2f;
    [Range(0f, 1f)] public float minimumIntensity = 0.33f;
    private float _initLightIntensity;
    private float _currentLightIntensity;
    private float _initLightRange;
    private float _currentLightRange;

    public void Start()
    {
        _isattachedLightNull = attachedLight == null;
        if (_isattachedLightNull)
            Debug.LogWarning("No light attached to the flashlight component in " + this.gameObject.name);
        _isfeedbackVisualNotNull = feedbackVisual != null;
        currentCharge = maxCharge;

        _initLightRange = attachedLight.range;
        _currentLightRange = _initLightRange;
        _initLightIntensity = attachedLight.intensity;
        _currentLightIntensity = _initLightIntensity;

        SetFlashlight(false);
    }

    private void Update()
    {
        ReduceCharge(infiniteCharge ? 0f : 1f, Time.deltaTime);
        ReduceIntensity(maxCharge * lightFlickerThreshold);
    }

    private void ReduceCharge(float amount, float deltaTime)
    {
        if (!IsFlashlightEnabled) return;
        currentCharge -= Mathf.Abs(amount) * deltaTime;
        if (currentCharge <= 0) DisableFlashlight();
    }

    public bool ToggleFlashlight()
    {
        if (_isattachedLightNull)
        {
            return false;
        }

        if (currentCharge <= 0)
        {
            return false;
        }

        SetFlashlight(!IsFlashlightEnabled);
        return true;
    }

    private void SetFlashlight(bool enable)
    {
        IsFlashlightEnabled = enable;
        if (_isfeedbackVisualNotNull)
        {
            feedbackVisual.SetActive(enable);
        }

        attachedLight.enabled = enable;
        attachedLight.intensity = enable ? _currentLightIntensity : 0;
        //TODO: Play sound of a torch interruptor
    }

    private void DisableFlashlight()
    {
        SetFlashlight(false);
    }

    public bool Recharge(float amount)
    {
        if (currentCharge < maxCharge)
        {
            currentCharge += amount;
            _currentLightRange = _initLightRange;
            _currentLightIntensity = _initLightIntensity;
            return true;
        }

        return false;
    }

    private void ReduceIntensity(float threshold)
    {
        if (!IsFlashlightEnabled) return;
        if (currentCharge <= threshold)
        {
            var amount = (currentCharge / threshold);
            _currentLightIntensity = Mathf.Lerp(minimumIntensity * _initLightIntensity, _initLightIntensity, amount);
            _currentLightRange = Mathf.Lerp(minimumIntensity * _initLightRange, _initLightRange, amount);
        }

        attachedLight.range = _currentLightRange;
        attachedLight.intensity = _currentLightIntensity;
    }
}