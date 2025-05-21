using System.Text.Json.Serialization;

namespace APIFaunaEnriquillo.Core.DomainLayer.Utils
{
    public class Result
    {
        public bool IsSuccess { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Error? Error { get; set; }

        protected Result()
        {
            IsSuccess = true;
            Error = default;
        }

        protected Result(Error error)
        {
            IsSuccess = false;
            Error = error;
        }

        public static implicit operator Result(Error error) => new Result(error);
        public static Result Success() => new Result();
        public static Result Failure(Error error) => new Result(error);

        public class ResultT<TValue> : Result
        {
            private readonly TValue? _value;

            public ResultT(TValue tvalue) : base()
            {
                _value = tvalue;
            }

            public ResultT(Error error) : base(error)
            { 
                _value = default;
            
            }

            public TValue Value =>
                IsSuccess ? _value! : throw new InvalidOperationException("Value cannot be accessed when IsSuccess is false");

            public static implicit operator ResultT<TValue>(Error error) => 
                new(error);

            public static ResultT<TValue> Success(TValue value) =>
                new(value);

            public static ResultT<TValue> Failure(Error error) =>
                new(error);

        }
    }
}
