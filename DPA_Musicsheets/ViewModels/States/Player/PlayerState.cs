using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.ViewModels.States.Player
{
    public abstract class PlayerState
    {
        protected string showText;
        protected Sequencer sequencer;
        public PlayerState(Sequencer sequencer)
        {
            this.sequencer = sequencer;
            showText = "";
        }

        public virtual void GoInto(MidiPlayerViewModel owner)
        {

        }
    }
}

