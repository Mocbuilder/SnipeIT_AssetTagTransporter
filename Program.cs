using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SnipeIT_AssetTagTransporter
{
    internal class Program
    {
        static string sourceFile;
        static string targetFile;
        static List<Asset> assetList = new List<Asset>();
        static List<string> rawOutput;
        static int[] columnIndexes = { 2, 3 }; // Indexes for Asset Name and Asset Tag
        static int formatPermIndex = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("File with Asset Tags:");
            sourceFile = Console.ReadLine();

            Console.WriteLine("Target File (Must already have empty column with correct title):");
            targetFile = Console.ReadLine();

            rawOutput = GetCSVValues(sourceFile);
            assetList = FormatOutput(rawOutput);

            List<string[]> csvData = LoadCSV(targetFile);
            UpdateCSVWithAssetTags(assetList, csvData);

            // Print the output for testing
            foreach (var item in rawOutput)
            {
                Console.WriteLine(item);
            }

            Console.ReadLine();
        }

        public static List<string> GetCSVValues(string filePath)
        {
            List<string> output = new List<string>();

            // Initialize TextFieldParser and configure
            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                // Skip the header row
                if (!parser.EndOfData)
                {
                    parser.ReadLine();
                }

                // Process each data row
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    if (fields != null && fields.Length >= 4) // Ensure there are enough columns
                    {
                        output.Add(fields[2]); // Asset Name
                        output.Add(fields[3]); // Asset Tag
                    }
                }
            }

            return output;
        }

        public static List<Asset> FormatOutput(List<string> outputList)
        {
            List<Asset> output = new List<Asset>();

            for (int i = 0; i < outputList.Count; i += 2)
            {
                if (i + 1 < outputList.Count) // Prevent index out of range
                {
                    Asset asset = new Asset(outputList[i].ToLower().Trim(), outputList[i + 1].Trim());
                    output.Add(asset);
                }
            }

            return output;
        }

        public static void UpdateCSVWithAssetTags(List<Asset> assetList, List<string[]> csvData)
        {
            foreach (Asset asset in assetList)
            {
                // Find the row where AssetName matches (assuming it's in column index 0 of the target CSV)
                var rowToUpdate = csvData.FirstOrDefault(row => row.Length > 0 && row[0].ToLower().Trim() == asset.AssetName);

                if (rowToUpdate != null)
                {
                    // Update the AssetTag in the specified column (e.g., column index 3)
                    if (rowToUpdate.Length > 3) // Assuming AssetTag should go into column index 3
                    {
                        rowToUpdate[3] = asset.AssetTag; // Update the asset tag
                        Console.WriteLine($"Updated AssetTag for '{asset.AssetName}' to '{asset.AssetTag}'.");
                    }
                    else
                    {
                        // Handle if the row doesn't have enough columns
                        Console.WriteLine($"Error: Row doesn't have enough columns to update AssetTag for AssetName: {asset.AssetName}");
                    }
                }
                else
                {
                    // Handle if AssetName is not found in the target CSV
                    Console.WriteLine($"Error: AssetName '{asset.AssetName}' not found in the CSV.");
                }
            }

            // Write the updated CSV back to the target file
            WriteCSV(targetFile, csvData);
        }

        public static void WriteCSV(string filePath, List<string[]> data)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (string[] fields in data)
                {
                    writer.WriteLine(string.Join(",", fields));
                }
            }
        }

        public static List<string[]> LoadCSV(string filePath)
        {
            List<string[]> csvData = new List<string[]>();

            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                // Read all rows
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    csvData.Add(fields);
                }
            }

            return csvData;
        }
    }
}
