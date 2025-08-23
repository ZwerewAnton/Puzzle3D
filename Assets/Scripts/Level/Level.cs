using System;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.Serialization;

namespace Level
{
    [Serializable]
    public class Level
    {
        [FormerlySerializedAs("_name")] [SerializeField] private string name;
        [FormerlySerializedAs("_icon")] [SerializeField] private Sprite icon;
        [FormerlySerializedAs("_ground")] [SerializeField] private Detail ground;
        [FormerlySerializedAs("_details")] [SerializeField] private List<Detail> details = new();

        public List<Detail> Details => details;
        public Detail Ground => ground;
        public Sprite Icon => icon;
        public string Name => name;

        public void Reset()
        {
            foreach (var detail in details)
            {
                detail.Reset();
            }
        }

        public float GetPercent()
        {
            float allDetails = 0;
            float installedDetails = 0;
            foreach (var detail in details)
            {
                foreach (var pPC in detail.GetPoints)
                {
                    if (pPC.IsInstalled)
                    {
                        installedDetails++;
                    }
                    allDetails++;
                }
            }
            return Mathf.Round((installedDetails/allDetails) * 100);
        }
    }
}