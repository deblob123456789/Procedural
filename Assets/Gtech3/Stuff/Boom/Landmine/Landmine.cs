using UnityEngine;

namespace deblob123456789
{
    public class Landmine : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }
    }
}