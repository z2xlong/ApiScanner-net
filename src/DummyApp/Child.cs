using DummyAssembly;

namespace DummyApp
{
    public class Child : Parent
    {
        public string Email { get; set; }

        public Color SkinColor { get; set; }

        public override string GetContactInfo()
        {
            return string.Format("Child's Mobile:{0},Email:{1}", this.Mobile, this.Email);
        }

        protected void DoSomething()
        {
            base.Do();
        }
    }
}
