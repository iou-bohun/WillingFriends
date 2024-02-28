using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : SingletoneBase<Managers>
{
    public static Action InitEvent; //�Ŵ������� ù �ʱ�ȭ��
    public static Action ClearEvent; //�Ŵ������� ��� ���� �� �ʱ�ȭ
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        AddAllInitialize();
    }
    public void AddAllInitialize() //���� ����
    {
        Debug.Log("Managers ����");
        InitEvent += GameManager.Instance.Initialize;
        InitEvent += ObjectPoolManager.Instance.Initialize;
        InitEvent += SoundManager.Instance.Initialize;
    }
    public void CallInitEvent()
    {
        InitEvent?.Invoke();
    }
    public void CallClearEvent()
    {
        ClearEvent?.Invoke();
    }
}
