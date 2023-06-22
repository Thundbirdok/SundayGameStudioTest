using UnityEngine;

namespace Ui
{
    using System;
    using System.Collections.Generic;
    using UnityEngine.UI;
    using Object = UnityEngine.Object;

    [Serializable]
    public class ImageGrid
    {
        public event Action<Sprite> OnImageSelected;
        
        [SerializeField]
        private string url = "http://data.ikppbb.com/test-task-unity-data/pics/";
        
        [SerializeField]
        private ImageHandler objectPrefab;

        [SerializeField]
        private ScrollRect scrollRect;

        [SerializeField]
        private GridLayoutGroup grid;

        [SerializeField]
        private RectTransform box;
        
        [SerializeField]
        public int startNumber = 1;

        [SerializeField]
        public int endNumber = 66;

        private readonly List<ImageHandler> _spawnedObjects = new List<ImageHandler>();

        private int _loadUpTo;

        private int LoadUpTo
        {
            get
            {
                return _loadUpTo;
            }

            set
            {
                var valueClamped = Mathf.Clamp(value, startNumber, endNumber);
                
                if (valueClamped % 2 == 0)
                {
                    _loadUpTo = valueClamped;
                    
                    return;
                }
                
                _loadUpTo = valueClamped + 1;
            }
        }

        private int _objectCountFitInScreen;
        
        public void Initialize()
        {
            scrollRect.onValueChanged.AddListener(LoadNewObjects);
            
            _objectCountFitInScreen = GetObjectCountFitInScreen();

            LoadUpTo += (int)(_objectCountFitInScreen * 1.5f);

            SpawnObjects(startNumber);
        }

        public void Deinitialize()
        {
            ClearSpawnedObjects();

            scrollRect.onValueChanged.RemoveListener(LoadNewObjects);
        }

        private void ClearSpawnedObjects()
        {
            for (var i = 0; i < _spawnedObjects.Count; i++)
            {
                _spawnedObjects[i].OnImageSelected -= ToView;
                Object.Destroy(_spawnedObjects[i].gameObject);
                _spawnedObjects.RemoveAt(i);
            }
        }

        private int GetObjectCountFitInScreen()
        {
            var lastVisibleIndex =
                startNumber
                + grid.constraintCount
                - 1
                + Mathf.CeilToInt
                (
                    box.rect.height
                    / (grid.cellSize.y + grid.padding.top + grid.padding.bottom)
                    * grid.constraintCount
                );

            return Mathf.Clamp(lastVisibleIndex, startNumber, endNumber);
        }

        private void LoadNewObjects(Vector2 normalizedPosition)
        {
            if (normalizedPosition.y > 0.1f)
            {
                return;
            }

            if (LoadUpTo == endNumber)
            {
                return;
            }

            var from = LoadUpTo + 1;
            LoadUpTo += _objectCountFitInScreen / 2;

            SpawnObjects(from);
            
            scrollRect.verticalScrollbar.interactable = true;
        }

        private void SpawnObjects(int from)
        {
            for (var i = from; i <= LoadUpTo; i++)
            {
                SpawnObject(i);
            }
        }

        private void SpawnObject(int number)
        {
            var spawnedObject = Object.Instantiate(objectPrefab, grid.transform);

            spawnedObject.Initialize(url, number);

            spawnedObject.OnImageSelected += ToView;
            _spawnedObjects.Add(spawnedObject);
        }

        private void ToView(Sprite sprite) => OnImageSelected?.Invoke(sprite);
    }
}
