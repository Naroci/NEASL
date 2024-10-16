namespace NEASL.Base;

public class Context
{
    private EventManager eventManager;
    public EventManager GetEventManager()
    {
        if (eventManager == null)
            eventManager = new EventManager();

        return eventManager;
    }
    
    private InstructionQueryManager m_queryManager;
    public InstructionQueryManager GetQueryManager()
    {
        if (m_queryManager == null)
            m_queryManager = new InstructionQueryManager();
        
        return m_queryManager;
    }
    
    private static Context current;
    public static Context GetInstance()
    {
        if (current == null)
            current = new Context();
        
        return current;
    }

}