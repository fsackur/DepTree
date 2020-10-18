namespace DependencyTree
{
    public interface ISatisfiable<T>
    {
        bool IsSatisfiedBy(T obj);
    }
}