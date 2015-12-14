namespace ApiScanner.Core
{
    public class Compatibility
    {
        public ChangeLevel ChangeLevel { get; private set; }

        public string Message { get; private set; }

        public Compatibility(ChangeLevel level) : this(level, string.Empty) { }

        public Compatibility(ChangeLevel level,string message)
        {
            this.ChangeLevel = level;
            this.Message = message;
        }
    }

    public enum ChangeLevel
    {
        Compatible = 1,
        NoChange = 0,
        Broken = -1,
    }
}
