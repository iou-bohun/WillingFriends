using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingZone : MonoBehaviour
{
    [Header("Tag")]
    [SerializeField] string _soundTag;
    [SerializeField] ParticleSystem _waterEffect;    

    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Player"))
        {            
            SoundManager.Instance.PlayAudioClip(_soundTag, transform);
            GameObject effect = Instantiate(_waterEffect.gameObject);
            effect.transform.position = other.transform.position;

            StartCoroutine(Co_ReturnParticle());
        }            
    }

    private IEnumerator Co_ReturnParticle()
    {
        yield return new WaitForSeconds(3f);
        
        GameManager.Instance.GameOver();
    }
}
