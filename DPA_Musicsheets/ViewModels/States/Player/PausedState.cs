using DPA_Musicsheets.Models.Events;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.ViewModels.States.Player
{
    public class PausedState : PlayerState
    {
        public PausedState(Sequencer sequencer) : base(sequencer)
        {
        }
        public override void GoInto(MidiPlayerViewModel owner)
        {
            owner.Running = false;
            sequencer.Stop();
            owner.UpdateButtons();
            OwnEventmanager.Manager.DispatchEvent("changeInformativeText", showText);
            OwnEventmanager.Manager.DispatchEvent("changeEditorState", "Idle");
        }
    }
}
