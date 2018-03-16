namespace Hotfix.framework
{

    public interface IReference
    {
        bool IsFromPool { get; set; }

        void Dispose();
    }

    public class ReferenceDisposer : IReference
    {
        public bool IsFromPool { get; set; }

        protected ReferenceDisposer()
        {
        }

        public virtual void Dispose()
        {
            if (this.IsFromPool)
            {
                ReferencePool.Recycle(this);
            }
        }
    }
}
