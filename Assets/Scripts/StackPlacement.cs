
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.EventSystems;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class StackPlacement : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public GameObject stackRoot;

    private bool isPlaced = false;

    private static readonly List<ARRaycastHit> hits =
        new List<ARRaycastHit>();

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
        if (isPlaced || raycastManager == null ||
            stackRoot == null)
            return;

        if (Touch.activeTouches.Count == 0)
            return;

        var touch = Touch.activeTouches[0];

        if (touch.phase !=
            UnityEngine.InputSystem.TouchPhase.Began)
            return;

        // Do not place Stack when touching UI buttons.
        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject(
                touch.touchId))
            return;

        if (raycastManager.Raycast(
            touch.screenPosition,
            hits,
            TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;

            stackRoot.transform.SetPositionAndRotation(
                pose.position,
                Quaternion.identity
            );

            stackRoot.SetActive(true);
            isPlaced = true;

            Debug.Log("Stack placed on AR surface!");
        }
    }
}
