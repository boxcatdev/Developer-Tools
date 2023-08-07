using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    enum DisplayPlane { XZ, XY}

    [Header("Transforms")]
    [Tooltip("Object to be tracked by the radar.")]
    [SerializeField] Transform _objectToTrack;
    [Tooltip("The icon on the display representing the object being tracked.")]
    [SerializeField] Transform _trackingIcon;
    [Tooltip("The center of the radar")]
    [SerializeField] Transform _source;

    [Header("Radar Properties")]
    [Tooltip("The local orientation of the display.")]
    [SerializeField] private DisplayPlane _plane;
    [Tooltip("Detection range of the radar.")]
    [SerializeField] float _maxRange = 100f;
    [Tooltip("Radius of the radar's display.")]
    [SerializeField] float _displayRadius = 0.115f;

    private float _currentDist;
    
    private void Update()
    {
        if (_objectToTrack == null || _trackingIcon == null || _source == null) return;

        Vector3 objPos = new Vector3(_objectToTrack.position.x, transform.position.y, _objectToTrack.position.z);
        Vector3 heading = _source.position - objPos;
        float dist = heading.magnitude;

        if (_currentDist != dist)
        {
            //do fraction of distance
            float screenDist = _displayRadius * (dist / _maxRange);

            //get direction
            var direction = heading / dist;

            Vector3 adjDirection = Vector3.zero;

            if (_plane == DisplayPlane.XZ)
                adjDirection = new Vector3(-direction.x, 0, -direction.z);
            else if (_plane == DisplayPlane.XY)
                adjDirection = new Vector3(-direction.x, -direction.z, 0);


            //move dot by screenDist in correct direction
            if (dist <= _maxRange)
                _trackingIcon.localPosition = screenDist * adjDirection;
            else
                _trackingIcon.localPosition = _displayRadius * adjDirection;

            _currentDist = dist;

            //rotate display to match ship orientation
            if(_source != null)
            {
                if(_plane == DisplayPlane.XZ)
                {
                    transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x,
                        transform.localRotation.eulerAngles.y, _source.transform.rotation.eulerAngles.y);
                }
                else
                {
                    _source.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, 
                        _source.localRotation.eulerAngles.y, transform.rotation.eulerAngles.y);
                }
            }
        }
    }
}
