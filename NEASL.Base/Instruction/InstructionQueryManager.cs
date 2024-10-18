using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;

namespace NEASL.Base;

public class InstructionQueryManager
{
    private bool m_isRunning;
    private BlockingCollection<Instruction> InstructionQuery;
    private DateTime m_lastStartTime;
    private Instruction m_currentInstruction;
    private int m_TimeOutTime;

    public void SetTimeOutTime(int timeOutTime)
    {
        this.m_TimeOutTime = timeOutTime;
    }

    public void Stop()
    {
        m_isRunning = false;
    }

    public void Start()
    {
        m_isRunning = true;
        m_currentInstruction = GetCurrentItem();
        if (m_currentInstruction == null)   
            Stop();
        
        if (IsRunning() && m_currentInstruction != null)
        {
            if (Context.GetInstance().GetEventManager().ReceiverForInstructionRegistered(m_currentInstruction))
                Context.GetInstance().GetEventManager().FireByInstruction(m_currentInstruction);
            else
            {
                Console.WriteLine($"Current Instruction ({m_currentInstruction.BaseName}->{m_currentInstruction.MethodName}()) could not be reolved");
                Console.WriteLine("Exiting...");
                Stop();
            }
        }
        else
        {
            Console.WriteLine("Nothing to do...!");
            Stop();
        }

      
    }

    private void StartControlLoop()
    {

    }

    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
      
    }

    public bool IsRunning()
    {
        return this.m_isRunning;
    }

    public InstructionQueryManager()
    {
        this.InstructionQuery = new BlockingCollection<Instruction>();
    }
    
    private Instruction currentItem
    {
        get
        {
            if (this.InstructionQuery != null && InstructionQuery.Count > 0
                                              && this.InstructionQuery.ToList().Exists(x => x?.Completed == false))
            {
                var temp = this.InstructionQuery.Where(x => x?.Completed == false).ToList();
                var itm = temp.OrderBy(x => x?.Id)?.First();
                return itm;
            }
            return null;
        }
    }

    public Instruction GetCurrentItem()
    {
        return this.currentItem;
    }

    public List<Instruction> GetQuery()
    {
        return this.InstructionQuery.ToList();
    }

    public void AddToQuery(IEnumerable<Instruction> instructions)
    {
        try
        {
            Type defaultType = typeof(Instruction);
            if (this.InstructionQuery != null && this.InstructionQuery.Count > 100)
            {
                var temp = this.InstructionQuery.Where(x => x?.Completed == false);
                lock (this.InstructionQuery)
                {
                    this.InstructionQuery = new BlockingCollection<Instruction>();
                    foreach (var item in temp)
                    {
                        this.InstructionQuery.Add(item);
                    }
                }
            }
            if (instructions != null)
            {
                lock (this.InstructionQuery)
                {
                    lock (instructions)
                    {
                        foreach (var instruction in instructions)
                        {
                            this.InstructionQuery.Add(instruction);
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(string.Format("ERROR WHILE ADDTOQUERY REQUEST!\n: {0}", e.Message));
        }
    }

    public void AddToQuery(Instruction instruction)
    {
        try
        {
            if (this.InstructionQuery != null && this.InstructionQuery.Count > 100)
            {
                var temp = this.InstructionQuery.Where(x => x?.Completed == false);
                lock (this.InstructionQuery)
                {
                    this.InstructionQuery = new BlockingCollection<Instruction>();
                    foreach (var item in temp)
                    {
                        this.InstructionQuery.Add(item);
                    }
                }
            }
            if (instruction != null)
            {
                lock (this.InstructionQuery)
                {
                    lock (instruction)
                    {
                        var type = instruction.GetType();
                        this.InstructionQuery.Add(instruction);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(string.Format("ERROR WHILE ADDTOQUERY REQUEST!\n: {0}", e.Message));
        }
    }

    private List<Instruction> Sort(List<Instruction> eventQueue)
    {
        try
        {
            lock (this.InstructionQuery)
            {
                var newList = this.InstructionQuery.Where(x => !x.Completed).ToList();
                newList = newList.OrderBy(x => x?.Id).ToList();
                return newList;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(string.Format("[EXCEPTION] Exception in GameEventQuery Occured! : {0}", e.Message));
            return null;
        }
    }

    public bool ItemExists(Instruction theEvent)
    {
        if (this.InstructionQuery == null)
            return false;

        return this.InstructionQuery.ToList().Contains(theEvent);
    }

    public void SendCompleted(string sourceName, string MethodName, object[] args)
    {
        if (string.IsNullOrEmpty(sourceName) || string.IsNullOrEmpty(MethodName))
            return;
        
        if (m_currentInstruction == null)
        {
            return;
        }
        
        lock (m_currentInstruction)
        {
            if (m_currentInstruction != null && m_currentInstruction.BaseName.Trim() == sourceName.Trim() &&
                m_currentInstruction.MethodName.Trim() == MethodName.Trim())
            {
                if (m_currentInstruction.Arguments != null && args == null 
                    || args != null && m_currentInstruction.Arguments == null
                    ||args != null && m_currentInstruction.Arguments != null && args.Length != m_currentInstruction.Arguments.Length)
                        Console.WriteLine("SendCompleted -> [INVALID INSTRUCTION REQUEST! Arguments do not match!]");
                
                else if (args == null && m_currentInstruction.Arguments == null
                         || args != null && m_currentInstruction.Arguments != null &&
                         args.Length == m_currentInstruction.Arguments.Length)
                {
                    RemoveItemFromList(m_currentInstruction);
                    Start();
                }
            }
        }
    }

    public bool RemoveItemFromList(Instruction theEvent)
    {
        try
        {
            lock (theEvent)
            {
                theEvent.Completed = true;
                lock (this.InstructionQuery)
                {
                    var index = this.InstructionQuery.ToList().IndexOf(theEvent);
                    if (index < 0)
                        return false;
                
                    this.InstructionQuery.ToList()[index] = theEvent;
                    return true;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(
                string.Format("[EXCEPTION] Exception in GameEventQuery (RemoveItemFromList) Occured! : {0}",
                    e.Message));
            return false;
        }
    }
}