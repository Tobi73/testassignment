using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TenderPlanAssignment.Interfaces;

namespace TenderPlanAssignment.BLL
{

    /// <summary>
    /// Класс, который отвечает за импорт документов из файлов формата .csv
    /// </summary>
    class DocumentImporter : IDocumentImporter<PhoneDictionaryEntry>
    {

        /// <summary>
        /// Построчная итерация файла
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public IEnumerable<PhoneDictionaryEntry> ImportDocuments(String filepath)
        {
            using (var sr = new StreamReader(filepath, Encoding.UTF8))
            {
                while (!sr.EndOfStream)
                {
                    yield return FormDocumentFromFile(sr);
                }
            }
        }


        /// <summary>
        /// Формирование документа из строки в файле
        /// </summary>
        /// <param name="filereader"></param>
        /// <returns></returns>
        private PhoneDictionaryEntry FormDocumentFromFile(StreamReader filereader)
        {
            string entryInString = filereader.ReadLine();
            var documentAttributes = new string[3];
            Array.Copy(entryInString.Split(';'), documentAttributes, 3);
            var newEntry = new PhoneDictionaryEntry
            {
                FullName = documentAttributes[0],
                PhoneNumber = documentAttributes[1],
            };
            if (documentAttributes[2] != null && !String.IsNullOrEmpty(documentAttributes[2]))
            {
                newEntry.Region = documentAttributes[2];
            }
            return newEntry;
        }

    }
}
