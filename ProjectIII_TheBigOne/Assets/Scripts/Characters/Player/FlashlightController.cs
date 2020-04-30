using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public float maxCharge = 120f;
    private float _currentCharge;

    public float currentCharge
    {
        get { return _currentCharge; }
        set { _currentCharge = Mathf.Clamp(value, 0f, maxCharge); }
    }

    public bool IsFlashlightEnabled { get; private set; }

    public Light attachedLight;
    public GameObject feedbackVisual;
    private bool _isfeedbackVisualNotNull;
    private bool _isattachedLightNull;

    public void Start()
    {
        _isattachedLightNull = attachedLight == null;
        if (_isattachedLightNull)
            Debug.LogWarning("No light attached to the flashlight component in " + this.gameObject.name);
        _isfeedbackVisualNotNull = feedbackVisual != null;
        currentCharge = maxCharge;
    }

    private void Update()
    {
        ReduceCharge(1f, Time.deltaTime);
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
            feedbackVisual.SetActive(enable);
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
            return true;
        }

        return false;
    }
}