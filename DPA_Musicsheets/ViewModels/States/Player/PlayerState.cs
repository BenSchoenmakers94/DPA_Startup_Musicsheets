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

        public virtual void Play(MidiPlayerViewModel owner)
        {
            owner.ChangeState("Playing");
        }

        public virtual void Pause(MidiPlayerViewModel owner)
        {
            owner.ChangeState("Paused");
        }

        public virtual void Stop(MidiPlayerViewModel owner)
        {
            owner.ChangeState("Stopped");
        }

        public virtual void GoInto(MidiPlayerViewModel owner)
        {
            
        }
    }
}

