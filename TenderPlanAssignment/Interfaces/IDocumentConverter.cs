
namespace TenderPlanAssignment.Interfaces
{
    interface IDocumentConverter<T, K>
    {

        T ConvertDocument(K document);

    }
}
