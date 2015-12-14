using System;

namespace DummyAssembly
{
    public class Parent
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Mobile { get; set; }

        public virtual string GetContactInfo()
        {
            return "Parent's Contact info.";
        }

        public void Do()
        {
            return;
        }
    }
}
