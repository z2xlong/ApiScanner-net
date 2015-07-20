namespace DummyAssembly
{
    public class Child : Parent
    {
        public string Email { get; set; }

        public override string GetContactInfo()
        {
            return string.Format("Mobile:{0},Email:{1}", this.Mobile, this.Email);
        }

        protected void DoSomething()
        {
            return;
        }
    }
}
