using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour {

    LevelLoader instance;
    Transform cameraTransform;

    [SerializeField]
    GameObject[] prefabs;
    [SerializeField]
    Vector3[] colors;

    Dictionary<Vector3, GameObject> objectDictionary = new Dictionary<Vector3, GameObject>();
    LevelObject[,] loadedLevelObjects;
    GameObject[,] currentLevelObjects;
    int mapWidth;
    int mapHeight;

    public delegate void LevelLoaded(string levelName);
    public static event LevelLoaded OnLevelLoaded;

    public delegate void LevelSpawned();
    public static event LevelSpawned OnLevelSpawned;

    void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
    }

    void Start()
    {
        cameraTransform = Camera.main.gameObject.transform;

        for (int i = 0; i < prefabs.Length; i++)
        {
            objectDictionary.Add(colors[i], prefabs[i]);
        }

        LoadLevelInfo("level2");
        SpawnLevel();
    }

    void LoadLevelInfo(string levelName)
    {
        Texture2D levelToLoad = Resources.Load<Texture2D>(string.Format("Levels/{0}", levelName));

        mapWidth = levelToLoad.width;
        mapHeight = levelToLoad.height;
        Color32[] pixelColors = levelToLoad.GetPixels32();
        loadedLevelObjects = new LevelObject[mapWidth, mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                Color32 currentPixelColor = pixelColors[(y * mapWidth) + x];
                Vector3 key = new Vector3(currentPixelColor.r, currentPixelColor.g, currentPixelColor.b);
                GameObject go;
                objectDictionary.TryGetValue(key, out go);

                if (go != null)
                    loadedLevelObjects[x, y] = new LevelObject(go, currentPixelColor.a);
            }
        }

        if (OnLevelLoaded != null)
            OnLevelLoaded(levelName);
    }

    void SpawnLevel()
    {
        currentLevelObjects = new GameObject[mapWidth, mapHeight];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                GameObject prefab = loadedLevelObjects[x, y].prefab;
                if (prefab != null)
                {
                    GameObject obj = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
                    currentLevelObjects[x, y] = obj;
                }
            }
        }
        cameraTransform.position = new Vector3(mapWidth/2, mapHeight/2, -10);

        if (OnLevelSpawned != null)
            OnLevelSpawned();
    }

    void ClearLevel()
    {
        foreach (GameObject go in currentLevelObjects)
        {
            Destroy(go);
        }
    }

    struct LevelObject
    {
        public GameObject prefab;
        public int additionalInfo;

        /// <summary>
        /// Stored all information related
        /// </summary>
        /// <param name="_prefab">GameObject associated with this level object</param>
        /// <param name="_additionalInfo">Aditional Information in Alpha channel</param>
        public LevelObject(GameObject _prefab, int _additionalInfo)
        {
            prefab = _prefab;
            additionalInfo = _additionalInfo;
        }
    }
}
