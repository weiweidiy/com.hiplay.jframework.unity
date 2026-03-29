
namespace JFramework
{
    public class JNetMessage : IJNetMessage
    {
        public virtual string Uid { get; set; }

        public virtual int TypeId { get; set; }

        public string Token { get; set; }
    }

    public class JNetMessageError : JNetMessage
    {
        public string ErrorMessage { get; set; }
        public override string Uid { get; set; } = "error";
        public override int TypeId { get; set; } = -1;
    }
}
