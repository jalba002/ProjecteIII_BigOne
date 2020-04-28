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
    private bool _Enabled { get; set; }

    public Light attachedLight;
    public GameObject feedbackVisual;

    public void Start()
    {
        currentCharge = maxCharge;
    }

    private void Update()
    {
        if (_Enabled) currentCharge -= Time.deltaTime;
    }

    public bool ToggleFlashlight()
    {
        if (attachedLight == null)
        {
            Debug.LogWarning("No light attached to the flashlight component in " + this.gameObject.name);
            return false;
        }

        _Enabled = !_Enabled;
        //attachedLight.enabled = _Enabled;
        feedbackVisual.SetActive(_Enabled);
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