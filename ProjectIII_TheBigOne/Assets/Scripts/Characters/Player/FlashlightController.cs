using System.Diagnostics.Contracts;
using UnityEditor.UI;
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

    public void Start()
    {
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
        if (attachedLight == null)
        {
            Debug.LogWarning("No light attached to the flashlight component in " + this.gameObject.name);
            return false;
        }

        if (currentCharge <= 0)
        {
            Debug.LogWarning("No flashlight battery left.");
            return false;
        }

        SetFlashlight(!IsFlashlightEnabled);
        return true;
    }

    private void SetFlashlight(bool enable)
    {
        IsFlashlightEnabled = enable;
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