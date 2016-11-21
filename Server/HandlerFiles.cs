using System;
using System.IO;
using System.Web;

namespace Server
{
    public class HandlerFiles
    {
        string _defaultPath = System.Web.Hosting.HostingEnvironment.MapPath("~/bin/html.c");
        //   public readonly string _defaultPathLexer = Directory.GetParent(@"..\..\..\").FullName + @"\lexer.c";
        
    
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
                file = File.ReadAllText(_defaultPath);
                //  file = Regex.Replace(File.ReadAllText(_defaultPath), @"[\r\t]+", "");

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
               // File.AppendAllText(_defaultPathLexer, code);
            }
            catch (Exception e)
            {
                System.Console.Write(" No se ha encontrado el archivo");
            }
        }
    }
}