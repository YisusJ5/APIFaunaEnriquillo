using APIFaunaEnriquillo.Core.DomainLayer.Enums;

namespace APIFaunaEnriquillo.Core.DomainLayer.Utils
{
    public class Error
    {
        public Error(string code, string description, ErrorType errorType)
        {
           Code = code;
           Description = description;
           ErrorType = errorType;
        }

        public string Code { get; set; }
        public string Description { get; set; }
        public ErrorType ErrorType { get; set; }

        public static Error Failure(string code, string description) => new Error(code, description, ErrorType.Failure);
        public static Error NotFound(string code, string description) => new Error (code, description, ErrorType.NotFound);

    }
}
