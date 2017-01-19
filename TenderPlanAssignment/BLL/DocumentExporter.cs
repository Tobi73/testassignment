using System;
using System.Collections.Generic;
using System.Text;
using TenderPlanAssignment.Interfaces;
using System.IO;

namespace TenderPlanAssignment.BLL
{
    /// <summary>
    /// Класс, который отвечает за экспорт документов в файл в формате .csv
    /// </summary>
    class DocumentExporter : IDocumentExporter<IEnumerable<PhoneDictionaryEntry>>
    {
        /// <summary>
        /// Итерация по всем документам коллекции для их записи в файл
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="collection"></param>
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

        /// <summary>
        /// Запись документа коллекции в файл в формате CSV
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="filewriter"></param>
        private void WriteDocumentDataInFile(PhoneDictionaryEntry entry, StreamWriter filewriter)
        {
            string newDocument = entry.FullName + ";" + entry.PhoneNumber + ";" + entry.Region;
            filewriter.WriteLine(newDocument);
        }
    }
}
