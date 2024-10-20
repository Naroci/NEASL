using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using NEASL.Base.Object;

namespace NEASL.Base;

public class EventManager
{

    Dictionary<string,IBaseEventReceiver> m_ReceiverList = new Dictionary<string,IBaseEventReceiver>();
    public EventManager()
    { 
    }

    public void Register(IBaseEventReceiver Event)
    {
        if (Event == null || Event != null && Event.GetUniqueIdentifier() == null)
            return;

        string identifier = Event.GetUniqueIdentifier();
        if (m_ReceiverList.ContainsKey(identifier))
            m_ReceiverList[identifier] = Event;
        else 
            m_ReceiverList.Add(identifier, Event);
    }

    public bool ReceiverForInstructionRegistered(Instruction instruction)
    {
        if (instruction == null || m_ReceiverList == null || m_ReceiverList != null && m_ReceiverList.Count == 0
            || instruction != null && string.IsNullOrEmpty(instruction.BaseName))
            return false;
        
        return m_ReceiverList.ContainsKey(instruction.BaseName);
    }

    public void FireByInstruction(Instruction instruction)
    {
        if (instruction == null || m_ReceiverList == null || m_ReceiverList != null && m_ReceiverList.Count == 0)
            return;

        Notify(m_ReceiverList[instruction.BaseName], instruction);
    }
    
    private void Notify(IBaseEventReceiver theEvent, Instruction instruction)
    {
        if (theEvent == null || instruction == null || instruction != null && string.IsNullOrEmpty(instruction.MethodName))
            throw new ArgumentNullException(nameof(theEvent));
        try
        {
            var args = InstructionReader.ResolveReferencedVariables(instruction.Sender, instruction.Arguments,instruction.IsAssignment);
            theEvent.Notify(instruction.MethodName.Trim(), args);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}