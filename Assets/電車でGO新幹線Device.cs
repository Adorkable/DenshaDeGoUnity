using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

// TODO: SetMotorSpeeds, PauseHaptics, etc
// TODO: DualMotorRumble
namespace 電車でGO
{
    [InputControlLayout(stateType = typeof(電車でGO新幹線DeviceState), displayName = "電車でGO新幹線 Controller")]
    public sealed class 電車でGO新幹線Device : Gamepad
    {
        private 新幹線専用コントローライージィ controller;

        public bool logDebug = false;

        /// <summary>
        /// The last used/added controller.
        /// </summary>
        public new static 電車でGO新幹線Device current { get; private set; }

        /// <inheritdoc />
        public override void MakeCurrent()
        {
            base.MakeCurrent();
            if (logDebug)
            {
                UnityEngine.Debug.Log("電車でGO新幹線Device: MakeCurrent");
            }
            current = this;
        }

        protected override void OnAdded()
        {
            if (logDebug)
            {
                UnityEngine.Debug.Log("電車でGO新幹線Device: OnAdded");
            }
            base.OnAdded();
        }

        /// <inheritdoc />
        protected override void OnRemoved()
        {
            if (current == this)
            {
                current = null;
            }
            if (logDebug)
            {
                UnityEngine.Debug.Log("電車でGO新幹線Device: OnRemoved");
            }

            base.OnRemoved();
        }

        private ButtonControl _dpadUp;
        public ButtonControl dpadUp => _dpadUp;
        private ButtonControl _dpadDown;
        public ButtonControl dpadDown => _dpadDown;
        private ButtonControl _dpadLeft;
        public ButtonControl dpadLeft => _dpadLeft;
        private ButtonControl _dpadRight;
        public ButtonControl dpadRight => _dpadRight;

        private ButtonControl _buttonNorth;
        public new ButtonControl buttonNorth => _buttonNorth;
        private ButtonControl _buttonSouth;
        public new ButtonControl buttonSouth => _buttonSouth;
        private ButtonControl _buttonWest;
        public new ButtonControl buttonWest => _buttonWest;
        private ButtonControl _buttonEast;
        public new ButtonControl buttonEast => _buttonEast;

        private ButtonControl _select;
        public ButtonControl select => _select;
        private ButtonControl _start;
        public ButtonControl start => _start;

        private AxisControl _brakeLever;
        public AxisControl brakeLever => _brakeLever;
        private AxisControl _powerLever;
        public AxisControl powerLever => _powerLever;

        /// <inheritdoc />
        protected override void FinishSetup()
        {
            _dpadUp = GetChildControl<ButtonControl>("dpad/up");
            _dpadDown = GetChildControl<ButtonControl>("dpad/down");
            _dpadLeft = GetChildControl<ButtonControl>("dpad/left");
            _dpadRight = GetChildControl<ButtonControl>("dpad/right");

            _buttonNorth = GetChildControl<ButtonControl>("buttonNorth");
            _buttonSouth = GetChildControl<ButtonControl>("buttonSouth");
            _buttonWest = GetChildControl<ButtonControl>("buttonWest");
            _buttonEast = GetChildControl<ButtonControl>("buttonEast");

            _select = GetChildControl<ButtonControl>("select");
            _start = GetChildControl<ButtonControl>("start");

            _brakeLever = GetChildControl<AxisControl>("brakeLever");
            _powerLever = GetChildControl<AxisControl>("powerLever");

            base.FinishSetup();
        }

        public void SetController(新幹線専用コントローライージィ controller)
        {
            this.controller = controller;
            controller.OnReadState += HandleController_OnReadState;
        }

        public void Dispose()
        {
            if (controller != null)
            {
                controller.OnReadState -= HandleController_OnReadState;
                controller.Dispose();
            }
        }

        private Queue<電車でGO新幹線DeviceState> _eventQueue = new Queue<電車でGO新幹線DeviceState>();
        internal void ProcessEventQueue()
        {
            while (_eventQueue.Count > 0)
            {
                var newState = _eventQueue.Dequeue();
                InputSystem.QueueStateEvent(this, newState);
            }
        }

        private void HandleController_OnReadState(object sender, 新幹線専用コントローライージィ.ReadStateEventArgs eventArgs)
        {
            var newState = new 電車でGO新幹線DeviceState(eventArgs);
            _eventQueue.Enqueue(newState);
        }

        public void SetLeftRumble(bool enabled)
        {
            if (controller == null)
            {
                return;
            }

            controller.EnableLeftRumble(enabled);
        }

        public void EnableLeftRumble()
        {
            if (controller == null)
            {
                return;
            }
            controller.EnableLeftRumble(true);
        }

        public void DisableLeftRumble()
        {
            if (controller == null)
            {
                return;
            }

            controller.EnableLeftRumble(false);

        }

        public void SetRightRumble(bool enabled)
        {
            if (controller == null)
            {
                return;
            }

            controller.EnableRightRumble(enabled);
        }

        public void EnableRightRumble()
        {
            if (controller == null)
            {
                return;
            }

            controller.EnableRightRumble(true);
        }

        public void DisableRightRumble()
        {
            if (controller == null)
            {
                return;
            }

            controller.EnableRightRumble(false);
        }

        public void SetLargeSegmentBar(int value)
        {
            if (controller == null)
            {
                return;
            }

            controller.SetLargeSegmentBar(value);
        }

        public void SetSmallSegmentBar(int value)
        {
            if (controller == null)
            {
                return;
            }

            controller.SetSmallSegmentBar(value);
        }

        public void SetDoorsClosedLight(bool value)
        {
            if (controller == null)
            {
                return;
            }

            controller.EnableDoorsClosedLight(value);
        }

        public void EnableDoorsClosedLight()
        {
            if (controller == null)
            {
                return;
            }

            controller.EnableDoorsClosedLight(true);
        }

        public void DisableDoorsClosedLight()
        {
            if (controller == null)
            {
                return;
            }

            controller.EnableDoorsClosedLight(false);
        }

        public void SetSpeedDisplay(int value)
        {
            if (controller == null)
            {
                return;
            }

            controller.SetSpeedDisplay(value);
        }

        public void SetLowerSpeedDisplay(int value)
        {
            if (controller == null)
            {
                return;
            }

            controller.SetLowerSpeedDisplay(value);
        }

        public void SetUpperSpeedDisplay(int value)
        {
            if (controller == null)
            {
                return;
            }

            controller.SetUpperSpeedDisplay(value);
        }

        public void SetATCDisplay(int value)
        {
            if (controller == null)
            {
                return;
            }

            controller.SetATCDisplay(value);
        }

        public void SetLowerATCDisplay(byte value)
        {
            if (controller == null)
            {
                return;
            }

            controller.SetLowerATCDisplay(value);
        }

        public void SetUpperATCDisplay(byte value)
        {
            if (controller == null)
            {
                return;
            }

            controller.SetUpperATCDisplay(value);
        }
    }
}