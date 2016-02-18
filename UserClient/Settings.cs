using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserClient
{
    static class Settings
    {
        public static string pdfRoot;
        public static string importFolder;
        public static string defaultPDF;

        public static void Load()
        {
            pdfRoot = "C:\\Users\\Zottel\\Documents\\GitHub\\NotenDB\\UserClient\\bin\\Debug\\PDFRoot";
            importFolder = "C:\\Users\\Zottel\\Documents\\GitHub\\NotenDB\\UserClient\\bin\\Debug\\ImportFolder";
            defaultPDF = "C:\\Users\\Zottel\\Documents\\GitHub\\NotenDB\\UserClient\\bin\\Debug\\default.pdf";
        }

        public static void Save()
        {

        }
    }
}
