//Copyright(c) 2016 KirbyRawr (Jairo Baldan Fernandez)
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software./
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Jai.Graphics
{
    [System.Serializable]
    public class Gradient
    {
        [System.Serializable]
        public class GradientKey
        {
            public GradientKey(Color color, float time)
            {
                this.color = color;
                this.time = time;
            }

            public Color color;
            [Range(0, 1)]
            public float time;
        }

        public List<GradientKey> keys = new List<GradientKey>() { new GradientKey(Color.white, 0) };

        public Color Evaluate(float time)
        {
            SortKeys();

            GradientKey lastKey = keys[keys.Count - 1]; 

            //If the time is over the time of the last key we return the last key color.
            if (time > lastKey.time)
            {
                return lastKey.color;
            }

            for (int i = 0; i < keys.Count - 1; i++)
            {
                GradientKey actualKey = keys[i];
                GradientKey nextKey = keys[i+1];

                if (time >= actualKey.time && time <= keys[i + 1].time)
                {    
                    return Color.Lerp(actualKey.color, nextKey.color, (time - actualKey.time) / (nextKey.time - actualKey.time));
                }
            }

            return keys[0].color;
        }
        
        public void SortKeys()
        {
            keys.Sort((p1, p2) => (p1.time.CompareTo(p2.time)));
        }
    }
}



