using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using DG.Tweening;

public class Bomb : MonoBehaviour
{
    public GameObject explosionEffect;

    [Header("Shake Option")]
    [SerializeField] float _shakeDuration;
    [SerializeField] float _positionShakePower;
    [SerializeField] float _rotationShakePower;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            Camera.main.DOComplete();
            Camera.main.DOShakePosition(_shakeDuration, _positionShakePower, 15);
            Camera.main.DOShakeRotation(_shakeDuration, _rotationShakePower, 15);

            SoundManager.Instance.PlayAudioClip("bomb");

            GameObject exp = Instantiate(explosionEffect);
            exp.transform.position = transform.position;
            Destroy(exp, 2f);

            Collider[] cols = Physics.OverlapSphere(exp.transform.position, exp.transform.localScale.z * 2);
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].TryGetComponent(out Enemy enemy) == true)
                    enemy.Die();

                if (cols[i].TryGetComponent(out Tree tree) == true)
                    tree.DestroyTree();
            }
            ObjectPoolManager.Instance.ReturnObject("Bomb", gameObject);
        }
    }
}