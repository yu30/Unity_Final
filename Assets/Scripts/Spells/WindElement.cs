using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spells
{
    public class WindElement : MonoBehaviour, ISpellHarmony
    {
        #region Public Fields

        public float rotateSpeed = 60f;
        public float damage = 2f;
        public float damageCd = 0.5f;
        public float decayTime = 8f;
        public float decayTimeVariance = 1.5f;
        
        public ParticleSystem childParticle;

        #endregion

        private ParticleSystem _particle;
        private Transform _transform;
        private bool _canHurt = true;
        private bool _isDying;
        
        #region Unity Methods

        private void Start()
        {
            decayTime += decayTimeVariance * (Random.value*2 - 1);
            
            StartCoroutine(Decaying());
            _particle = GetComponent<ParticleSystem>();
            _transform = GetComponent<Transform>();

            // If this projectile has death particle, then we should turn off play on awake
            // and play it's child particle
            if (_particle)
            {
                if (childParticle && childParticle.gameObject.activeSelf)
                    childParticle.Play();
            }
        }

        private void Update()
        {
            // If not dying, then rotate around player
            if (!_isDying)
            {
                _transform.RotateAround(_transform.parent.position, Vector3.up, rotateSpeed * Time.deltaTime);
                _transform.Rotate(Vector3.forward, 1f, Space.Self);
            }
        }

        // TODO : On trigger stay???
        private void OnTriggerEnter(Collider other)
        {
            // Wait for CD
            if (!_canHurt) return;
            
            // Refactor : better detection 
            if (other.gameObject.name == "Player") return;

            EntityStats stats = other.GetComponent<EntityStats>();
            if (stats)
            {
                stats.ReceiveDamage(damage);
                _canHurt = false;
                StartCoroutine(DamageCdRecover());
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        #endregion

        #region Private Methods

        private IEnumerator Decaying()
        {
            while (decayTime > 0)
            {
                decayTime -= Time.deltaTime;
                yield return null;
            }

            ParticleDeath();
        }

        private void ParticleDeath()
        {
            _isDying = true;
            _transform.parent = null;
            
            // If this projectile has death effect, then play it,
            // and wait for particle system to destroy this gameObject
            // UPDATE : don't need this code anymore
            // if (_particle)
            // {
            // int childCount = _transform.childCount;
            // for (int i = childCount - 1; i >= 0; i--)
            //     Destroy(_transform.GetChild(i).gameObject);
            //
            //     _particle.Play();
            // }
            // If not, just destroy
            // else
            // {
            if (childParticle && childParticle.gameObject.activeSelf)
                childParticle.Stop();
            Destroy(gameObject, 1.5f);
            // }
        }

        private IEnumerator DamageCdRecover()
        {
            float timer = 0;
            while (timer < damageCd)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            
            _canHurt = true;
        }
        
        #endregion
        
        public void Harmony()
        {
            // TODO : Temporary EFX
            transform.localScale *= 3;
        }
    }
}