﻿using UnityEngine;
using System.Collections;

public class ShortcutController : MonoBehaviour {

	public ShortcutSettings _ShortcutSettings;
	public ItemSettings _ItemSettings;
	public ItemHierarchy _ItemHierarchy;
	public Transform _Camera;

	private float _distanceFromMainCamera = 0.5f;

	private bool _isFirst = true;

	private bool _isAppearing = false;
	public bool IsAppearing { get { return _isAppearing; } set { _isAppearing = value; } }

	void Awake () {
		if (!CheckInspector ()) {
			print ("Check inspector factors");
			return;
		}
		_ShortcutSettings.ItemSettings = _ItemSettings;
		_distanceFromMainCamera = _ShortcutSettings.DistanceFromMainCamera;

		PutInsideOfMainCamera ();

		if (_ShortcutSettings.AutoStart) {
			Appear();
		}
	}


	/* Check all Inspector factors are valid. */
	private bool CheckInspector() {
		if (_ShortcutSettings == null ||
		    _ItemSettings == null ||
		    _ItemHierarchy == null ||
		    _Camera == null) {
			return false; 
		}
		return true;
	}


	/* Put this shortcut inside of main camera. */
	private void PutInsideOfMainCamera() {
		gameObject.transform.SetParent (_Camera.transform, false);


		Vector3 pos = new Vector3 (_ShortcutSettings.XPosition, Mathf.Lerp (-0.5f, 1.5f,_ShortcutSettings.YPosition), ComputeZPos (_ShortcutSettings.XPosition, _ShortcutSettings.YPosition));

		gameObject.transform.position = Camera.main.ViewportToWorldPoint (pos);
		gameObject.transform.localScale = Vector3.one*_distanceFromMainCamera;

	}

	private float ComputeZPos(float x, float y) {
		float d = _distanceFromMainCamera;

		return Mathf.Sqrt (Mathf.Abs((d * d) - ((x-0.5f) * (x-0.5f)) - ((y-0.5f) * (y-0.5f))));
	}


	/* Appear this shortcut. */
	public void Appear() {
		_isAppearing = true;

		if (_isFirst) {
			// item hierarchy build
			_ItemHierarchy.Build (_ShortcutSettings, gameObject);
			_isFirst = false;
		} 
		else {
			_ItemHierarchy.Appear ();
		}


	}
	
	/* DisAppear this shortcut. */
	public void Disappear() {
		_isAppearing = false;

		_ItemHierarchy.Disappear ();
	}



}
