using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDatabase
{
    public class JsonFormatter
    {

        public static string Format(string s)
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
            return sb.ToString();
        }
    }
}
