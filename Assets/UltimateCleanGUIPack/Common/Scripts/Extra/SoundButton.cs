// Copyright (C) 2015-2021 gamevanilla - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the Asset Store EULA is available at http://unity3d.com/company/legal/as_terms.

using UnityEngine;

namespace UltimateClean
{
    /// <summary>
	/// This component handles the logic to enable and disable the sounds
	/// and store the player selection in PlayerPrefs.
    /// </summary>
	public class SoundButton : MonoBehaviour
	{
	    private SpriteSwapper m_spriteSwapper;
	    private bool m_on;

	    private void Start()
	    {
	        m_spriteSwapper = GetComponent<SpriteSwapper>();
	        m_on = PlayerPrefs.GetInt("sound_on") == 1;
	        if (!m_on)
	            m_spriteSwapper.SwapSprite();
	    }

	    public void Toggle()
	    {
	        m_on = !m_on;
	        AudioListener.volume = m_on ? 1 : 0;
	        PlayerPrefs.SetInt("sound_on", m_on ? 1 : 0);
	    }

	    public void ToggleSprite()
	    {
	        m_on = !m_on;
	        m_spriteSwapper.SwapSprite();
	    }
	}
}
