using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class VRInputModule : BaseInputModule
{
    public Camera _camera;
    public SteamVR_Input_Sources _targetSource;
    public SteamVR_Action_Boolean _clickAction;

    private GameObject _currentObject = null;
    private PointerEventData _data = null;

    protected override void Awake()
    {
        base.Awake();

        _data = new PointerEventData(eventSystem);
    }

    public override void Process()
    {
        _data.Reset();
        _data.position = new Vector2(_camera.pixelWidth / 2, _camera.pixelHeight / 2);

        eventSystem.RaycastAll(_data, m_RaycastResultCache);
        _data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        _currentObject = _data.pointerCurrentRaycast.gameObject;

        m_RaycastResultCache.Clear();

        HandlePointerExitAndEnter(_data, _currentObject);

        //Press
        if (_clickAction.GetStateDown(_targetSource))
        {
            ProcessPress(_data);
        }

        //Release
        if (_clickAction.GetStateUp(_targetSource))
        {
            ProcessRelease(_data);
        }
    }

    public PointerEventData GetData()
    {
        return _data;
    }

    private void ProcessPress(PointerEventData data)
    {
        data.pointerPressRaycast = data.pointerCurrentRaycast;

        GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(_currentObject, data, ExecuteEvents.pointerDownHandler);
        if (newPointerPress == null)
        {
            newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(_currentObject);
        }

        data.pressPosition = data.position;
        data.pointerPress = newPointerPress;
        data.rawPointerPress = _currentObject;
    }

    private void ProcessRelease(PointerEventData data)
    {
        ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);

        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(_currentObject);
        if (_data.pointerPress == pointerUpHandler)
        {
            ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerClickHandler);
        }

        eventSystem.SetSelectedGameObject(null);

        data.pressPosition = Vector2.zero;
        data.pointerPress = null;
        data.rawPointerPress = null;
    }
}
