namespace Niwatori
{
    public interface IAttackCollider
    {
        void SetNumberAndTeamNumber(int number, int teamNumber);
        int GetNumber();
        int GetTeamNumber();
        void SetPower(int power);
        int GetPower();
    }
}