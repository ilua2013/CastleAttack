public interface IUnit 
{
    Mover Mover { get; }
    Fighter Fighter { get; }
    UnitCard Card { get; }
    void Init(UnitCard card, Cell cell);
    void DoStep();
}
