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
        if (IsFlashlightEnabled) currentCharge -= Time.deltaTime;
    }

    public bool ToggleFlashlight()
    {
        if (attachedLight == null)
        {
            Debug.LogWarning("No light attached to the flashlight component in " + this.gameObject.name);
            return false;
        }

        IsFlashlightEnabled = !IsFlashlightEnabled;
        //attachedLight.enabled = _Enabled;
        feedbackVisual.SetActive(IsFlashlightEnabled);
        return true;
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