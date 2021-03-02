using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenDatabase.Utilities
{
    public class ColorUtil
    {

        public static Color GetColorFromHex(string hex)
        {
            hex = hex.TrimStart('#');
            Color c = new Color(255, 0, 0);
            if (hex.Length >= 6)
            {
                c.r = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                c.g = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                c.b = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                if (hex.Length == 8)
                    c.a = int.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
            }
            return c;
        }

        public static string GetHexFromColor(Color color)
        {
            return $"#{(int)(color.r * 255):X2}" +
                $"{(int)(color.g * 255):X2}" +
                $"{(int)(color.b * 255):X2}" +
                $"{(int)(color.a * 255):X2}";
        }

    }
}
