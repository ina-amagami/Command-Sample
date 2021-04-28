using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace InputCommand
{
    [CreateAssetMenu(menuName = "ScriptableObject/DemoData")]
    public class DemoData : ScriptableObject
    {
        [System.Serializable]
        public class CommandInfo
        {
            public Player.Action Action;
            public bool IsWait;
            public float WaitDuration;
        }
        public List<CommandInfo> CommandInfos = new List<CommandInfo>();
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DemoData))]
    public class DemoDataEditor : Editor
    {
        private SerializedProperty commandInfosProp;
        private List<DemoData.CommandInfo> list;
        private GUIContent waitLabel = new GUIContent("待機");
        private GUIContent actionLabel = new GUIContent("アクション");

        public void OnEnable()
        {
            commandInfosProp = serializedObject.FindProperty("CommandInfos");
            list = (target as DemoData).CommandInfos;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var count = commandInfosProp.arraySize;
            for (int i = 0; i < count; ++i)
            {
                var element = commandInfosProp.GetArrayElementAtIndex(i);

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField($"{i.ToString()}.", GUILayout.Width(20f));

                    if (element.FindPropertyRelative("IsWait").boolValue)
                    {
                        EditorGUILayout.PropertyField(element.FindPropertyRelative("WaitDuration"), waitLabel);
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(element.FindPropertyRelative("Action"), actionLabel);
                    }

                    var buttonWidth = GUILayout.Width(20f);

                    // 1つ上のものと入れ替え
                    if (i != 0)
                    {
                        if (GUILayout.Button("↑", buttonWidth))
                        {
                            var temp = list[i];
                            list[i] = list[i - 1];
                            list[i - 1] = temp;
                            Repaint();
                            return;
                        }
                    }
                    else
					{
                        GUILayout.Label("", buttonWidth);
					}

                    // 1つ下のものと入れ替え
                    if (i != count - 1)
                    {
                        if (GUILayout.Button("↓", buttonWidth))
                        {
                            var temp = list[i];
                            list[i] = list[i + 1];
                            list[i + 1] = temp;
                            Repaint();
                            return;
                        }
                    }
                    else
                    {
                        GUILayout.Label("", buttonWidth);
                    }

                    // 削除
                    if (GUILayout.Button("×", buttonWidth))
                    {
                        for (int j = i + 1; j < count; ++j)
                        {
                            list[j - 1] = list[j];
                        }
                        list.RemoveAt(count - 1);
                        Repaint();
                        return;
                    }
                }
            }

            EditorGUILayout.Separator();

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("アクション追加"))
                {
                    list.Add(new DemoData.CommandInfo()
                    {
                        IsWait = false
                    });
                    Repaint();
                    return;
                }
                if (GUILayout.Button("待機追加"))
                {
                    list.Add(new DemoData.CommandInfo()
                    {
                        IsWait = true
                    });
                    Repaint();
                    return;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
