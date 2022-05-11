using UnityEngine;

namespace UnityTemplateProjects
{
    public class AsteroidHealthComponent : HealthComponent,IDamageble
    {
        [SerializeField] private GameObject destructionParticles;
        public void TakeDamage(float damage)
        {
            Healt -= damage;
            print("Taking damage Hp: "+ Healt);
            if (Healt <= 0)
            {
                Destroy(this.gameObject);
            }
        }

        public void OnDestroy()
        {
            if (destructionParticles)
            {
                Destroy(Instantiate(destructionParticles, transform.position, Quaternion.identity),2f);
            }   
        }
    }
}