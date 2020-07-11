using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	private NPCManager m_npcManager;

	private GameObject m_player;
	
	private void Start() {
		m_npcManager = FindObjectOfType<NPCManager>();

		m_player = GameObject.Find("Player");
	}
	
	private void Update() {
		if (!m_npcManager.AreThereAnyInfectedNPCs() || m_player.gameObject == null) {
			StartCoroutine(LoadMenuAsync());
		}
	}

	private IEnumerator LoadMenuAsync() {
		yield return new WaitForSeconds(3);

		var asyncWait = SceneManager.LoadSceneAsync("Menu");

		while (!asyncWait.isDone) {
			yield return null;
		}
	}
	
}
