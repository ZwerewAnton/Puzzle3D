using System;
using System.Collections.Generic;
using UnityEngine;

namespace _1_LEVEL_REWORK.New.Data
{
    [CreateAssetMenu(menuName = "Level/DetailData")]
    public class DetailData : ScriptableObject
    {
        [SerializeField] private string id = Guid.NewGuid().ToString();
        [SerializeField] private int count = 1;
        [SerializeField] private GameObject prefab;
        
        public int Count => count;
        public GameObject Prefab
        {
            get => prefab;
            set => prefab = value;
        }
        public Sprite Icon
        {
            get => icon;
            set => icon = value;
        }

        [SerializeField] private Sprite icon;
        
        public string Id => id;
        public List<PointData> points = new();
    }
}