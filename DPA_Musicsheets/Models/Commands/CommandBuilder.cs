using DPA_Musicsheets.IO;
using DPA_Musicsheets.Managers;

namespace DPA_Musicsheets.Models.Commands
{
    public class CommandBuilder
    {
        public Command BuildCommands(FileHandleFacade fileHandlerFacade)
        {
            Command first = new AddClefTrebleCommand(fileHandlerFacade);
            Command second = new AddTempoCommand(fileHandlerFacade);
            Command third = new AddTimeCommand(fileHandlerFacade);
            Command fourth = new OpenFileCommand(fileHandlerFacade);
            Command fifth = new SaveAsLilyPondCommand(fileHandlerFacade);
            Command sixth = new SaveAsPdfCommand(fileHandlerFacade);
            Command backStop = new Command(fileHandlerFacade);
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
