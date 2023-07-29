namespace Pixie
{
    public class GameState
    {
        private bool _shutDown = false;

        internal GameState()
        { }

        public void ShutDown()
        {
            _shutDown = true;
        }

        internal bool ShouldShutDown()
        { 
            return _shutDown; 
        }
    }
}
