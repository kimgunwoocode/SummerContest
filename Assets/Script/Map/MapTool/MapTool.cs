using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using static UnityEngine.Rendering.DebugUI;
using System.Xml.Linq;


public class MapTool : EditorWindow
{
    Dictionary<string, List<GameObject>> categorizedPrefabs;
    Dictionary<string, bool> foldoutStates;
    GameObject selectedPrefab;
    Vector2 scroll;

    [MenuItem("Window/MapTool")]
    public static void ShowWindow()
    {
        GetWindow<MapTool>("Map Tool");
    }

    void OnEnable()
    {
        LoadPrefabs();
        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    void LoadPrefabs()
    {
        categorizedPrefabs = new Dictionary<string, List<GameObject>>();
        foldoutStates = new Dictionary<string, bool>();

        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefab/Map" });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            string category = GetCategoryFromPath(prefab);
            if (!categorizedPrefabs.ContainsKey(category))
            {
                categorizedPrefabs[category] = new List<GameObject>();
                foldoutStates[category] = true;
            }

            categorizedPrefabs[category].Add(prefab);
        }
    }

    void OnGUI()
    {
        if (categorizedPrefabs == null || categorizedPrefabs.Count == 0)
        {
            EditorGUILayout.HelpBox("No prefabs found in Assets/Prefab/Map", MessageType.Info);
            return;
        }

        EditorGUILayout.LabelField("Map Prefabs", EditorStyles.boldLabel);
        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach (var kvp in categorizedPrefabs)
        {
            string category = kvp.Key;
            List<GameObject> prefabs = kvp.Value;

            foldoutStates[category] = EditorGUILayout.Foldout(foldoutStates[category], category, true);
            if (foldoutStates[category])
            {
                EditorGUI.indentLevel++;
                foreach (GameObject prefab in prefabs)
                {
                    if (prefab == null) continue;

                    EditorGUILayout.BeginHorizontal();

                    string label = selectedPrefab == prefab ? $"▶ {prefab.name}" : prefab.name;
                    if (GUILayout.Button(label))
                    {
                        selectedPrefab = prefab;
                    }

                    // 프리팹 위치로 이동 버튼
                    if (GUILayout.Button("E", GUILayout.Width(30)))
                    {
                        EditorGUIUtility.PingObject(prefab);
                        Selection.activeObject = prefab;
                    }

                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUILayout.Space(15);
        if (GUILayout.Button("맵 오브젝트 ID 일괄 설정"))
        {
            AssignIDsAndSaveToInitData();
            //AssignIDs();
        }

        EditorGUILayout.Space();
        GUI.enabled = selectedPrefab != null;
        if (GUILayout.Button("선택 해제"))
        {
            selectedPrefab = null;
        }
        GUI.enabled = true;
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (selectedPrefab == null) return;

        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        Plane plane = new Plane(Vector3.forward, 0);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 point = ray.origin + ray.direction * distance;
            Vector3 gridPoint = SnapToGrid(point, 1f);

            Handles.color = Color.green;
            Handles.DrawWireCube(gridPoint, Vector3.one);

            if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
            {
                GameObject placed = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);
                Undo.RegisterCreatedObjectUndo(placed, "Place Map Element");
                placed.transform.position = gridPoint;

                string category = GetCategoryFromPath(selectedPrefab);
                Transform parent = EnsureHierarchy(category);
                placed.transform.SetParent(parent);

                // 설치한 프리팹 선택 상태로 유지
                Selection.activeGameObject = placed;

                selectedPrefab = null;
                e.Use();
            }

            SceneView.RepaintAll();
        }
    }




    #region 맵의 다양한 오브젝트 ID 자동 부여 및 설정, 저장까지!
    void AssignIDs()
    {
        int mainID = 2001;
        int semiID = 1001;
        int totalMain = 0;
        int totalSemi = 0;

        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes/Map" });

        foreach (string guid in sceneGuids)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guid);
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

            SavePoint[] savePoints = GameObject.FindObjectsByType<SavePoint>(FindObjectsSortMode.None);

            foreach (var sp in savePoints)
            {
                if (sp == null) continue;

                int assignedID = -1;

                switch (sp.SavePoint_type)
                {
                    case SavePoint.SP_type.Main:
                        assignedID = mainID++;
                        totalMain++;
                        break;
                    case SavePoint.SP_type.Semi:
                        assignedID = semiID++;
                        totalSemi++;
                        break;
                }

                if (assignedID != -1)
                {
                    sp.ID = assignedID;
                    EditorUtility.SetDirty(sp);

                }
            }

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
        }

        AssetDatabase.SaveAssets();

        Debug.Log($"SavePoint ID 부여 완료! Main: {totalMain}, Semi: {totalSemi}");

    }
    private const string InitSaveDataPath = "Assets/InitData/Init Save Data.asset";
    private const string MapScenePath = "Assets/Scenes/Map";

    [MenuItem("Tools/Assign IDs and Save to InitSaveData")]
    public static void AssignIDsAndSaveToInitData()
    {
        InitSaveData initData = AssetDatabase.LoadAssetAtPath<InitSaveData>(InitSaveDataPath);

        if (initData == null)
        {
            Debug.LogError("InitSaveData.asset 을 찾을 수 없습니다. 경로를 확인해주세요.");
            return;
        }

        // 초기화
        initData.MapData = new MapData();

        int mainID = 2001;
        int semiID = 1001;
        int shopID = 3001;
        int interactionID = 4001;

        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { MapScenePath });

        foreach (string guid in sceneGuids)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guid);
            SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

            // SavePoint
            var savePoints = Object.FindObjectsByType<SavePoint>(FindObjectsSortMode.None);
            foreach (var sp in savePoints)
            {
                int id = -1;
                switch (sp.SavePoint_type)
                {
                    case SavePoint.SP_type.Main:
                        id = mainID++;
                        break;
                    case SavePoint.SP_type.Semi:
                        id = semiID++;
                        break;
                }

                if (id != -1)
                {
                    sp.ID = id;
                    initData.MapData.SpawnPoints[id] = sp.SavePointEnabled;
                    EditorUtility.SetDirty(sp);
                }
            }

            // ShopObject
            var shops = Object.FindObjectsByType<ShopObject>(FindObjectsSortMode.None);
            foreach (var shop in shops)
            {
                int id = shopID++;
                shop.ID = id;

                var shopData = new ShopData
                {
                    ID = id,
                    isOpened = shop.isOpened,
                    Items = new Dictionary<int, bool>() // 비워둠. 초기 상태로는 필요 없을 수도 있음
                };

                initData.MapData.Shops.Add(shopData);
                EditorUtility.SetDirty(shop);
            }

            // Interaction
            var interactions = Object.FindObjectsByType<Interaction>(FindObjectsSortMode.None);
            foreach (var interaction in interactions)
            {
                int id = interactionID++;
                interaction.ID = id;
                initData.MapData.InteractionObjects[id] = interaction.isInteracted;
                EditorUtility.SetDirty(interaction);
            }

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
        }

        EditorUtility.SetDirty(initData);
        AssetDatabase.SaveAssets();
        Debug.Log("ID 자동 부여 및 InitSaveData 저장 완료!");
    }
    #endregion


    Vector3 SnapToGrid(Vector3 pos, float size)
    {
        pos.x = Mathf.Round(pos.x / size) * size;
        pos.y = Mathf.Round(pos.y / size) * size;
        pos.z = 0;
        return pos;
    }

    string GetCategoryFromPath(GameObject prefab)
    {
        string path = AssetDatabase.GetAssetPath(prefab); // Assets/Prefab/Map/...
        string[] parts = path.Split('/');
        int mapIndex = System.Array.IndexOf(parts, "Map");
        if (mapIndex >= 0 && mapIndex + 1 < parts.Length)
        {
            return parts[mapIndex + 1];
        }
        return "Uncategorized";
    }

    Transform EnsureHierarchy(string category)
    {
        GameObject mapRoot = GameObject.Find("Map");
        if (mapRoot == null)
        {
            mapRoot = new GameObject("Map");
            Undo.RegisterCreatedObjectUndo(mapRoot, "Create Map Root");
        }

        Transform categoryObj = mapRoot.transform.Find(category);
        if (categoryObj == null)
        {
            GameObject newGroup = new GameObject(category);
            Undo.RegisterCreatedObjectUndo(newGroup, "Create Category Group");
            newGroup.transform.SetParent(mapRoot.transform);
            categoryObj = newGroup.transform;
        }

        return categoryObj;
    }
}
