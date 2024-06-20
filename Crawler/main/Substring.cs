namespace Substring
{
    class SubString
    {
        public static string SubStr(string input, char spl, int start, int end)
        {
            int count = 0;
            int index = 0;
            string sub = "";

            while (count < start && index < input.Length) {
                if (input[index] == spl) {
                    ++count;
                }
                ++index;
            }
            int startSub = index;
            while (count < end && index < input.Length) {
                if (input[index] == spl) {
                    ++count;
                }
                ++index;
            }
            if (count == end -1) {
                sub = input.Substring(startSub, index - startSub);
            } else {
                sub = input.Substring(startSub, index - startSub - 1);
            }
            // Console.WriteLine($"111{input}, {sub}");
            return sub;
        }
    }
}