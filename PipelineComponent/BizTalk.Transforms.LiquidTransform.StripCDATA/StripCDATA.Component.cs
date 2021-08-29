using System;
using System.Collections.Generic;
using System.Resources;
using System.Drawing;
using System.Collections;
using System.Reflection;
using System.ComponentModel;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.IO;
using Microsoft.BizTalk.Streaming;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.XLANGs.RuntimeTypes;

namespace BizTalkComponents.PipelineComponents
{

    public partial class StripCDATA : Microsoft.BizTalk.Component.Interop.IComponent, IComponentUI
    {
      

        #region IBaseComponent Members

        public string Description
        {
            get { return "Pipeline Component to strip liquid content from CDATA envelope"; }
        }

        public string Name
        {
            get { return "Strip CDATA"; }
        }

        public string Version
        {
            get { return "1.0.0"; }
        }

        #endregion

        #region IComponentUI Members

        public IntPtr Icon
        {
            get
            {
                return new IntPtr();
            }
        }

        public IEnumerator Validate(object projectSystem)
        {
            return null;
        }

        #endregion



        public void GetClassID(out Guid classID)
        {
            classID = new Guid("0852030C-5C41-4FC7-B38D-5034239253F7");
        }

        public void InitNew()
        {
            throw new Exception("The method or operation is not implemented.");
        }

       
    }

}
