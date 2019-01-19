using DPA_Musicsheets.Managers;

namespace DPA_Musicsheets.Models.Commands
{
    public class CommandBuilder
    {
        public Command BuildCommands(MusicLoader musicLoader)
        {
            Command first = new AddClefTrebleCommand(musicLoader);
            Command second = new AddTempoCommand(musicLoader);
            Command third = new AddTimeCommand(musicLoader);
            Command fourth = new OpenFileCommand(musicLoader);
            Command fifth = new SaveAsLilyPondCommand(musicLoader);
            Command sixth = new SaveAsPdfCommand(musicLoader);
            Command backStop = new Command(musicLoader);
            first.Next = second;
            second.Next = third;
            third.Next = fourth;
            fourth.Next = fifth;
            fifth.Next = sixth;
            sixth.Next = backStop;
            return first;
        }
    }
}
