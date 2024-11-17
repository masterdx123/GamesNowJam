using Interfaces;
using UnityEngine;

namespace Entity.Enemy
{
    public class VoltLeechBehaviour : MonoBehaviour
    {
        public GameObject AttachTarget { private get => _attachTarget; set => _attachTarget = value; }
        private GameObject _attachTarget;
        private GameObject _previousTarget;
        
        void Start()
        {
            
        }

        protected void Update()
        {
            if (_attachTarget != _previousTarget && _previousTarget)
            {
                gameObject.transform.parent = null;
            }
        }

        public void AttachToTarget(float damage, GameObject target)
        {
            if (_attachTarget)
            {
                IDamageable damageable = _attachTarget.GetComponent<IDamageable>() ?? _attachTarget.GetComponentInChildren<IDamageable>();
                damageable?.TakeDamage(damage);
                return;
            }
            
            _attachTarget = target;
            _previousTarget = _attachTarget;
            gameObject.transform.parent = target.transform;
        }
    }
}
