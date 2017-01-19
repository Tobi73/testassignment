using System;
using System.Collections.Generic;
using System.Text;
using TenderPlanAssignment.Interfaces;
using System.IO;

namespace TenderPlanAssignment.BLL
{
    class DocumentExporter : IDocumentExporter<IEnumerable<PhoneDictionaryEntry>>
    {
        public void ExportDocuments(String filepath, IEnumerable<PhoneDictionaryEntry> collection)
        {
            using (var sw = new StreamWriter(filepath, true, Encoding.UTF8))
            {
                foreach (var entry in collection)
                {
                    WriteDocumentDataInFile(entry, sw);
                }
            }
        }

        private void WriteDocumentDataInFile(PhoneDictionaryEntry entry, StreamWriter filewriter)
        {
            string newDocument = entry.FullName + ";" + entry.PhoneNumber + ";" + entry.Region;
            filewriter.WriteLine(newDocument);
        }
    }
}
