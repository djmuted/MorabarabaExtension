using System;
using System.Collections.Generic;
using System.Text;

namespace MorabarabaExtension
{
    class MorabarabaField
    {
        public string name;
        public int value;

        public MorabarabaField(string _name)
        {
            this.name = _name;
            this.value = -1;
        }
    }
}
