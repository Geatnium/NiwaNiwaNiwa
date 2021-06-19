using UnityEngine;
using UnityEngine.Events;

namespace Niwatori
{
    public interface IChicken
    {
        void SetNumber(int number);
        void SetTeamNumber(int teamNumber);
        void SetIsPlayer(bool isPlayer);
        void SetNameLabel(string name);
        string GetName();
        void SetNameColor(ENameColor nameColor);
        int GetNumber();
        int GetTeamNumber();
        void DoFindPairChickenAndSet();
        GameObject GetMineGameObject();
        IChicken GetPairChicken();
        void SetIsEnable(bool enable);
        int GetIntVariable(string variableName);
        float GetFloatVariable(string variableName);
        bool GetBoolVariable(string variableName);
        object GetVariableNoCast(string variableName);
        void AddDownInputEvent(EInput input, UnityAction call);
        void AddUpInputEvent(EInput input, UnityAction call);
        void OnDownInput(EInput input);
        void OnUpInput(EInput input);
        void OnTakeDamage(int damage);
        void OnTeamDeath();
        void OnResurectPair();
        bool GetIsResurrectAble();
        bool GetIsInput(EInput input);
        float GetHorizontal();
        float GetVertical();
        Vector3 GetPosition();
    }
}
