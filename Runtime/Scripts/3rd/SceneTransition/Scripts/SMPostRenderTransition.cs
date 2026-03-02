

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace JFramework.Unity
{
	/// <summary>
	/// base class for transitions running in the post render phase
	/// </summary>
	public abstract class SMPostRenderTransition : SMTransition
	{

		/// <summary>
		/// material used between the fade out and fade in effect
		/// </summary>
		public Material holdMaterial;

		private Camera tempCamera;
		private bool reentrantLock = false;

		protected new virtual void Awake()
		{
			if (holdMaterial == null)
			{
				Debug.LogError("'Hold' material is missing");
			}

			//tempCamera = GetComponent<Camera>();
			tempCamera = gameObject.AddComponent<Camera>();
			tempCamera.cullingMask = 0;
			tempCamera.renderingPath = RenderingPath.Forward;
			tempCamera.depth = Mathf.Floor(float.MaxValue);
			tempCamera.clearFlags = CameraClearFlags.Depth;
		}

		private void OnEnable()
		{
			RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
		}
		protected virtual void OnDisable()
		{
			RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
		}

		private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext arg1, Camera arg2)
		{
			if (arg2 == Camera.main)
			{

			}

			OnPostRender();

		}

		void OnPostRender()
		{
			// just to be sure the coroutine is started only once each frame
			if (reentrantLock)
			{
				return;
			}
			//Debug.Log("2222");
			reentrantLock = true;
			StartCoroutine(ProcessFrame());
		}

		IEnumerator ProcessFrame()
		{
			yield return new WaitForEndOfFrame();

			if (state == SMTransitionState.Hold)
			{
				OnRenderHold();
			}
			else
			{
				OnRender();
			}
			reentrantLock = false;
		}

		protected virtual void OnRenderHold()
		{
			GL.PushMatrix();
			GL.LoadOrtho();
			GL.LoadIdentity();
			for (var i = 0; i < holdMaterial.passCount; ++i)
			{
				holdMaterial.SetPass(i);
				GL.Begin(GL.QUADS);
				GL.TexCoord3(0, 0, 0);
				GL.Vertex3(0, 0, 0);
				GL.TexCoord3(0, 1, 0);
				GL.Vertex3(0, 1, 0);
				GL.TexCoord3(1, 1, 0);
				GL.Vertex3(1, 1, 0);
				GL.TexCoord3(1, 0, 0);
				GL.Vertex3(1, 0, 0);
				GL.End();
			}
			GL.PopMatrix();
		}

		/// <summary>
		/// invoked at the end of each frame
		/// </summary>
		protected abstract void OnRender();
	}
}

