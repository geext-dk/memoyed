namespace Memoyed.DomainFramework
{
    public interface ISnapshotable<out TSnapshot>
    {
        TSnapshot CreateSnapshot();
    }
}