using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeActiveAfterLife : MonoBehaviour 
{
    public bool CopyLocation;
    public Transform CopyTransformObject;
    public Vector3 CopyLocationOffset;
    [SerializeField]
    float LifeTime;
    float Tang;
	private void OnEnable()
	{
        Tang = 0;
	}

	private void Update()
	{
        if(Tang < LifeTime)
        {
            Tang += Time.deltaTime;

            if(CopyLocation)
            {
                if(CopyTransformObject)
                {
                    transform.position = CopyTransformObject.position + CopyLocationOffset;
                }
            }
        }
        else
        {
            CopyLocation = false;
            CopyTransformObject = null;
            this.gameObject.SetActive(false);
        }
	}
}
