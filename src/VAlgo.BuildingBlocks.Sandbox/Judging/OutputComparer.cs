using System.Text;

namespace VAlgo.BuildingBlocks.Sandbox.Judging
{
    public static class OutputComparer
    {
        public static bool Equals(string actual, string expected)
        {
            var normActual = Normalize(actual);
            var normExpected = Normalize(expected);

            return normActual == normExpected;
        }

        private static string Normalize(string output)
        {
            if (string.IsNullOrWhiteSpace(output))
                return string.Empty;

            var lines = output
                .Replace("\r\n", "\n")
                .Replace('\r', '\n')
                .Split('\n');

            var sb = new StringBuilder();

            foreach (var line in lines)
            {
                // Trim end: bỏ spaces / tabs dư cuối dòng
                sb.Append(line.TrimEnd());
                sb.Append('\n');
            }

            // Bỏ newline dư cuối output
            while (sb.Length > 0 && sb[^1] == '\n')
                sb.Length--;

            return sb.ToString();
        }
    }
}