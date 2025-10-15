using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SysProgDrugiDeo19461.FileManager
{
    public static class FileManager
    {
        public static string FindFile(string root, string fileName)
        {
            if (string.IsNullOrWhiteSpace(root) || string.IsNullOrWhiteSpace(fileName))
                return "";

            foreach (var file in Directory.GetFiles(root, "*", SearchOption.AllDirectories))
            {
                if (Path.GetFileName(file) == fileName)
                    return file;
            }

            return "";
        }

        public static int FindAverageWordLength(string file)
        {
            int avg = 0, count = 0;
            List<string> reci = Regex.Matches(File.ReadAllText(file), @"[a-zA-Z]+").Cast<Match>().Select(x => x.Value).ToList();

            foreach (var rec in reci)
            {
                int length = rec.Where(char.IsLetter).Count();
                avg += length;
                count++;
            }

            if (count != 0)
                avg /= count;

            return avg;
        }
    }
}
