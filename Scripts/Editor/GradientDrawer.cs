//Copyright(c) 2016 KirbyRawr (Jairo Baldan Fernandez)
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software./
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

//DISCLAIMER: THE CODE FOR DRAW THE GRADIENT WAS MODIFIED FROM A BASE COMING FROM THIS REPOSITORY: https://github.com/rstecca/ColorBands
//THE CODE HAVE MIT LICENSE AND IT WAS CREATED BY RIC IN 2015

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

    [CustomPropertyDrawer(typeof(Jai.Graphics.Gradient))]
    public class GradientDrawer : PropertyDrawer
    {
        Texture2D previewTexture = new Texture2D(256, 8);
        Jai.Graphics.Gradient instance;
         
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DrawGradientTexture(property);
            DrawGradientVariable(position, property, label);
        }

        void DrawGradientVariable(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            //Draw the label of the variable and set the position
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            //Set the box position
            Rect boxRect = new Rect(position.x, position.y, position.width, 16);

            //Draw a Box behind the Gradient
            GUI.Box(boxRect, "");

            //Set the gradient position
            Rect gradientRect = new Rect(boxRect.position.x + 1, boxRect.position.y + 1, boxRect.width - 2, boxRect.height - 2);

            //Draw gradient texture with a workaround method
            PropertyDrawerMethods.DrawTexture(gradientRect, previewTexture);

            //Set the Color of the Editor GUI to transparent
            GUI.color = Color.clear;
            
            //Draw the button of the Gradient Editor
            if (GUI.Button(gradientRect, ""))
            {
                GradientEditor.Init(instance);
            }

            //Revert the Color of the Editor GUI
            GUI.color = Color.white;

            EditorGUI.EndProperty();
        }

        void DrawGradientTexture(SerializedProperty property)
        {
            if(instance == null) {
              instance = PropertyDrawerMethods.GetActualObjectForSerializedProperty<Jai.Graphics.Gradient>(fieldInfo, property);
            }
      
            if (previewTexture == null)
            {
              previewTexture = new Texture2D(256, 8);
            }

            int width = previewTexture.width;
            int height = previewTexture.height;

            Color[] colors = new Color[width * height];

            Color bgColor = Color.clear;

            for (int i = 0; i < width; i++)
            {
                float t = Mathf.Clamp01(((float)i) / ((float)width));
                
                Color color = instance.Evaluate(t);

                colors[i] = color * (color.a) + bgColor * (1f - color.a);
                colors[i + width * 4] = color * (color.a) + bgColor * (1f - color.a);
            }

            for (int L = 0; L < height; L += 4)
            {
                for (int l = 0; l < 4; l++)
                {
                    if ((L % height / 2) < 4)
                        System.Array.Copy(colors, 0, colors, L * width + l * width, width);
                    else
                        System.Array.Copy(colors, width * 4, colors, L * width + l * width, width);
                }

            }

            previewTexture.SetPixels(0, 0, width, height, colors);
            previewTexture.Apply();
        }
    }
  