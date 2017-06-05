using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Test_UISlotBase_Assign : MonoBehaviour {
	
	public UISlotBase slot;
	public Texture texture;
	public Sprite sprite;
	
	void Start()
	{
		this.Assign();
		this.Destruct();
	}
	
	[ContextMenu("Assign")]
	public void Assign()
	{
		if (this.slot == null)
			return;
		
		if (this.texture != null)
		{
			this.slot.Assign(this.texture);
		}
		else if (this.sprite != null)
		{
			this.slot.Assign(this.sprite);
		}
	}
	
	private void Destruct()
	{
		DestroyImmediate(this);
	}
}
