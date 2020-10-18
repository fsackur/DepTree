namespace DependencyTree
{
    public interface ISatisfiable
    {
        bool IsSatisfiedBy(object obj);
    }

    public interface ISatisfiable<T>
    {
        bool IsSatisfiedBy(T obj);
    }
}