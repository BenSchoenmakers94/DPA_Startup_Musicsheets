using DPA_Musicsheets.Models.Events;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.ViewModels.States.Player
{
    public class PlayingState : PlayerState
    {
        public PlayingState(Sequencer sequencer) : base(sequencer)
        {
            showText = "Playing...";
        }
        public override void Play(MidiPlayerViewModel owner)
        {
            sequencer.Continue();
            owner.Running = true;
            owner.UpdateButtons();
            OwnEventmanager.Manager.DispatchEvent("changeEditorState", "Playing");
            OwnEventmanager.Manager.DispatchEvent("changeInformativeText", showText);
        }

        public override void GoInto(MidiPlayerViewModel owner)
        {
            Play(owner);
        }
    }
}
