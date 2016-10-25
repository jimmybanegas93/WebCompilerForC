﻿using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleTest
{
    class HandlerFiles
    {
        public readonly string _defaultPath = Directory.GetParent(@"..\..\..\").FullName + @"\code.c";

        
        public HandlerFiles()
        {

        }

        public HandlerFiles(string path)
        {
            _defaultPath = path;
        }

        public string GetCode()
        {
            string file;
            try
            {
             //   file = System.IO.File.ReadAllText(defaultPath);
                 file = Regex.Replace(File.ReadAllText(_defaultPath), @"[\r\t]+", "");

            }
            catch (Exception e)

            {
                System.Console.Write(" No se ha encontrado el archivo");
                return "";
            }
            return file;

        }

        public void WriteCode(string code)
        {
            try
            {
                File.WriteAllText("C:\\Users\\Jimmy Ramos\\Documents\\code.c", code);
            }
            catch (Exception e)
            {
                System.Console.Write(" No se ha encontrado el archivo");
            }
        }
    }
}