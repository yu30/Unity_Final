using System.Collections;
using UnityEngine;

namespace Spells
{
    public class Trap : MonoBehaviour, ISpellHarmony
    {
        #region Public Fields
        
        public float damage = 8f;
        public float decayTime = 10f;

        public Animation spikeAnim;

        #endregion

        private ParticleSystem _particle;
        private Transform _transform;

        #region Unity Methods

        private void Start()
        {
            StartCoroutine(Decaying());
            _particle = GetComponent<ParticleSystem>();
            _transform = GetComponent<Transform>();
        }
        

        private void OnTriggerEnter(Collider other)
        {
            // Refactor : better detection 
            if (other.gameObject.name == "Player") return;

            EntityStats stats = other.GetComponent<EntityStats>();
            if (stats)
            {
                TrapActivate(stats);
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

            TrapDied();
        }

        private void TrapActivate(EntityStats target)
        {
            // Prevent decaying and activating at the same time
            StopAllCoroutines();
            
            spikeAnim.Play();
            StartCoroutine(AnimFinished(target));
        }

        private void TrapDied()
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
                // TODO : Trap died visual effect
                Destroy(gameObject);
            }
        }

        private IEnumerator AnimFinished(EntityStats target)
        {
            // Sync damage with trap animation
            yield return new WaitForSeconds(7/12f);
            target.ReceiveDamage(damage);
            yield return new WaitForSeconds(5/12f);
            TrapDied();
        }
        
        #endregion
        
        public void Harmony()
        {
            // TODO : Temporary EFX
            transform.localScale *= 3;
        }
    }
}