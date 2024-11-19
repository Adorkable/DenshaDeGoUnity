using System.Runtime.InteropServices;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace 電車でGO
{
    [StructLayout(LayoutKind.Sequential)]
    public struct 電車でGO新幹線DeviceState : IInputStateTypeInfo
    {
        public static FourCC kFormat => new FourCC('電', '車', 'で', 'G');
        public FourCC format => kFormat;

        public enum Button
        {
            DPadUp = 0,
            DPadDown = 1,
            DPadLeft = 2,
            DPadRight = 3,
            Start = 4,
            Select = 5,
            A = 6,
            B = 7,
            C = 8,
            D = 9,
        }

        [InputControl(name = "dpad", layout = "Dpad", sizeInBits = 4, bit = 0)]
        [InputControl(name = "dpad/up", displayName = "Up", bit = (uint)Button.DPadUp)]
        [InputControl(name = "dpad/down", displayName = "Down", bit = (uint)Button.DPadDown)]
        [InputControl(name = "dpad/left", displayName = "Left", bit = (uint)Button.DPadLeft)]
        [InputControl(name = "dpad/right", displayName = "Right", bit = (uint)Button.DPadRight)]

        [InputControl(name = "start", bit = (uint)Button.Start, displayName = "Start")]
        [InputControl(name = "select", bit = (uint)Button.Select, displayName = "Select")]
        [InputControl(name = "buttonSouth", bit = (uint)Button.B, displayName = "B")]
        [InputControl(name = "buttonEast", bit = (uint)Button.C, displayName = "C")]
        [InputControl(name = "buttonWest", bit = (uint)Button.A, displayName = "A")]
        [InputControl(name = "buttonNorth", bit = (uint)Button.D, displayName = "D")]
        public ushort buttons;

        [InputControl(name = "brakeLever", layout = "Axis", displayName = "Brake Lever", shortDisplayName = "Brake", processors = "normalize(min=-0.125,max=1,zero=-0.125)")] // TODO: is this just mine that it reports -0.125?
        public float brakeLever;

        [InputControl(name = "powerLever", layout = "Axis", displayName = "Power Lever", shortDisplayName = "Power")]
        public float powerLever;

        static ushort DirectionToButtonFlags(DirectionState directionState)
        {
            switch (directionState)
            {
                case DirectionState.Up:
                    return 1;
                case DirectionState.UpperRight:
                    return 1 | (1 << 3);
                case DirectionState.Right:
                    return 1 << 3;
                case DirectionState.DownRight:
                    return 1 << 1 | 1 << 3;
                case DirectionState.Down:
                    return 1 << 1;
                case DirectionState.DownLeft:
                    return 1 << 1 | 1 << 2;
                case DirectionState.Left:
                    return 1 << 2;
                case DirectionState.UpperLeft:
                    return 1 | 1 << 2;
                default:
                    return 0;
            }
        }

        public 電車でGO新幹線DeviceState(新幹線専用コントローライージィ.ReadStateEventArgs fromEventArgs)
        {
            buttons = DirectionToButtonFlags(fromEventArgs.Direction);

            if (fromEventArgs.StartButton)
            {
                buttons |= 1 << 4;
            }
            if (fromEventArgs.SelectButton)
            {
                buttons |= 1 << 5;
            }

            // TODO: can we get the shift value from the enum?
            if (fromEventArgs.AButton)
            {
                buttons |= 1 << 6;
            }
            if (fromEventArgs.BButton)
            {
                buttons |= 1 << 7;
            }
            if (fromEventArgs.CButton)
            {
                buttons |= 1 << 8;
            }
            if (fromEventArgs.DButton)
            {
                buttons |= 1 << 9;
            }

            // TODO: should we and how should we report emergency and release brake states?
            if (fromEventArgs.BrakeHandle.inBetween)
            {
                brakeLever = fromEventArgs.BrakeHandle.previousPercentageLevel;
            }
            else
            {
                brakeLever = fromEventArgs.BrakeHandle.percentageLevel;
            }

            if (fromEventArgs.PowerHandle.inBetween)
            {
                powerLever = fromEventArgs.PowerHandle.previousPercentageLevel;
            }
            else
            {
                powerLever = fromEventArgs.PowerHandle.percentageLevel;
            }
        }
    };
}