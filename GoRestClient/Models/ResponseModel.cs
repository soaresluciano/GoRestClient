namespace GoRestClient.Models
{
    public class ResponseModel<TMeta, TData>
    {
        public uint Code { get; set; }
        public TMeta Meta { get; set; }
        public TData Data { get; set; }
    }

    public class ResponseModel<TData> : ResponseModel<dynamic, TData>{ }
}
