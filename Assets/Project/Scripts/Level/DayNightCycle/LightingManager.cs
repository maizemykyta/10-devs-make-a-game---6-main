using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light _directionalLight;
    [SerializeField] private LightingPreset _presetLight;

    [SerializeField, Range(0, 300)] private float _timeOfDay;

    private void Start()
    {
        _timeOfDay = 0;
    }

    private void Update()
    {
        if (_presetLight == null)
        {
            return;
        }

        _timeOfDay += Time.deltaTime;
        _timeOfDay %= 300;
        float timePercent = _timeOfDay / 300;

        UpdateLighting(timePercent);
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = _presetLight.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = _presetLight.FogColor.Evaluate(timePercent);

        if (_directionalLight != null)
        {
            _directionalLight.color = _presetLight.AmbientColor.Evaluate(timePercent);
            _directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360) - 90, 170, 0));
        }
    }

    private void OnValidate()
    {
        if (_directionalLight != null)
        {
            return;
        }

        if (RenderSettings.sun != null)
        {
            _directionalLight = RenderSettings.sun;
            return;
        }

        Light[] lights = FindObjectsOfType<Light>();

        foreach (Light light in lights)
        {
            if (light.type == LightType.Directional)
            {
                _directionalLight = light;
                return;
            }
        }
    }
}
