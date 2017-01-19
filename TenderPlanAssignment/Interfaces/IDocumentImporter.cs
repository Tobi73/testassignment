using System;
using System.Collections.Generic;

namespace TenderPlanAssignment.Interfaces
{
    public interface IDocumentImporter<T>
    {

        IEnumerable<T> ImportDocuments(String filepath);

    }
}
