namespace RDPToolSet.WebCore.Controllers
{
    public class ReturnResult<T>
    {
        public bool status { get; set; }

        public string message { get; set; }

        public T data { get; set; }

        public static ReturnResult<T> Success(T data, string message = "success")
        {
            return new ReturnResult<T>()
            {
                status = true,
                message = message,
                data = data
            };
        }

        public static ReturnResult<T> Fail(T data, string message = "fail")
        {
            return new ReturnResult<T>()
            {
                status = false,
                message = message,
                data = data
            };
        }
    }
}
