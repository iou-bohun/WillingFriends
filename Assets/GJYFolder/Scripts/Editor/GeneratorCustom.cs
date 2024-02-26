using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlatformGenerator))]
public class GeneratorCustom : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlatformGenerator generator = (PlatformGenerator)target;

        // ��ư�� �����ϰ� Ŭ�� �� ȣ��� �Լ��� �����մϴ�.
        if (GUILayout.Button("[START] Generate Platform"))
        {
            generator.StartGenerate();
        }
        if (GUILayout.Button("[STOP] Generate Platform"))
        {
            generator.StopGenerate();
        }
    }
}
