#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace TT.SMVC.V1
{
    [CustomEditor(typeof(GearboxSetup))]
    public class GearboxSetupEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GearboxSetup gearboxSetup = (GearboxSetup)target;

            if (GUILayout.Button("Compute Ratios"))
            {
                gearboxSetup.ComputeValues();
                EditorUtility.SetDirty(gearboxSetup);
                EditorUtility.SetDirty(gearboxSetup.gearbox);
                AssetDatabase.SaveAssets();

                string assetPath = AssetDatabase.GetAssetPath(gearboxSetup.gearbox);
                Debug.Log("[Gearbox Setup] Ratios computed for gearbox at: " + assetPath, gearboxSetup.gearbox);
            }

            if (GUILayout.Button("Setup Gears"))
            {
                gearboxSetup.ComputeValues();
                gearboxSetup.SetupGearbox();
                EditorUtility.SetDirty(gearboxSetup);
                EditorUtility.SetDirty(gearboxSetup.gearbox);
                AssetDatabase.SaveAssets();

                string assetPath = AssetDatabase.GetAssetPath(gearboxSetup.gearbox);
                Debug.Log("[Gearbox Setup] Configured gearbox at: " + assetPath, gearboxSetup.gearbox);
            }
        }
    }
}
#endif