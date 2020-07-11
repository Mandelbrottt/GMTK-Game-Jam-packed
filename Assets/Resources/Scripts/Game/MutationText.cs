using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MutationText : MonoBehaviour {
	public UnityEvent onLevelStart;
	
    [Header("Mutation Text Settings")]
    public GameObject mutationDescription;
    public float durationInMiddleOfScreen;

    public Vector3 textStartPosition;
    public Vector3 textEndPosition;

    public Vector3 textStartScale;
    public Vector3 textEndScale;

    [Header("Mutation Names and Descriptions")]
    public string moveFasterTitle;
    public string moveFasterDescription;

    public string biggerSizeTitle;
    public string biggerSizeDescription;

    public string surviveLongerTitle;
    public string surviveLongerDescription;

    public string moreStartInfectedTitle;
    public string moreStartInfectedDescription;

    public string leavesAoeAfterDeathTitle;
    public string leavesAoeAfterDeathDescription;

    public string explodesAfterDeathTitle;
    public string explodesAfterDeathDescription;

    public string splitsIntoTwoUponDeathTitle;
    public string splitsIntoTwoUponDeathDescription;

    TMP_Text m_MutationTitle;
    TMP_Text m_MutationDescription;

    float m_InterpolationSpeed = 2.0f;
    float m_InterpolationParam;

    float m_MiddleOfScreenCountdown;

	private bool m_hasLevelStarted = false;

    // Start is called before the first frame update
    void Awake()
    {
        m_MutationTitle       = GetComponent<TMP_Text>();
        m_MutationDescription = mutationDescription.GetComponent<TMP_Text>();
		
		textStartPosition = new Vector3(Screen.width / 2f, Screen.height / 2f + m_MutationTitle.fontSize * 3, 0);
		textEndPosition   = new Vector3(Screen.width / 2f, Screen.height, 0);
    }

    // Update is called once per frame
    void Update()
    {
        m_MiddleOfScreenCountdown -= Time.deltaTime;

        if (m_MiddleOfScreenCountdown >= 0f)
			return;
		
		if (m_InterpolationParam < 1f)
        {
            m_InterpolationParam += m_InterpolationSpeed * Time.deltaTime;
            Mathf.Min(m_InterpolationParam, 1f);

            transform.position   = Vector3.Lerp(textStartPosition, textEndPosition, m_InterpolationParam);
            transform.localScale = Vector3.Lerp(textStartScale,    textEndScale,    m_InterpolationParam);
        }
		else if (!m_hasLevelStarted) {
			m_hasLevelStarted = true;

			onLevelStart?.Invoke();
		}
    }

    public void IntroduceMutation(InfectedMutations a_Mutation) {
		m_hasLevelStarted = false;
		
        m_MiddleOfScreenCountdown = durationInMiddleOfScreen;

        m_InterpolationParam = 0f;

        transform.position = textStartPosition;
        transform.localScale = textStartScale;

        m_MutationTitle.text = "Mutation: ";

        switch (a_Mutation)
        {
            case InfectedMutations.none:
                m_MutationTitle.text       = string.Empty;
                m_MutationDescription.text = string.Empty;
                break;

            case InfectedMutations.moveFaster:
                m_MutationTitle.text      += moveFasterTitle;
                m_MutationDescription.text = moveFasterDescription;
                break;

            case InfectedMutations.biggerSize:
                m_MutationTitle.text      += biggerSizeTitle;
                m_MutationDescription.text = biggerSizeDescription;
                break;

            case InfectedMutations.surviveLonger:
                m_MutationTitle.text      += surviveLongerTitle;
                m_MutationDescription.text = surviveLongerDescription;
                break;

            case InfectedMutations.moreStartInfected:
                m_MutationTitle.text      += moreStartInfectedTitle;
                m_MutationDescription.text = moreStartInfectedDescription;
                break;

            case InfectedMutations.leavesAoeAfterDeath:
                m_MutationTitle.text      += leavesAoeAfterDeathTitle;
                m_MutationDescription.text = leavesAoeAfterDeathDescription;
                break;

            case InfectedMutations.explodeAfterDeath:
                m_MutationTitle.text      += explodesAfterDeathTitle;
                m_MutationDescription.text = explodesAfterDeathDescription;
                break;

            case InfectedMutations.splitsIntoTwoUponDeath:
                m_MutationTitle.text      += splitsIntoTwoUponDeathTitle;
                m_MutationDescription.text = splitsIntoTwoUponDeathDescription;
                break;
        }
    }
	
	
}
