using UnityEngine;

namespace Niwatori
{
    public class AttackCollider : MonoBehaviour, IAttackCollider
    {
        private int power;
        private int number;
        private int teamNumber;

        public void SetNumberAndTeamNumber(int number, int teamNumber)
        {
            this.number = number;
            this.teamNumber = teamNumber;
        }

        public int GetNumber()
        {
            return number;
        }

        public int GetTeamNumber()
        {
            return teamNumber;
        }

        public void SetPower(int power)
        {
            this.power = power;
        }

        public int GetPower()
        {
            return power;
        }
    }
}
