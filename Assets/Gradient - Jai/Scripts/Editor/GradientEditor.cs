using UnityEngine;
using System.Collections;
using UnityEditor;

public class GradientEditor : EditorWindow {

  public static SerializedProperty gradient;

  public static void Init(SerializedProperty property) {
    gradient = property;
    GetWindow<GradientEditor>();
  }
 

}
