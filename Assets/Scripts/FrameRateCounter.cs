using TMPro;
using UnityEngine;

public class FrameRateCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_Text;
	[SerializeField] private UnityEngine.UI.Slider m_Slider;
    [SerializeField] private int m_FrameCount = 0;
    [SerializeField] private float m_DeltaTime = 0.0f;
    [SerializeField] private float m_FramesPerSecond = 0.0f;
    [SerializeField] private float m_UpdateRate = 4.0f; // 4 updates per sec.
	
    void Awake()
    {
		QualitySettings.vSyncCount = 0;
		SetFrameRate();
		
		if (!Debug.isDebugBuild)
		{
			Destroy(this.gameObject);
		}
    }
	
	public void SetFrameRate()
	{
		Application.targetFrameRate = (int)m_Slider.value;
	}

    void Update()
    {
        m_FrameCount++;

        m_DeltaTime += Time.deltaTime;

        if (m_DeltaTime > 1.0 / m_UpdateRate)
        {
            m_FramesPerSecond = m_FrameCount / m_DeltaTime;
            m_FrameCount = 0;
            m_DeltaTime -= 1.0f / m_UpdateRate;

            if (m_FramesPerSecond > 30)
            {
                m_Text.text = "FPS: <#FBFF00>" + m_FramesPerSecond.ToString();
            }
            else if (m_FramesPerSecond < 30 && m_FramesPerSecond > 10)
            {
                m_Text.text = "FPS: <#FFA400>" + m_FramesPerSecond.ToString();
            }
            else
            {
                m_Text.text = "FPS: <#FF4F00>" + m_FramesPerSecond.ToString();
            }

        }
    }

}