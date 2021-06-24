using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BenfordLab
{
    public class BenfordData
    {
        public int Digit { get; set; }
        public int Count { get; set; }

        public BenfordData() { }
    }

    public class Benford
    {
       
        public static BenfordData[] calculateBenford(string csvFilePath)
        {
            // load the data
            var data = File.ReadAllLines(csvFilePath)
                .Skip(1) // For header
                .Select(s => Regex.Match(s, @"^(.*?),(.*?)$"))
                .Select(data => new
                {
                    Country = data.Groups[1].Value,
                    Population = int.Parse(data.Groups[2].Value)
                });

            // manipulate the data!
            //
            // Select() with:
            //   - Country
            //   - Digit (using: FirstDigit.getFirstDigit() )
            // 
            // Then:
            //   - you need to count how many of *each digit* there are
            //
            // Lastly:
            //   - transform (select) the data so that you have a list of
            //     BenfordData objects
            //
            var benfordDataRecords = data.GroupBy(
                    populationRecord => FirstDigit.getFirstDigit(populationRecord.Population),
                    populationRecord => populationRecord,
                    (firstDigit, populationGroup) =>
                    {
                        BenfordData benfordData = new BenfordData();
                        benfordData.Digit = firstDigit;
                        benfordData.Count = populationGroup.Count();
                        return benfordData;
                    }
                )
                .OrderBy(benfordData=>benfordData.Digit)
                .ToArray();

            return benfordDataRecords;
        }
    }
}
