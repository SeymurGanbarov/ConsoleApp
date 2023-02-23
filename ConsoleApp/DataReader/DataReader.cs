using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConsoleApp.Models;

namespace ConsoleApp
{

    public class DataReader :IDataReader
    {
        List<ImportedObject> ImportedObjects;
        private const string Database = "DATABASE";

        public void ImportAndPrintData(string fileToImport, bool printData = true)
        {
            ImportedObjects = new List<ImportedObject>();

            ReadDataFromFile(fileToImport);
            
            AssignNumberOfChildren();

            if (printData)
                PrintData();

        }

        private void ReadDataFromFile(string file)
        {
            var streamReader = new StreamReader(file);

            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                // check line is null,empty or whitespace
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var values = line.Split(';');
                    var importedObject = new ImportedObject();

                    //check values array has specified index
                    //check importedObject properties is null,empty or whitespace
                    if (values.ElementAtOrDefault(0) != null && !string.IsNullOrWhiteSpace(values[0]))
                        importedObject.Type = values[0].Trim().Replace(" ", "").Replace(Environment.NewLine, "").ToUpper();
                    if (values.ElementAtOrDefault(1) != null && !string.IsNullOrWhiteSpace(values[1]))
                        importedObject.Name = values[1].Trim().Replace(" ", "").Replace(Environment.NewLine, "");
                    if (values.ElementAtOrDefault(2) != null && !string.IsNullOrWhiteSpace(values[2]))
                        importedObject.Schema = values[2].Trim().Replace(" ", "").Replace(Environment.NewLine, "");
                    if (values.ElementAtOrDefault(3) != null && !string.IsNullOrWhiteSpace(values[3]))
                        importedObject.ParentName = values[3].Trim().Replace(" ", "").Replace(Environment.NewLine, "");
                    if (values.ElementAtOrDefault(4) != null && !string.IsNullOrWhiteSpace(values[4]))
                        importedObject.ParentType = values[4].Trim().Replace(" ", "").Replace(Environment.NewLine, "");
                    if (values.ElementAtOrDefault(5) != null && !string.IsNullOrWhiteSpace(values[5]))
                        importedObject.DataType = values[5].Trim().Replace(" ", "").Replace(Environment.NewLine, "");
                    if (values.ElementAtOrDefault(6) != null && !string.IsNullOrWhiteSpace(values[6]))
                        importedObject.IsNullable = values[6].Trim().Replace(" ", "").Replace(Environment.NewLine, "");

                    ImportedObjects.Add(importedObject);
                }
            }
        }

        private void AssignNumberOfChildren()
        {
            //assign number of children
            for (int i = 0; i < ImportedObjects.Count(); i++)
            {
                var importedObject = ImportedObjects[i];
                foreach (var impObj in ImportedObjects)
                {
                    if (impObj.ParentType == importedObject.Type)
                    {
                        if (impObj.ParentName == importedObject.Name)
                        {
                            importedObject.NumberOfChildren++;
                        }
                    }
                }
            }
        }

        private void PrintData()
        {
            foreach (var database in ImportedObjects)
            {
                if (database.Type == Database)
                {
                    Console.WriteLine($"Database '{database.Name}' ({database.NumberOfChildren} tables)");

                    //check ParentType is null,empty or whitespace
                    // print all database's tables and 
                    foreach (var table in ImportedObjects)
                    {
                        if (!string.IsNullOrWhiteSpace(table.ParentType) && table.ParentType.ToUpper() == database.Type)
                        {
                            if (table.ParentName == database.Name)
                            {
                                Console.WriteLine($"\tTable '{table.Schema}.{table.Name}' ({table.NumberOfChildren} columns)");
                                // print all table's columns
                                foreach (var column in ImportedObjects)
                                {
                                    if (!string.IsNullOrWhiteSpace(column.ParentType) && column.ParentType.ToUpper() == table.Type)
                                    {
                                        if (column.ParentName == table.Name)
                                        {
                                            Console.WriteLine($"\t\tColumn '{column.Name}' with {column.DataType} data type {(column.IsNullable == "1" ? "accepts nulls" : "with no nulls")}");
                                        }
                                    }
                                }
                            }
                        }

                    }
                }

            }
        }
    }

}
