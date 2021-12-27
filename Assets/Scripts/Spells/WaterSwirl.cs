using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spells
{
    public class WaterSwirl : MonoBehaviour, ISpellHarmony
    {
        #region Public Fields

        public float damage = 5f;
        public float damageCd = 0.5f;
        public float decayTime = 5f;

        public GameObject balls;

        #endregion

        private ParticleSystem _particle;
        private Transform _transform;
        private bool _canHurt = true;
        private bool _died = false;
        
        #region Unity Methods

        private void Start()
        {
            StartCoroutine(Decaying());
            StartCoroutine(SpawnBalls());
            _particle = GetComponent<ParticleSystem>();
            _transform = GetComponent<Transform>();

            // If this projectile has death particle, then we should turn off play on awake
            // and play it's child particle
            // if (_particle)
            // {
            //     if (childParticle)
            //         childParticle.Play();
            // }
        }

        private void Update()
        {
            if (!_died)
                _transform.Rotate(Vector3.forward, 1f, Space.Self);
        }

        private void OnTriggerStay(Collider other)
        {
            // wait for CD
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
            // If this projectile has death effect, then play it,
            // and wait for particle system to destroy this gameObject
            if (_particle)
            {
                int childCount = _transform.childCount;
                for (int i = childCount - 1; i >= 0; i--)
                    Destroy(_transform.GetChild(i).gameObject);

                _particle.Play();
            }
            // If not, just destroy
            else
            {
                // childParticle.Stop();
                Destroy(gameObject, 1);
            }
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

        private IEnumerator SpawnBalls()
        {
            float rnd = Random.value/10 + 0.01f;
            yield return new WaitForSeconds(rnd);
            Vector3 pos = new Vector3(Random.value-0.5f, Random.value-0.5f, 0.3f);
            Instantiate(balls, _transform.position + pos, _transform.rotation, _transform);
            StartCoroutine(SpawnBalls());
        }
        
        #endregion
        
        public void Harmony()
        {
            // TODO : Temporary EFX
            transform.localScale *= 3;
        }
    }
}