

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JFramework.Unity
{

	/// <summary>
	/// A transition that just loads the new level without any visual effect.
	/// </summary>
	[AddComponentMenu("Scripts/SceneManager/Plain Transition")]
	public class SMPlainTransition : SMTransition
	{

		protected override IEnumerator DoTransitionOut(Action complete)
		{
			yield return 0; // to make the compiler happy
			complete?.Invoke();
		}

		protected override bool Process(float elapsedTime)
		{
			return false;
		}
	}
}