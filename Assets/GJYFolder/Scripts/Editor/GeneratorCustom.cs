using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlatformGenerator))]
public class GeneratorCustom : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlatformGenerator generator = (PlatformGenerator)target;

        // 버튼을 생성하고 클릭 시 호출될 함수를 설정합니다.
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
