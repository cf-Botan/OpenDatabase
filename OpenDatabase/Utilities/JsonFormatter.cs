using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDatabase.Utilities.Formatter
{
    public class JsonFormatter
    {

        public static string Format(string s, bool excludeZero = false)
        {
            StringBuilder sb = new StringBuilder();

            int tabCount = 0;
            bool newLine = false;
            for(int i = 0; i<s.Length; i++)
            {
                char c = s[i];

                if (newLine)
                {
                    for (int x = 0; x < tabCount; x++)
                        sb.Append("\t");
                    newLine = false;
                }


                if (c.Equals('{'))
                {
                    sb.Append(c);
                    tabCount++;
                    sb.AppendLine();
                    newLine = true;
                }
                else if (c.Equals('}'))
                {
                    sb.AppendLine();
                    newLine = true;
                    tabCount--;
                    if (newLine)
                    {
                        for (int x = 0; x < tabCount; x++)
                            sb.Append("\t");
                        newLine = false;
                    }

                    sb.Append(c);
                }
                else if (c.Equals(','))
                {
                    sb.Append(c);
                    sb.AppendLine();
                    newLine = true;
                }
                else if (c.Equals('['))
                {
                    sb.Append(c);
                    sb.AppendLine();
                    newLine = true;
                    tabCount++;
                }
                else if (c.Equals(']'))
                {
                    sb.AppendLine();
                    newLine = true;
                    tabCount--;
                    if (newLine)
                    {
                        for (int x = 0; x < tabCount; x++)
                            sb.Append("\t");
                        newLine = false;
                    }
                    sb.Append(c);

                }
                else
                {
                    sb.Append(c);
                    if (c.Equals(':'))
                        sb.Append(' ');
                }
            }

            string temp = sb.ToString();
            StringBuilder output = new StringBuilder();
            if (excludeZero)
            {
                string[] lines = temp.Replace("\r", "").Split('\n');
                for(int i = 0; i<lines.Length; i++)
                {
                    string line = lines[i];
                    if (line.Contains("\": 0")) continue;
                    output.AppendLine(line);
                }
                string _tmp = output.ToString().Replace("\r", "").Replace("\n", "").Replace(": ", ":").Replace("\t", "");
                output = new StringBuilder();
                for(int i = 0; i<_tmp.Length;i++)
                {
                    char c_current = _tmp[i];
                    char c_next = ' ';

                    if (i+1<_tmp.Length)
                    {
                        c_next = _tmp[i + 1];
                    }

                    if (c_next == '}' || c_next == ']')
                    {
                        if (c_current == ',') continue;
                    }

                    output.Append(c_current);
                }
            }

            return  (!excludeZero) ? sb.ToString() : Format(output.ToString());
        }
    }
}
