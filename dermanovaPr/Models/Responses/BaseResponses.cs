namespace dermanovaPr.Models.Responses
{
    public class BaseResponses
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public bool IsSuccess { get; set; } = false;

        public int NewClienteId { get; set; }
        public int NewDiagnosticoId {  get; set; }
    }
}
