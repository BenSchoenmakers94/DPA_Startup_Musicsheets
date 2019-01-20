namespace DPA_Musicsheets.ViewModels.Editor.Memento
{
    public class Memento
    {
        public string State { get; }

        public Memento(string state)
        {
            State = state;
        }
    }
}
