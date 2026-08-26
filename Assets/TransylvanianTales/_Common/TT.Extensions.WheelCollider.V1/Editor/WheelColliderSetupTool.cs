#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace TT.Extensions.V1
{
    public class WheelColliderSetupTool : EditorWindow
    {
        private WheelCollider wheelCollider;
        private Renderer wheelRenderer;

        private const string MenuRoot = "Tools/Transylvanian Tales/";


        [MenuItem(MenuRoot + "Wheel Collider Setup Tool/V1")]
        public static void ShowWindow()
        {
            GetWindow<WheelColliderSetupTool>("Wheel Collider Setup");
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Wheel Collider Setup", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            wheelCollider = (WheelCollider)EditorGUILayout.ObjectField(
                "Wheel Collider",
                wheelCollider,
                typeof(WheelCollider),
                true);

            wheelRenderer = (Renderer)EditorGUILayout.ObjectField(
                "Wheel Renderer",
                wheelRenderer,
                typeof(Renderer),
                true);

            EditorGUILayout.Space();

            GUI.enabled = wheelCollider != null && wheelRenderer != null;

            if (GUILayout.Button("Setup Wheel Collider"))
            {
                SetupWheel();
            }

            GUI.enabled = true;
        }

        private void SetupWheel()
        {
            Undo.RecordObject(wheelCollider.transform, "Setup Wheel Collider");
            Undo.RecordObject(wheelCollider, "Setup Wheel Collider");

            WheelColliderExtensions.Setup(wheelCollider, wheelRenderer);

            EditorUtility.SetDirty(wheelCollider);
            EditorUtility.SetDirty(wheelCollider.transform);

            Debug.Log("Wheel Collider setup completed.");
        }
    }
}
#endif