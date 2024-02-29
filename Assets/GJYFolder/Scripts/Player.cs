using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] string _soundTag;
    [SerializeField] GameObject _broken;

    public bool IsDead { get; private set; } = false;

    private void Awake()
    {
        GameManager.Instance.OnPlayerDie += () => IsDead = true;
    }

    public void CarCrash()
    {
        Debug.Log("크래쉬!");
        SoundManager.Instance.PlayAudioClip(_soundTag);
        _broken.SetActive(true);

        _broken.transform.position = transform.position;
        gameObject.SetActive(false);

        GameManager.Instance.GameOver();
    }
}
