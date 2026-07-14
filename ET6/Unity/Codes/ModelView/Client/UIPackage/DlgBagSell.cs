using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
namespace ET
{
    [ComponentOf(typeof(UIComponent))]
    public class DlgBagSell : Entity, IAwake, IDestroy
    {
        public GameObject m_window = null;

        public int m_nMaterialId = 0;
    }
}
