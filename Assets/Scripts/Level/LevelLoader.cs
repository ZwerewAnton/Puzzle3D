using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace Level
{
    public class LevelLoader : MonoBehaviour
    {
        public Detail ground;
    
        private bool _isInstantiate;
        private readonly List<Detail> _allDetails;
        private List<Detail> _availableDetails;
        private List<Detail> _installedDetails;
        private List<PointParentConnector> _allPoints;
        private Detail _instDetail;

        public LevelLoader(List<Detail> allDetails, List<Detail> availableDetails)
        {
            _allDetails = allDetails;
            _availableDetails = availableDetails;
        }

        private void Awake()
        {
            _installedDetails = new List<Detail>();
            _availableDetails = new List<Detail>();
            StartDetailInstance();
        }
    
        private List<Detail> GetOpenDetails(Detail detail)
        {
            var list = new List<Detail>();
            foreach (var det in _allDetails)
            {
                foreach (var pointParConn in det.points)
                {
                    if (pointParConn.IsInstalled) 
                        continue;
                    
                    foreach (var parentDet in pointParConn.parentList)
                    {
                        if (parentDet.parentDetail == detail)
                        {
                            list.Add(det);
                        }
                    }
                }
            }
            return list;
        }

        private void StartDetailInstance()
        {
            _installedDetails.Add(ground);
            _availableDetails.AddRange(GetOpenDetails(ground));

            foreach (var detail in _allDetails)
            {
                foreach (var pPC in detail.points)
                {
                    var isDetailInstalled = false;
                    if (!pPC.IsInstalled)
                        continue;
                    
                    var instObject = detail.prefab.transform.GetChild(0).gameObject;
                    Instantiate(instObject, pPC.point.position, Quaternion.Euler(pPC.point.rotation));
                    foreach (var installedDetail in _installedDetails)
                    {
                        if (detail != installedDetail) 
                            continue;
                        
                        isDetailInstalled = true;
                        break;
                    }
                    if (isDetailInstalled) 
                        continue;
                    
                    _installedDetails.Add(detail);
                    _availableDetails.AddRange(GetOpenDetails(detail));
                }
            }
        }
    }
}