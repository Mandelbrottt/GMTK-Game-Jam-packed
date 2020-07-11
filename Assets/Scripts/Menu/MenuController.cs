using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
	// Start is called before the first frame update
	private void Start() {
	}

	public void StartGameScene() {
		StartCoroutine(LoadGameSceneAsync());
	}

	private static IEnumerator LoadGameSceneAsync() {
		var asyncLoad = SceneManager.LoadSceneAsync("Gameplay");

		while (!asyncLoad.isDone) {
			yield return null;
		}
	}
	
	public void QuitApplication() {
		Application.Quit();
	}
}
