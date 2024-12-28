namespace TravisRFrench.Dependencies
{
    public class Context : IContext
    {
        public IContainer Container { get; }

        public Context()
        {
            this.Container = new Container();
        }
        
        public void Initialize()
        {
            this.Setup(this.Container);
        }

        protected virtual void Setup(IContainer container)
        {
        }
    }
}
