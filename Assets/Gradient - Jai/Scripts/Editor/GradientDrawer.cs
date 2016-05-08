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
        bool gradientKeysFoldout;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CreateGradientTexture(property);
            EditorGUI.BeginProperty(position, label, property);

            //Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);   
            
            //Draw gradient texture
            GUI.DrawTexture(new Rect(position.x, position.y, position.width, 16), previewTexture);

            DrawKeys(position, property);

            EditorGUI.EndProperty();
        }

        void DrawKeys(Rect position, SerializedProperty property)
        {
            SerializedProperty keys = property.FindPropertyRelative("keys");
            Rect lastKeyRect = new Rect(position.x, position.y + 16, position.width, position.height);

            if (gradientKeysFoldout)
            {
                for (int i = 0; i < keys.arraySize; i++)
                {
                    Rect colorRect = new Rect(lastKeyRect.x, lastKeyRect.y + 18, lastKeyRect.width, position.height);
                    Rect timeRect = new Rect(lastKeyRect.x, lastKeyRect.y + 36, lastKeyRect.width, position.height);

                    EditorGUI.PropertyField(colorRect, keys.GetArrayElementAtIndex(i).FindPropertyRelative("color"));
                    EditorGUI.PropertyField(timeRect, keys.GetArrayElementAtIndex(i).FindPropertyRelative("time"));

                    lastKeyRect = timeRect;
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            if (!gradientKeysFoldout)
            {
                return base.GetPropertyHeight(prop, label);
            }
            else
            {
                return base.GetPropertyHeight(prop, label) + 16;
            }
        }

        void CreateGradientTexture(SerializedProperty property)
        {
            Jai.Graphics.Gradient instance = PropertyDrawerMethods.GetActualObjectForSerializedProperty<Jai.Graphics.Gradient>(fieldInfo, property);
      
            if (previewTexture == null)
            {
                previewTexture = new Texture2D(256, 8);
            }

            int width = previewTexture.width;
            int height = previewTexture.height;

            Color[] colors = new Color[width * height];

            Color bgColor = Color.black;

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
