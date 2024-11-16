namespace WebApplication1.Model
{
    public class Employee
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
    }
   

    public class ApiBaseResponse<T>
    {
        public ResponseContext Context { get; set; }
        public List<Employee> Response { get;  set; }
    }
    public class ResponseContext
    {
        public string TransactionId { get; set; }
        public int StatusCode { get; set; }
        public long Timestamp { get; set; }
        public string Message { get;  set; }
    }
}
