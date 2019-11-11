using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class Test_ModelManipulation {

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	// Unity specific unit test will be implemented when Travis can build 
	// the project.
	[UnityTest]
	public IEnumerator Test_ModelDeformation() {
		yield return null;
	}
}
