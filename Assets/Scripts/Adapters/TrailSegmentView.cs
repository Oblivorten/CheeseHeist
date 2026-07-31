using System.Collections.Generic;
using UnityEngine;
using CheeseHeist.Core;

namespace CheeseHeist.Adapters
{
    public class TrailSegmentView : MonoBehaviour
    {
        [SerializeField] private GameObject _segmentPrefab;
        [SerializeField] private Transform _poolParent;

        private Transform[] _pool;

        public void Initialize(int capacity)
        {
            _pool = new Transform[capacity];
            for (int i = 0; i < capacity; i++)
            {
                var instance = Instantiate(_segmentPrefab, _poolParent);
                instance.SetActive(false);
                _pool[i] = instance.transform;
            }
        }

        public void Sync(IReadOnlyList<TrailSegmentData> segments)
        {
            for (int i = 0; i < segments.Count; i++)
            {
                var data = segments[i];
                var view = _pool[i];

                if (data.IsActive)
                {
                    if (!view.gameObject.activeSelf)
                    {
                        view.gameObject.SetActive(true);
                    }
                    view.position = new Vector3(data.Position.X, data.Position.Y, data.Position.Z);
                }
                else if (view.gameObject.activeSelf)
                {
                    view.gameObject.SetActive(false);
                }
            }
        }
    }
}