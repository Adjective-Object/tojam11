using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    class GameStateDictionary
    {
        private Dictionary<string, string> StateDictionary;
        public static GameStateDictionary instance;
        public GameStateDictionary()
        {
            StateDictionary = new Dictionary<string, string>();
            instance = this;
        }

        public void setState(string stateName, string value)
        {
            if (StateDictionary.ContainsKey(stateName))
            {
                StateDictionary[stateName] = value;
            }
            else
            {
                StateDictionary.Add(stateName, value);
            }
        }

        public string getState(string stateName)
        {
            string value = "";
            if (StateDictionary.TryGetValue(stateName, out value))
                return value;
            else
                return String.Empty;
        }
    }
}
