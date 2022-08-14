using UnityEngine;
using UnityEditor;
using System.Collections;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ArrayLayout))]
public class CustPropertyDrawer : PropertyDrawer {

	public override void OnGUI(Rect position,SerializedProperty property,GUIContent label){
		EditorGUI.PrefixLabel(position,label);
		Rect newposition = position;
		newposition.y += 25f;
		SerializedProperty data = property.FindPropertyRelative("rows");
        if (data.arraySize != 9)
            data.arraySize = 9;

		for(int j=0;j < 9;j++){
			SerializedProperty row = data.GetArrayElementAtIndex(j).FindPropertyRelative("row");
			newposition.height = 25f;
			if(row.arraySize != 12)
				row.arraySize = 12;

			newposition.width = 30f;// position.width/9;
			for(int i=0;i < 12;i++){
				EditorGUI.PropertyField(newposition,row.GetArrayElementAtIndex(i),GUIContent.none);
				newposition.x += newposition.width;
			}

			newposition.x = position.x;
			newposition.y += 25f;
		}
	}

	public override float GetPropertyHeight(SerializedProperty property,GUIContent label){
		return 18f * 15;
	}
}
#endif