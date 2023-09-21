using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace nk
{
    public class ARPlacement : MonoBehaviour
    {
        public GameObject prefabToSpawn;
        public GameObject placementIndicator;

        private GameObject spawnedPrefab;
        private Pose placementPose;

        private Camera mainCamera;
        private ARRaycastManager raycastManager;
        private bool placementPoseIsValid = false;

        private void Start()
        {
            raycastManager = GetComponent<ARRaycastManager>();
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (spawnedPrefab == null &&
                placementPoseIsValid && 
                Input.touchCount > 0 && 
                Input.GetTouch(0).phase == TouchPhase.Began)
            {
                PlaceObjectOnWorld();
            }

            UpdatePLacementPose();
            UpdatePlacementIndicator();
        }

        private void PlaceObjectOnWorld()
        {
            prefabToSpawn.SetActive(true);
            prefabToSpawn.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }

        private void UpdatePLacementPose()
        {
            var screenCenter = mainCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
            var hits = new List<ARRaycastHit>();
            raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

            placementPoseIsValid = hits.Count > 0;
            if (placementPoseIsValid)
                placementPose = hits[0].pose;
        }

        private void UpdatePlacementIndicator()
        {
            if (spawnedPrefab == null && placementPoseIsValid)
            {
                placementIndicator.SetActive(true);
                placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);

                return;
            }

            placementIndicator.SetActive(false);
        }
    }
}
