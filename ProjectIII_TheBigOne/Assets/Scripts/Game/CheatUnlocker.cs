namespace Tavaris.Manager
{
    internal class CheatUnlocker : LockableUnlocker
    {
        public override bool ResolveLock()
        {
            return true;
        }
        public override void Complete()
        {
            
        }
    }
}