using UnityEngine;
using System.Collections;

namespace TribalUI
{
	[ExecuteInEditMode]
	public class ElementPositionForQuality : MonoBehaviour {
		
		[SerializeField] private bool m_Vertical = true;
		[SerializeField] private bool m_Horizontal = true;
		
		[SerializeField][HideInInspector] private bool m_AddedVertical = false;
		[SerializeField][HideInInspector] private bool m_AddedHorizontal = false;
		
		protected virtual void OnRectTransformDimensionsChange()
		{
			RectTransform rt = this.transform as RectTransform;
			
			if (this.m_Vertical && !this.m_AddedVertical && (rt.rect.height % 2) == 1)
			{
				rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y + 0.5f);
				this.m_AddedVertical = true;
			}
			else if (this.m_Vertical && this.m_AddedVertical && (rt.rect.height % 2) == 0)
			{
				rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y - 0.5f);
				this.m_AddedVertical = false;
			}
			
			if (this.m_Horizontal && !this.m_AddedHorizontal && (rt.rect.width % 2) == 1)
			{
				rt.anchoredPosition = new Vector2(rt.anchoredPosition.x + 0.5f, rt.anchoredPosition.y);
				this.m_AddedHorizontal = true;
			}
			else if (this.m_Horizontal && this.m_AddedHorizontal && (rt.rect.width % 2) == 0)
			{
				rt.anchoredPosition = new Vector2(rt.anchoredPosition.x - 0.5f, rt.anchoredPosition.y);
				this.m_AddedHorizontal = false;
			}
		}
	}
}