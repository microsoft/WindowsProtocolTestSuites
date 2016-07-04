// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpei
{
    /// <summary>
    /// Abstract Data Model
    /// </summary>
    public class TouchContactStateMachine
    {
        // The finite state machine diagram, describes the states through which a contact involved in a touch transaction can transition during its lifetime.
        private static Dictionary<StateFlags, TouchContactState> transitionDiagram;

        /// <summary>
        /// Map the contact id to its state.
        /// </summary>
        public Dictionary<byte, TouchContactAttribute> contactStateMap;

        /// <summary>
        /// Initialize the transition diagram and reset the contact state table
        /// </summary>
        public void Initialize()
        {
            if (transitionDiagram == null)
            {
                BuildTransitionDiagram();
            }
            contactStateMap = new Dictionary<byte, TouchContactAttribute>();
        }

        private void BuildTransitionDiagram()
        {
            transitionDiagram = new Dictionary<StateFlags, TouchContactState>();
            InsertStateFlagTransition(TouchContactState.OutOfRange, ValidStateFlagCombinations.UPDATE_INRANGE, TouchContactState.Hovering);
            InsertStateFlagTransition(TouchContactState.OutOfRange, ValidStateFlagCombinations.DOWN_INRANGE_INCONTACT, TouchContactState.Engaged);
            InsertStateFlagTransition(TouchContactState.Hovering, ValidStateFlagCombinations.UPDATE_INRANGE, TouchContactState.Hovering);
            InsertStateFlagTransition(TouchContactState.Hovering, ValidStateFlagCombinations.UPDATE_CANCELED, TouchContactState.OutOfRange);
            InsertStateFlagTransition(TouchContactState.Hovering, ValidStateFlagCombinations.UPDATE, TouchContactState.OutOfRange);
            InsertStateFlagTransition(TouchContactState.Hovering, ValidStateFlagCombinations.DOWN_INRANGE_INCONTACT, TouchContactState.Engaged);
            InsertStateFlagTransition(TouchContactState.Engaged, ValidStateFlagCombinations.UP_INRANGE, TouchContactState.Hovering);
            InsertStateFlagTransition(TouchContactState.Engaged, ValidStateFlagCombinations.UP, TouchContactState.OutOfRange);
            InsertStateFlagTransition(TouchContactState.Engaged, ValidStateFlagCombinations.UP_CANCELED, TouchContactState.OutOfRange);
            InsertStateFlagTransition(TouchContactState.Engaged, ValidStateFlagCombinations.UPDATE_INRANGE_INCONTACT, TouchContactState.Engaged);
        }

        private void InsertStateFlagTransition(TouchContactState srcState, ValidStateFlagCombinations flag, TouchContactState desState)
        {
            StateFlags sf = new StateFlags { state = srcState, flag = flag };
            transitionDiagram.Add(sf, desState);
        }

        /// <summary>
        /// This method updates the state of a contact.
        /// </summary>
        /// <param name="id">The contactId in the RDPINPUT_CONTACT_DATA.</param>
        /// <param name="contactFlag">The valid value of contactFlags in the RDPINPUT_CONTACT_DATA</param>
        /// <param name="x">The x value of the previous position of the contact.</param>
        /// <param name="y">The y value of the previous position of the contact.</param>
        /// <returns>Returns 0 if the update succeeds. Returns 1 if the contact state transition is invalid. Returns 2 if the contact position changes when transitioning from "engaged" state to "out of range" or "hovering" state, which is unexpected.</returns>
        public byte UpdateContactsMap(byte id, ValidStateFlagCombinations contactFlag, int x, int y)
        {
            if (!contactStateMap.ContainsKey(id))
            {
                contactStateMap.Add(id, new TouchContactAttribute(TouchContactState.OutOfRange, -1, -1));
            }
            TouchContactAttribute attr;
            contactStateMap.TryGetValue(id, out attr);
            TouchContactState s;
            bool fResult = transitionDiagram.TryGetValue(new StateFlags(attr.state, contactFlag), out s);
            // Invalid contact state transition
            if (!fResult)
            {
                return 1;
            }
            // Check whether the contact position changes when transitioning from "engaged" state to "out of range" or "hovering" state.
            if (contactFlag == ValidStateFlagCombinations.UP || contactFlag == ValidStateFlagCombinations.UP_CANCELED || contactFlag == ValidStateFlagCombinations.UP_INRANGE)
            {
                TouchContactAttribute attribute;
                contactStateMap.TryGetValue(id, out attribute);
                if (x != attribute.x || y != attribute.y)
                {
                    return 2;
                }
            }
            // Valid contact state transition, update the contact state.
            contactStateMap.Remove(id);
            contactStateMap.Add(id, new TouchContactAttribute(s, x, y));
            return 0;
        }
    }

    /// <summary>
    /// Indicates the state of a contact
    /// </summary>
    public enum TouchContactState : byte
    {
        /// <summary>
        /// Out of Range.
        /// </summary>
        OutOfRange,

        /// <summary>
        /// Hovering.
        /// </summary>
        Hovering,

        /// <summary>
        /// Engaged.
        /// </summary>
        Engaged
    }

    /// <summary>
    /// Valid contact state flags
    /// </summary>
    public enum ValidStateFlagCombinations : uint
    {
        UP = RDPINPUT_CONTACT_DATA_ContactFlags.CONTACT_FLAG_UP,

        UP_CANCELED = RDPINPUT_CONTACT_DATA_ContactFlags.CONTACT_FLAG_UP | RDPINPUT_CONTACT_DATA_ContactFlags.CONTACT_FLAG_CANCELED,

        UPDATE = RDPINPUT_CONTACT_DATA_ContactFlags.CONTACT_FLAG_UPDATE,

        UPDATE_CANCELED = RDPINPUT_CONTACT_DATA_ContactFlags.CONTACT_FLAG_UPDATE | RDPINPUT_CONTACT_DATA_ContactFlags.CONTACT_FLAG_CANCELED,

        DOWN_INRANGE_INCONTACT = RDPINPUT_CONTACT_DATA_ContactFlags.CONTACT_FLAG_DOWN | RDPINPUT_CONTACT_DATA_ContactFlags.CONTACT_FLAG_INRANGE | RDPINPUT_CONTACT_DATA_ContactFlags.CONTACT_FLAG_INCONTACT,

        UPDATE_INRANGE_INCONTACT = RDPINPUT_CONTACT_DATA_ContactFlags.CONTACT_FLAG_UPDATE | RDPINPUT_CONTACT_DATA_ContactFlags.CONTACT_FLAG_INRANGE | RDPINPUT_CONTACT_DATA_ContactFlags.CONTACT_FLAG_INCONTACT,

        UP_INRANGE = RDPINPUT_CONTACT_DATA_ContactFlags.CONTACT_FLAG_UP | RDPINPUT_CONTACT_DATA_ContactFlags.CONTACT_FLAG_INRANGE,

        UPDATE_INRANGE = RDPINPUT_CONTACT_DATA_ContactFlags.CONTACT_FLAG_UPDATE | RDPINPUT_CONTACT_DATA_ContactFlags.CONTACT_FLAG_INRANGE
    }

    /// <summary>
    /// Indicates the last state and transition flags of a contact. Used to compute the current state of the contact.
    /// </summary>
    internal struct StateFlags
    {
        public TouchContactState state;

        public ValidStateFlagCombinations flag;

        public StateFlags(TouchContactState s, ValidStateFlagCombinations f)
        {
            state = s;
            flag = f;
        }
    }

    /// <summary>
    /// Indicates the contact state and position
    /// </summary>
    public struct TouchContactAttribute
    {
        public TouchContactState state;
        // The position of the contact.
        public int x;
        public int y;

        public TouchContactAttribute(TouchContactState s, int xVal, int yVal)
        {
            state = s;
            x = xVal;
            y = yVal;
        }
    }
}
