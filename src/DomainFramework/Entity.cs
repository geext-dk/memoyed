namespace Memoyed.DomainFramework
{
    public abstract class Entity
    {
        // For db
        private int _dbId;
        private int DbId => _dbId;
    }
}