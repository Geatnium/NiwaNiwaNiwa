using UnityEngine.Events;
using System.Collections.Generic;

namespace Niwatori
{
    public class InputEvent
    {
        private List<EInput> inputs;
        private List<UnityEvent> calls;

        public InputEvent()
        {
            inputs = new List<EInput>();
            calls = new List<UnityEvent>();
        }

        public void AddListener(EInput input, UnityAction call)
        {
            inputs.Add(input);
            UnityEvent u = new UnityEvent();
            u.AddListener(call);
            calls.Add(u);
        }

        public void Invoke(EInput input)
        {
            int i = 0;
            foreach (EInput inputsItem in inputs)
            {
                if (inputsItem == input)
                {
                    calls[i].Invoke();
                }
                i++;
            }
        }
    }
}