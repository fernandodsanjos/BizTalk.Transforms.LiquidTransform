using System;
using System.Collections.Generic;
using System.Resources;
using System.Drawing;
using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using System.ComponentModel;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.IO;
using Microsoft.BizTalk.Streaming;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.ScalableTransformation;
using Microsoft.XLANGs.BaseTypes;
//using Microsoft.XLANGs.RuntimeTypes;
using System.ComponentModel.DataAnnotations;
using Microsoft.BizTalk.Component.Utilities;
using Microsoft.XLANGs.RuntimeTypes;
using System.Xml.Serialization;


namespace BizTalkComponents.PipelineComponents
{

    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_Encoder)]
    [System.Runtime.InteropServices.Guid("0852030C-5C41-4FC7-B38D-5034239253F7")]
    public partial class StripCDATA : IBaseComponent
    {





        public IBaseMessage Execute(IPipelineContext pContext, IBaseMessage pInMsg)
        {

            BizTalk.Transforms.LiquidTransform.CDATAStream cdata = new BizTalk.Transforms.LiquidTransform.CDATAStream(pInMsg.BodyPart.Data);

            pInMsg.BodyPart.Data = cdata;
            pContext.ResourceTracker.AddResource(cdata);

            return pInMsg;
        }



    }

    
}
