using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDBase : MonoBehaviour
{
	#region SERIALIZE FIELDS
    [SerializeField] protected bool showHUD = true;
	#endregion

	#region FIELDS
	private static HUDBase instance;
	private Canvas mainCanvas;
	#endregion

	private void Awake()
	{
		CheckIsSingleton();
		InitializeHUD();

		AfterAwake();
	}

	private void Start()
	{
		BeginPlay();
	}

	private void CheckIsSingleton()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	private void InitializeHUD()
	{
		InitializeCanvas();

		if (!showHUD)
			this.enabled = false;
	}

	protected virtual void AfterAwake() { }
	protected virtual void BeginPlay() { }

	private void InitializeCanvas()
	{
		Canvas canvas = FindObjectOfType<Canvas>();
		if (canvas)
			mainCanvas = canvas;
		else
		{
			GameObject canvasObject = new GameObject("MainCanvas");
			mainCanvas = canvasObject.AddComponent<Canvas>();
			mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

			canvasObject.AddComponent<CanvasScaler>();
			canvasObject.AddComponent<GraphicRaycaster>();
		}
	}

	protected T CreateWidget<T>(T widget) where T : Object
	{
		return Instantiate<T>(widget, mainCanvas.transform);
	}
}
