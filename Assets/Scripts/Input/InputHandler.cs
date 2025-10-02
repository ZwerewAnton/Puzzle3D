namespace Input
{
    public class InputHandler
    {
        public InputActions.CameraActions CameraActions { get; private set; }
        private InputActions InputActions { get; set; }

        public InputHandler()
        {
            InputActions = new InputActions();
            CameraActions = InputActions.Camera;
        }
        
        private void EnableActions()
        {
            InputActions.Enable();
        }

        private void DisableActions()
        {
            InputActions.Disable();
        }
    }
}