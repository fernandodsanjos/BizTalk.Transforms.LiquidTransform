using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using System.Xml;
using System.IO;

namespace BizTalk.Transforms.LiquidTransform
{
	public class CDATA : DotLiquid.Block, ILiquidRegister
	{
		public void Register(RenderParameters parameters)
		{
			Template.RegisterTag<CDATA>("cdata");
		}

		public override void Render(Context context, TextWriter result)
		{
			result.Write("<JSON><![CDATA[");
			base.Render(context, result);
			result.Write("]]></JSON>");
		}


	}
}
