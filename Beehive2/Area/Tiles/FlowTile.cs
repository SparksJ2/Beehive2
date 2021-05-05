using System;

namespace Beehive2
{
	[Serializable()]
	public class FlowTile : BaseTile<FlowMap, FlowTile>
	{
		public double flow;
		public bool mask;

		public FlowTile(Loc p, FlowMap f) : base(p, f)
		{
		}
	}
}