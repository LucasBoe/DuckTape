using SS;

[SingletonSettings(SingletonLifetime.Persistant, _canBeGenerated: true, _eager: true)]
public class StatTracker : SingletonBehaviour<StatTracker>
{
    public int NumberOfStationsVisited = 0;
}