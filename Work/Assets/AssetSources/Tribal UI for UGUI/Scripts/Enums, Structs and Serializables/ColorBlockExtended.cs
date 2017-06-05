using System;
using UnityEngine;

[Serializable]
public struct ColorBlockExtended
{
	//
	// Static Properties
	//
	public static ColorBlockExtended defaultColorBlock
	{
		get
		{
			return new ColorBlockExtended
			{
				m_NormalColor = new Color32 (255, 255, 255, 255),
				m_HighlightedColor = new Color32 (245, 245, 245, 255),
				m_PressedColor = new Color32 (200, 200, 200, 255),
				m_ActiveColor = new Color32 (255, 255, 255, 255),
				m_ActiveHighlightedColor = new Color32 (245, 245, 245, 255),
				m_ActivePressedColor = new Color32 (200, 200, 200, 255),
				m_DisabledColor = new Color32 (200, 200, 200, 128),
				m_ColorMultiplier = 1f,
				m_FadeDuration = 0.1f
			};
		}
	}
	
	//
	// Properties
	//
	[SerializeField] private Color m_NormalColor;
	[SerializeField] private Color m_HighlightedColor;
	[SerializeField] private Color m_PressedColor;
	[SerializeField] private Color m_ActiveColor;
	[SerializeField] private Color m_ActiveHighlightedColor;
	[SerializeField] private Color m_ActivePressedColor;
	[SerializeField] private Color m_DisabledColor;
	[Range(1f, 5f), SerializeField] private float m_ColorMultiplier;
	[SerializeField] private float m_FadeDuration;
	
	public Color normalColor {
		get {
			return this.m_NormalColor;
		}
		set {
			this.m_NormalColor = value;
		}
	}
	
	public Color highlightedColor {
		get {
			return this.m_HighlightedColor;
		}
		set {
			this.m_HighlightedColor = value;
		}
	}
	
	public Color pressedColor {
		get {
			return this.m_PressedColor;
		}
		set {
			this.m_PressedColor = value;
		}
	}
	
	public Color disabledColor {
		get {
			return this.m_DisabledColor;
		}
		set {
			this.m_DisabledColor = value;
		}
	}
	
	public Color activeColor {
		get {
			return this.m_ActiveColor;
		}
		set {
			this.m_ActiveColor = value;
		}
	}
	
	public Color activeHighlightedColor {
		get {
			return this.m_ActiveHighlightedColor;
		}
		set {
			this.m_ActiveHighlightedColor = value;
		}
	}
	
	public Color activePressedColor {
		get {
			return this.m_ActivePressedColor;
		}
		set {
			this.m_ActivePressedColor = value;
		}
	}
	
	public float colorMultiplier {
		get {
			return this.m_ColorMultiplier;
		}
		set {
			this.m_ColorMultiplier = value;
		}
	}
	
	public float fadeDuration {
		get {
			return this.m_FadeDuration;
		}
		set {
			this.m_FadeDuration = value;
		}
	}
}