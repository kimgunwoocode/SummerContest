using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraManager))]
public class CameraManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CameraManager camManager = (CameraManager)target;

        if (GUILayout.Button("카메라 바운드 자동 등록"))
        {
            GameObject mapObject = GameObject.FindGameObjectWithTag("Map");
            if (mapObject == null)
            {
                Debug.LogWarning("Map 태그를 가진 오브젝트를 찾을 수 없습니다.");
                return;
            }

            Transform cameraMove = mapObject.transform.Find("CameraMove");
            if (cameraMove == null)
            {
                Debug.LogWarning("Map 오브젝트 안에 'CameraMove' 오브젝트를 찾을 수 없습니다.");
                return;
            }

            int count = cameraMove.childCount;
            Collider2D[] boundsArray = new Collider2D[count];

            for (int i = 0; i < count; i++)
            {
                Transform child = cameraMove.GetChild(i);
                Collider2D col = child.GetComponent<Collider2D>();
                if (col != null)
                {
                    boundsArray[i] = col;
                }
                else
                {
                    Debug.LogWarning($"'{child.name}'에는 Collider2D가 없습니다. null로 설정됩니다.");
                }
            }

            Undo.RecordObject(camManager, "Auto Assign Camera Bounds");
            SerializedObject so = new SerializedObject(camManager);
            SerializedProperty boundsProp = so.FindProperty("_cameraBoundsList");
            boundsProp.arraySize = boundsArray.Length;

            for (int i = 0; i < boundsArray.Length; i++)
            {
                boundsProp.GetArrayElementAtIndex(i).objectReferenceValue = boundsArray[i];
            }

            so.ApplyModifiedProperties();

            EditorUtility.SetDirty(camManager);
        }
    }
}
