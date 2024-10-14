using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SnipeIT_AssetTagTransporter2
{
    internal class Program
    {
        static string sourceFile;
        static string targetFile;
        static List<Asset> assetList = new List<Asset>();
        static List<string[]> csvData;

        static void Main2(string[] args)
        {
            Console.WriteLine("File with Asset Tags:");
            sourceFile = Console.ReadLine();

            Console.WriteLine("Target File (Must already have empty column with correct title):");
            targetFile = Console.ReadLine();

            // Load the CSV data into a list of string arrays
            csvData = LoadCSV(sourceFile);

            // Example asset list (replace with your actual asset list)
            List<Asset> assetList = new List<Asset>
            {
                new Asset { AssetName = "ZI-KOB-001", AssetTag = "9999" },
                // Add more assets as needed
            };

            // Update the target CSV file with asset tags
            UpdateCSVWithAssetTags(assetList);

            Console.WriteLine("CSV updated successfully.");
            Console.ReadLine();
        }

        public static List<string[]> LoadCSV(string filePath)
        {
            List<string[]> csvData = new List<string[]>();

            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    csvData.Add(fields);
                }
            }

            return csvData;
        }

        public static void UpdateCSVWithAssetTags(List<Asset> assetList)
        {
            foreach (Asset asset in assetList)
            {
                // Find the row where AssetName matches
                var rowToUpdate = csvData.FirstOrDefault(row => row.Length > 2 && row[2] == asset.AssetName); // Assuming AssetName is in column index 2

                if (rowToUpdate != null)
                {
                    // Update the AssetTag in the specified column (e.g., column index 3)
                    if (rowToUpdate.Length > 3) // Assuming AssetTag should go into column index 3
                    {
                        rowToUpdate[3] = asset.AssetTag;
                    }
                    else
                    {
                        // Handle if the row doesn't have enough columns
                        Console.WriteLine($"Error: Row doesn't have enough columns to update AssetTag for AssetName: {asset.AssetName}");
                    }
                }
                else
                {
                    // Handle if AssetName is not found in the CSV
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
    }

    // Sample Asset class (if needed for further implementation)
    public class Asset
    {
        public string AssetName { get; set; }
        public string AssetTag { get; set; }
    }
}
