using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingZone : MonoBehaviour
{
    [SerializeField] ParticleSystem _waterEffect;    

    private void OnTriggerEnter(Collider other)
    {
        // 게임오버 로직은 GameManager 총괄.
        if (other.CompareTag("Player"))
        {
            Debug.Log("게임오버");
            GameObject effect = Instantiate(_waterEffect.gameObject);
            effect.transform.position = other.transform.position;

            StartCoroutine(Co_ReturnParticle(effect));
        }            
    }

    private IEnumerator Co_ReturnParticle(GameObject effect)
    {
        yield return new WaitForSeconds(2f);
        
        GameManager.Instance.GameOver();
    }
}
