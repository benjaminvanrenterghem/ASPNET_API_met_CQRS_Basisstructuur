
namespace Domain.Model.Messaging {
    public class SuccessResponse : Response<bool> {
        public new List<bool> Content => new List<bool>() { true };
        public new bool Success => true;
        public new List<Message> Messages { get; set; } = new();
    }

    public class FailureResponse : Response<bool> {
        public new List<bool> Content => new List<bool>() { false };
        public new bool Success => false;
        public FailureResponse(Exception e) {
            base.AddError(e);
		}
    }
}
