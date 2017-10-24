using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions; //Match

namespace Bokföring
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Skriv sokvagen till SIE-fIlen, ar du snall");
            var file = Console.ReadLine(); // C:\Users\J3ss!c4\Desktop\SIE.txt
            string pattern = @"#TRANS (\d{4}) {} (-?\d*.\d*)";

            var total = 0;
            var fileContent = File.ReadAllText(file);
            var reader = new StringReader(fileContent);
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                    break;

                if (Regex.Match(line, pattern).Success)
                {
                    total++;
                }
            }

            /*Summering av konto TRANS*/
            var TransAccounts = new Dictionary<string, decimal>();
            var streamReader = File.OpenText(file);
            while (true)
            {
                var line = streamReader.ReadLine();
                if (line == null)
                    break;
                var match = Regex.Match(line, pattern);
                if (match.Success)
                {
                    var accountId = match.Groups[1].Value;
                    var amount = decimal.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    
                    if (TransAccounts.ContainsKey(accountId))
                    {
                        TransAccounts[accountId] += amount;
                    }                        
                    else
                        TransAccounts[accountId] = amount;
                }
            }
            Console.WriteLine();
            Console.WriteLine($"Antal #TRANS konton: {total}");
            Console.WriteLine();
            foreach (var entry in TransAccounts.OrderBy(e => e.Key))
            Console.WriteLine($"{entry.Key} {entry.Value.ToString("F2")}");


            Console.WriteLine();
            Console.WriteLine("Summan av alla #TRANS konto");
            Console.WriteLine(TransAccounts.Sum(e => e.Value));

            Console.ReadLine();
        }
    }
}