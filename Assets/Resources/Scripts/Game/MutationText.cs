using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MutationText : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        m_MutationTitle       = GetComponent<TMP_Text>();
        m_MutationDescription = mutationDescription.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        m_MiddleOfScreenCountdown -= Time.deltaTime;

        if (m_MiddleOfScreenCountdown < 0f && m_InterpolationParam < 1f)
        {
            m_InterpolationParam += m_InterpolationSpeed * Time.deltaTime;
            Mathf.Min(m_InterpolationParam, 1f);

            transform.position   = Vector3.Lerp(textStartPosition, textEndPosition, m_InterpolationParam);
            transform.localScale = Vector3.Lerp(textStartScale,    textEndScale,    m_InterpolationParam);
        }
    }

    public void IntroduceMutation(InfectedMutations a_Mutation)
    {
        m_MiddleOfScreenCountdown = durationInMiddleOfScreen;

        m_InterpolationParam = 0f;

        transform.position   = textStartPosition;
        transform.localScale = textStartScale;

        m_MutationTitle.text = "Mutation: ";

        switch (a_Mutation)
        {
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
