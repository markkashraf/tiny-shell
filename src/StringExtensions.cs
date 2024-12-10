namespace StringExtensions
{
    public static class StringExtensions
    {
        public static string Concatinate(this string[] str, int start, int end)
        {
            string result = "";
            for (int i = start; i < end; i++)
            {
                result += str[i];
                if (i != end - 1)
                    result += " ";
            }

            return result;
        }

        public static string[] SplitParam(this string str)
        {
            List<string> ans = new List<string>();
            bool literal = false;
            string temp = "";
            foreach (var c in str)
            {
                if (!literal)
                {
                    if (c == '\'') literal = true;
                    else if (c != ' ') temp += c;
                    else { ans.Add(temp); temp = ""; }
                }
                else
                {

                    if (c != '\'') temp += c;
                    else { ans.Add(temp); literal = false; temp = ""; }
                }
            }
            if (temp is not "") ans.Add(temp);
            return ans.ToArray();
        }

    }

}