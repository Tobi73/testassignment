
namespace TenderPlanAssignment
{
    public interface IArgumentHandler<T>
    {
        T FormObjectFromArguments(string[] arguments);

        bool EnsureArgumentsAreValid(string[] arguments);

    }
}
