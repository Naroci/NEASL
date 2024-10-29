using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using NEASL.Base.Object;

namespace NEASL.Base;

public class EventManager
{
    // Dictionary of curren subscribed eventlisteners.
    Dictionary<string,IBaseEventReceiver> m_ReceiverList = new Dictionary<string,IBaseEventReceiver>();
    
    
    public EventManager()
    { 
    }

    /// <summary>
    /// Registers / Adds a new subscriber to the list of listeners.
    /// </summary>
    /// <param name="Event">The Listener.</param>
    public void Register(IBaseEventReceiver Event)
    {
        // Fetch the pointer like identifier. if null the Receiver is not valid.
        if (Event == null || Event != null && (string.IsNullOrEmpty(Event.GetFullName())) )
            return; // otherwise nullpointer exception.

        string identifier = Event.GetFullName();
        
        // Make sure that each Identifier (Type/Name combination) only exists once!
        if (m_ReceiverList.ContainsKey(identifier))
            m_ReceiverList[identifier] = Event; 
            // additionally : throw new DuplicateNameException();
        else
            m_ReceiverList.Add(identifier, Event);
    }
    
    public string FindFullReceiverNameByNameReference(string Name)
    {
        if (string.IsNullOrEmpty(Name))
            throw new ArgumentNullException();

        if (m_ReceiverList.Keys.ToList().Contains($"$${Name.ToLower()}"))
        {
            return m_ReceiverList.Keys.ToList().First(x => x.Contains($"$${Name.ToLower()}"));
        }

        return null;
    }

    /// <summary>
    /// Returns if there is any Listener registered for the given Instruction or not.
    /// </summary>
    /// <param name="instruction">The Instruction to find a Listener for.</param>
    /// <returns>FALSE || TRUE</returns>
    public bool ReceiverForInstructionRegistered(Instruction instruction)
    {
        if (instruction == null || m_ReceiverList == null || m_ReceiverList != null && m_ReceiverList.Count == 0
            || instruction != null && string.IsNullOrEmpty(instruction.ObjectName))
                return false;
        return m_ReceiverList.ContainsKey(instruction.ObjectName);
    }
    
    /// <summary>
    /// Fires an event by the given Instruction.
    /// </summary>
    /// <param name="instruction">The Event to fire.</param>
    public void FireByInstruction(Instruction instruction)
    {
        if (instruction == null || m_ReceiverList == null || m_ReceiverList != null && m_ReceiverList.Count == 0
            || instruction != null && string.IsNullOrEmpty(instruction.ObjectName))
            return;
    
        Notify(m_ReceiverList[instruction.ObjectName], instruction);
    }
    
    /// <summary>
    /// Notifies the subscribed element to perform the requested instruction.
    /// </summary>
    /// <param name="theEvent">The subscribed element / Eventlistener</param>
    /// <param name="instruction">the Instruction to execute.</param>
    /// <exception cref="ArgumentNullException">Is thrown once one of the arguments are NULL</exception>
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