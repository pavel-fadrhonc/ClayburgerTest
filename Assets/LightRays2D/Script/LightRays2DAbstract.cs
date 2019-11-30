using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class LightRays2DAbstract:MonoBehaviour{

	private Material mat;

	public Color color1=Color.white;
	private Color _color1;

	public Color color2=new Color(0,0.46f,1,0);
	private Color _color2;

	[Range(0f,5f)]
	public float speed=0.5f;
	private float _speed;

	[Range(1f,30f)]
	public float size=15f;
	private float _size;

	[Range(-1f,1f)]
	public float skew=0.5f;
	private float _skew;

	[Range(0f,5f)]
	public float shear=1f;
	private float _shear;

	[Range(0f,1f)]
	public float fade=1f;
	private float _fade;

	[Range(0f,50f)]
	public float contrast=1f;
	private float _contrast;

	[Range(-1f,1f)]
	public float xFadeMin = 0.0f;
	private float _xFadeMin;
	
	[Range(-1f,1f)]
	public float xFadeMax = 1.0f;
	private float _xFadeMax;

	[Range(-1f,1f)]
	public float yFadeMin = 0.0f;
	private float _yFadeMin;
	
	[Range(-1f,1f)]
	public float yFadeMax = 1.0f;
	private float _yFadeMax;
	

	private void Awake(){
		InitializeReferences();
		_color1=color1=mat.GetColor("_Color1");
		_color2=color2=mat.GetColor("_Color2");
		_speed=speed=mat.GetFloat("_Speed");
		_size=size=mat.GetFloat("_Size");
		_skew=skew=mat.GetFloat("_Skew");
		_shear=shear=mat.GetFloat("_Shear");
		_fade=fade=mat.GetFloat("_Fade");
		_contrast=contrast=mat.GetFloat("_Contrast");
		_xFadeMin = xFadeMin = mat.GetVector("_FadeBorders").x;
		_xFadeMax = xFadeMax = mat.GetVector("_FadeBorders").y;
		_yFadeMin = yFadeMin = mat.GetVector("_FadeBorders").z;
		_yFadeMax = yFadeMax = mat.GetVector("_FadeBorders").w;
	}

	protected virtual void Update(){
		GetReferences();
		if(AnythingChanged()){
			mat.SetColor("_Color1",color1);
			mat.SetColor("_Color2",color2);
			mat.SetFloat("_Speed",_speed);
			mat.SetFloat("_Size",_size);
			mat.SetFloat("_Skew",_skew);
			mat.SetFloat("_Shear",_shear);
			mat.SetFloat("_Fade",_fade);
			mat.SetFloat("_Contrast",_contrast);
			mat.SetVector("_FadeBorders", new Vector4(_xFadeMin, _xFadeMax, _yFadeMin, _yFadeMax));
		}
	}

	private void InitializeReferences(){
		if(mat==null){
			GetReferences();
			mat=GetMaterial();
		}
	}

	protected abstract void GetReferences();
	
	protected abstract Material GetMaterial();
	
	protected abstract void ApplyMaterial(Material material);

	bool AnythingChanged(){
		bool changed=false;
		if(_color1!=color1){
			_color1=color1;
			changed=true;
		}
		if(_color2!=color2){
			_color2=color2;
			changed=true;
		}
		if(_speed!=speed){
			_speed=speed;
			changed=true;
		}
		if(_size!=size){
			_size=size;
			changed=true;
		}
		if(_skew!=skew){
			_skew=skew;
			changed=true;
		}
		if(_shear!=shear){
			_shear=shear;
			changed=true;
		}
		if(_fade!=fade){
			_fade=fade;
			changed=true;
		}
		if(_contrast!=contrast){
			_contrast=contrast;
			changed=true;
		}
		if (_xFadeMin != xFadeMin)
		{
			_xFadeMin = xFadeMin;
			changed = true;
		}
		if (_xFadeMax != xFadeMax)
		{
			_xFadeMax = xFadeMax;
			changed = true;
		}	
		if (_yFadeMin != yFadeMin)
		{
			_yFadeMin = yFadeMin;
			changed = true;
		}
		if (_yFadeMax != yFadeMax)
		{
			_yFadeMax = yFadeMax;
			changed = true;
		}			
		return changed;
	}
}