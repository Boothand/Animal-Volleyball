using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerCamera))]
public class CameraEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		PlayerCamera cam = (PlayerCamera)target;

		if (GUILayout.Button("Move to target position"))
		{
			cam.transform.position = cam.CameraPosObject.position;
			cam.transform.forward = cam.CameraPosObject.forward;
		}
	}
}