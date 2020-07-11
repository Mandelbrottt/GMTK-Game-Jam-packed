﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public enum InfectedMutations
{
    none,
    moveFaster,
    biggerSize,
    surviveLonger,
    moreStartInfected,
    leavesAoeAfterDeath,
    explodeAfterDeath,
    splitsIntoTwoUponDeath
}

public class NPCManager : MonoBehaviour
{ 
    public List<NPC> NPCs { get; private set; }
    public InfectedMutations currentMutation = InfectedMutations.none;

    public MutationText mutationText;
	
	public UnityEvent onLastInfectedDied;

    // Start is called before the first frame update
	private void Awake() {
		var levelManager = FindObjectOfType<LevelManager>();
	}

    public void RegisterNPC(NPC a_NPC)
    {
        NPCs.Add(a_NPC);
    }

    public void UnregisterNPC(NPC a_NPC)
    {
        NPCs.Remove(a_NPC);

		if (!AreThereAnyInfectedNPCs()) {
			onLastInfectedDied?.Invoke();
		}
    }

    public bool AreThereAnyInfectedNPCs()
    {
        foreach (NPC npc in NPCs)
        {
            if (npc.isInfected && npc.isAlive)
                return true;
        }

        return false;
    }
	
	public void PreLevelLoad() {
		NPCs = new List<NPC>();

        mutationText.IntroduceMutation(currentMutation);
    }

    public void PostLevelLoad() {
        if (currentMutation == InfectedMutations.moreStartInfected)
		    SetRandomInfectedNPC(6);
        else
            SetRandomInfectedNPC();
    }

    void SetRandomInfectedNPC(int a_NumNPCsToInfect = 1)
    {
        for (int i = 0; i < a_NumNPCsToInfect; i++)
        {
            int randomNPCIndex = Random.Range(0, NPCs.Count);

            if (NPCs[randomNPCIndex].isInfected)
                i--;
            else
                NPCs[randomNPCIndex].OnInfected();
        }
    }
}
