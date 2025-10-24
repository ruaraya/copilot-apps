namespace Minimizer.Common.Models.Exceptions
{
    public class ValidationError
    {
        public string Name { get; }

        public string Reason { get; }

        public ValidationError(string name, string reason)
        {
            Name = name;
            Reason = reason;
        }
    }
}