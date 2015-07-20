namespace DummyAssembly
{
    public class Parent
    {
        protected int Id { get; set; }

        public string Name { get; set; }

        public string Mobile { get; set; }

        public virtual string GetContactInfo()
        {
            return this.Mobile;
        }
    }
}
