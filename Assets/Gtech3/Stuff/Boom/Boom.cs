using UnityEngine;

namespace deblob123456789
{
    public class Boom : MonoBehaviour
    {
        void OnEnable()
        {
            AudioSource audio = transform.parent.GetChild(1).GetComponent<AudioSource>();
            audio.PlayOneShot(audio.clip);
        }

        public void AnimationEnd()
        {
            gameObject.SetActive(false);
        }
    }
}