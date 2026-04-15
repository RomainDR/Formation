
    using UnityEngine;

    public class PlayerAnimationEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem smoke;

        public void PlaySmoke()
        {
            Instantiate(smoke, transform.position, transform.rotation * Quaternion.Euler(90f, 0f, 0f));
        }
 }