using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] string _soundTag;
    [SerializeField] GameObject _broken;
    [SerializeField] Transform _parent;

    private Collider _collider;    

    public bool IsDead { get; private set; } = false;

    private void Awake()
    {
        _collider = GetComponent<Collider>();

        GameManager.Instance.OnPlayerDie += () => { IsDead = true; _collider.isTrigger = true; };
    }

    public void CarCrash()
    {
        Debug.Log("크래쉬!");
        SoundManager.Instance.PlayAudioClip(_soundTag);
        _broken.SetActive(true);

        _broken.transform.position = transform.position;
        gameObject.SetActive(false);

        Dead();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Border"))
            Dead();
    }

    public void EnterLog(Transform logTransform)
    {
        transform.SetParent(logTransform);
    }

    public void ExitLog()
    {
        transform.SetParent(_parent);
    }

    public void Dead()
    {
        GameManager.Instance.GameOver();
    }
}
