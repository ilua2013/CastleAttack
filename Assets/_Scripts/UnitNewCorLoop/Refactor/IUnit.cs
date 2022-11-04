public interface IUnit 
{
    Mover Mover { get; }
    Fighter Fighter { get; }
    Card Card { get; }
    void Init(Card card, Cell cell);
    void DoStep();
}
