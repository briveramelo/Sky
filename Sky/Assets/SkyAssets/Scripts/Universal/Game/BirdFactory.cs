using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GenericFunctions;
using Random = UnityEngine.Random;

public class BirdFactory : Singleton<BirdFactory>
{
    [System.Serializable]
    private struct BirdPrefab
    {
        public BirdType BirdType;
        public GameObject Prefab;
    }

    private class BirdData
    {
        public GameObject Prefab;
        public Transform Parent;
    }

    [SerializeField] private BirdPrefab[] _birdPrefabs;
    [SerializeField] private Transform _birdParentTransform;
    private Dictionary<BirdType, BirdData> _birdTypeData = new Dictionary<BirdType, BirdData>();
    protected override bool _destroyOnLoad => true;
    private bool _displayBirdButtons;

    protected override void Awake()
    {
        base.Awake();
        InitializeBirdPrefabData();
    }

    private void InitializeBirdPrefabData()
    {
        foreach (var birdPrefab in _birdPrefabs)
        {
            var container = new GameObject(birdPrefab.BirdType.ToString());
            container.transform.SetParent(_birdParentTransform);
            
            _birdTypeData[birdPrefab.BirdType] = new BirdData
            {
                Prefab = birdPrefab.Prefab,
                Parent = container.transform
            };
        }
    }

    public void CreateNextBird(BirdType birdType)
    {
        var xSpot = -ScreenSpace.WorldEdge.x;
        var ySpot = Random.Range(-ScreenSpace.WorldEdge.y, ScreenSpace.WorldEdge.y) * 0.6f;
        if (birdType == BirdType.Tentacles || birdType == BirdType.Crow)
        {
            xSpot = 0f;
            ySpot = 0f;
        }
        else if (birdType == BirdType.Eagle)
        {
            xSpot = -ScreenSpace.WorldEdge.x * 5f;
        }
        else if (birdType == BirdType.Seagull)
        {
            xSpot = ScreenSpace.WorldEdge.x * (Bool.TossCoin() ? 1 : -1);
        }

        CreateBird(birdType, new Vector3(xSpot, ySpot));
    }

    public Bird CreateBird(BirdType birdType, Vector2 position)
    {
        var birdGameObject = Instantiate(_birdTypeData[birdType].Prefab, position, Quaternion.identity, _birdTypeData[birdType].Parent);
        var bird = birdGameObject.GetComponent<Bird>();
        return bird;
    }

    #if DEBUG
    private const int _numSimulTouches = 4;
    private void Update()
    {
        var touchPassed = (Input.touchCount == _numSimulTouches && Input.GetTouch(_numSimulTouches - 1).phase == TouchPhase.Began);
        var keyboardPassed = Input.GetMouseButtonDown(1);
        if (touchPassed || keyboardPassed)
        {
            _displayBirdButtons = !_displayBirdButtons;
        }
    }

    private void OnGUI()
    {
        if (!_displayBirdButtons)
        {
            return;
        }

        var birdTypes = Enum.GetValues(typeof(BirdType)).Cast<BirdType>().OrderBy(item => item.ToString());
        
        foreach (var birdType in birdTypes)
        {
            if (birdType == BirdType.All)
            {
                continue;
            }
            //GUIStyle style = new GUIStyle("button");
            // or
            GUIStyle leftAlignedButtonStyle = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = (int)(5 + 11 * (ScreenSpace.ScreenZoom - 1)),
                
            };

            if (GUILayout.Button(birdType.ToString(), leftAlignedButtonStyle))
            {
                CreateNextBird(birdType);
            }
        }
    }
    #endif
}