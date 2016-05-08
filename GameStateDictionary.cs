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

		public static void SetState(string stateName, string value){
			instance.setState (stateName, value);
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

		public static string GetState(string stateName) {
			return instance.getState (stateName);
		}
        public string getState(string stateName)
        {
            string value = "";
            if (StateDictionary.TryGetValue(stateName, out value))
                return value;
            else
                return String.Empty;
        }

		public static void Increment(string stateName) {
			instance.increment(stateName);
		}
		public void increment(string stateName) {
			string value = "";
			int parsedInt;

			if (StateDictionary.TryGetValue (stateName, out value)) {
				if (int.TryParse (value, out parsedInt)) {
					StateDictionary [stateName] = (parsedInt + 1).ToString ();
				} else {
					Console.WriteLine ("tried to increment non-int value [" + stateName + "]: " + value);
				}
			} else {
				StateDictionary [stateName] = "1";
			}
		}

		public static int GetNum(string stateName) {
			return instance.getNum(stateName);
		}
		public int getNum(string stateName) {
			string value;
			int parsedInt;
			if (StateDictionary.TryGetValue (stateName, out value)) {
				if (int.TryParse (value, out parsedInt)) {
					return parsedInt;
				} else {
					Console.WriteLine ("tried to increment non-int value [" + stateName + "]: " + value);
					return 0;
				}
			} else {
				return 0;
			}
		}

		public static void SetNum(string stateName, int num) {
			instance.setNum(stateName, num);
		}
		public void setNum(string stateName, int num) {
			StateDictionary [stateName] = num.ToString ();
		}
	


		public static bool GetFlag(string stateName) {
			return instance.getFlag (stateName);
		}
		public bool getFlag(string stateName) {
			string value = "";
			if (StateDictionary.TryGetValue (stateName, out value)) {
				return value.Equals("yes");
			} else {
				return false;
			}	
		}



		public static void SetFlag(string stateName, bool flag) {
			instance.setFlag (stateName, flag);
		}
		public void setFlag(string stateName, bool flag) {
			StateDictionary [stateName] = flag ? "yes" : "no";
		}
    }
}
