using Bonjoura.Resource;
using Bonjoura.Services;
using UnityEngine;

namespace Bonjoura.Enemy
{
    public class EnemyHealth : Health
    {
        [SerializeField] private BaseResource _resource;
             
        private void OnDie()
        {
            _resource.Get();

            Destroy(gameObject);
        }

        private void OnEnable()
        {
            OnDieEvent += OnDie;
        }
        
        private void OnDisable()
        {
            OnDieEvent -= OnDie;
        }
    }
}