using DPA_Musicsheets.Managers;

namespace DPA_Musicsheets.Models.Commands
{
    public class CommandBuilder
    {
        public Command BuildCommands(MusicLoader musicLoader)
        {
            Command first = new AddBarLinesCommand(null, musicLoader);
            Command second = new AddClefTrebleCommand(null, musicLoader);
            Command third = new AddTempoCommand(null, musicLoader);
            Command fourth = new AddTimeCommand(null, musicLoader);
            Command fifth = new OpenFileCommand(null, musicLoader);
            Command sixth = new SaveAsLilyPondCommand(null, musicLoader);
            Command seventh = new SaveAsPdfCommand(null, musicLoader);
            Command backStop = new Command(null, musicLoader);
            first.Next = second;
            second.Next = third;
            third.Next = fourth;
            fourth.Next = fifth;
            fifth.Next = sixth;
            sixth.Next = seventh;
            seventh.Next = backStop;
            return first;
        }
    }
}
