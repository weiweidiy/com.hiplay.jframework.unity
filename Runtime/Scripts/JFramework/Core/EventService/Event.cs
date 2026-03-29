namespace JFramework
{
    public abstract class Event
    {
        public bool Handled { get; set; }

        public object Body { get; set; }
    }


}