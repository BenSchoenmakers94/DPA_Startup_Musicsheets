using DPA_Musicsheets.Models.Events;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.ViewModels.States.Player
{
    public class StoppedState : PlayerState
    {
        public StoppedState(Sequencer sequencer) : base(sequencer)
        {
        }
        public override void Stop(MidiPlayerViewModel owner)
        {
            owner.Running = false;
            sequencer.Stop();
            sequencer.Position = 0;
            owner.UpdateButtons();
            OwnEventmanager.Manager.DispatchEvent("changeInformativeText", showText);
            OwnEventmanager.Manager.DispatchEvent("changeEditorState", "Idle");
        }

        public override void GoInto(MidiPlayerViewModel owner)
        {
            Stop(owner);
        }
    }
}
