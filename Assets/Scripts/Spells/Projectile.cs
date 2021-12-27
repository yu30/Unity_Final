using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Spells
{
    public class Projectile : MonoBehaviour, ISpellHarmony
    {
        #region Public Fields

        public float flyingSpeed = 60f;
        public float damage = 5f;
        public float decayTime = 5f;

        public ParticleSystem childParticle;

        #endregion

        private ParticleSystem _particle;
        private Transform _transform;
        private bool _died = false;
        
        #region Unity Methods

        private void Start()
        {
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
            if (!_died)
                _transform.Translate(flyingSpeed * Time.deltaTime * Vector3.forward);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_died) return;
            
            // Refactor : better detection 
            if (other.gameObject.name == "Player") return;

            EntityStats stats = other.GetComponent<EntityStats>();
            if (stats)
            {
                stats.ReceiveDamage(damage);
                ProjectileDeath();
            }
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

            ProjectileDeath();
        }

        private void ProjectileDeath()
        {
            _died = true;
            StopAllCoroutines();
            
            // If this projectile has death effect, then play it,
            // and wait for particle system to destroy this gameObject
            if (_particle && childParticle.gameObject.activeSelf)
            {
                int childCount = _transform.childCount;
                for (int i = childCount - 1; i >= 0; i--)
                    Destroy(_transform.GetChild(i).gameObject);

                _particle.Play();
            }
            // If not, just destroy
            else
            {
                // refactor : 1.5 for mesh effect to fade out , but it will leave a transparent sphere
                Destroy(gameObject, 1.5f);
            }
        }

        #endregion

        public void Harmony()
        {
            // TODO : Temporary EFX
            transform.localScale *= 3;
        }
    }
}