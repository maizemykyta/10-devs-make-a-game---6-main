using Bonjoura.Services;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.UI;

namespace Bonjoura.Player
{
    public class PlayerTemperature : MonoBehaviour
    {
        [Header("Temperature Settings")]
        [SerializeField] private float _temperature = 37f; 
        [SerializeField] private int _damageFromCold = 2;

        [Tooltip("How often does cold should damage the player")]
        [SerializeField] private float _damageRatio = 1.5f; 

        [SerializeField] private float _minTemperature = 20f; 
        [SerializeField] private float _raycastDistance = 1.5f;
        [SerializeField] private float _warmUpDelay = 5f;

        [SerializeField] private ParticleSystem _freezeParticleSystem;

        [Header("UI Settings")]
        [SerializeField] private TextMeshProUGUI _temperatureText;

        private float _initDamageRatio;
        private bool _isInWater = false;
        private float _timeToWarmUp;
        private float _coldRate = 1f;

        private void Start()
        {
            _timeToWarmUp = _warmUpDelay;
            _initDamageRatio = _damageRatio;
        }
        private void Update()
        {
            CheckIfStandingOnWater();

            if (_isInWater)
            {

                _temperature -= _coldRate * Time.deltaTime;
                _temperature = Mathf.Max(_temperature, _minTemperature);
                _timeToWarmUp = _warmUpDelay;
                CheckForDamageFromTemperature();
            } else
            {

                if (_timeToWarmUp >= 0)
                {
                    _timeToWarmUp -= Time.deltaTime;

                }
                else if (_temperature <= 37)
                {
                    _temperature += _coldRate * Time.deltaTime;
                }

                if (_temperature > 35)
                {
                    _freezeParticleSystem.Stop();
                }
            }
            
            UpdateTemperatureUI();
        }

        private void CheckIfStandingOnWater()
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, _raycastDistance))
            {
                if (hit.collider.CompareTag("Water"))
                {
                    _isInWater = true;
                    return;
                }
            }
            _isInWater = false;
        }

        private void CheckForDamageFromTemperature()
        {
            if (_damageRatio >= 0)
            {
                _damageRatio -= Time.deltaTime;
                if (_temperature <= 35 && _temperature > 33)
                {
                    _freezeParticleSystem.Play();
                }

                

                
            } else
            {
                _damageRatio = _initDamageRatio;
                if (_temperature <= 33)
                {
                    GetComponent<Health>().Damage(_damageFromCold);

                }
            }
        }

        private void UpdateTemperatureUI()
        {
            if (_temperatureText != null)
            {
                _temperatureText.text = $"{Mathf.RoundToInt(_temperature)}°C"; 
            }
        }
    }
}
