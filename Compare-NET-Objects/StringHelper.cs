﻿using System;
using System.Text;

namespace KellermanSoftware.CompareNetObjects
{
    /// <summary>
    /// Methods for manipulating strings
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Insert spaces into a string 
        /// </summary>
        /// <example>
        /// OrderDetails = Order Details
        /// 10Net30 = 10 Net 30
        /// FTPHost = FTP Host
        /// </example> 
        /// <param name="input"></param>
        /// <returns></returns>
        public static string InsertSpaces(string input)
        {
            bool isLastUpper = true;

            if (String.IsNullOrEmpty(input))
                return string.Empty;

            StringBuilder sb = new StringBuilder(input.Length + (input.Length / 2));

            //Replace underline with spaces
            input = input.Replace("_", " ");
            input = input.Replace("  ", " ");

            //Trim any spaces
            input = input.Trim();

            char[] chars = input.ToCharArray();

            sb.Append(chars[0]);

            for (int i = 1; i < chars.Length; i++)
            {
                bool isUpperOrNumberOrDash = (chars[i] >= 'A' && chars[i] <= 'Z') || (chars[i] >= '0' && chars[i] <= '9') || chars[i] == '-';
                bool isNextCharLower = i < chars.Length - 1 && (chars[i + 1] >= 'a' && chars[i + 1] <= 'z');
                bool isSpace = chars[i] == ' ';
                bool isLower = (chars[i] >= 'a' && chars[i] <= 'z');

                //There was a space already added
                if (isSpace)
                {
                }
                //Look for upper case characters that have lower case characters before
                //Or upper case characters where the next character is lower
                else if ((isUpperOrNumberOrDash && isLastUpper == false)
                    || (isUpperOrNumberOrDash && isNextCharLower))
                {
                    sb.Append(' ');
                    isLastUpper = true;
                }
                else if (isLower)
                {
                    isLastUpper = false;
                }

                sb.Append(chars[i]);

            }

            //Replace double spaces
            sb.Replace("  ", " ");

            return sb.ToString();
        }
    }
}
