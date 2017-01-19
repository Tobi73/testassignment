using System;
using System.Collections.Generic;
using System.Linq;

namespace TenderPlanAssignment.Interfaces
{
    interface IDocumentExporter<T>
    {

        void ExportDocuments(String filepath, T data);

    }
}
