#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
	public class RangeExpansion : Indicator
	{
		private ATR atrIndicator;
		private int periodSize;
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Displays the ratio of the ATR in the current period to the previous one. Numbers around one indicate volatility has remained the same within the lookback period. Numbers higher indicate it has increased, and number lower indicate it has decreased.";
				Name										= "RangeExpansion";
				Calculate									= Calculate.OnBarClose;
				IsOverlay									= false;
				DisplayInDataBox							= true;
				DrawOnPricePanel							= true;
				DrawHorizontalGridLines						= true;
				DrawVerticalGridLines						= true;
				PaintPriceMarkers							= true;
				ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				//Disable this property if your indicator requires custom values that cumulate with each new market data event. 
				//See Help Guide for additional information.
				IsSuspendedWhileInactive					= true;
				Lookback					= 14;
				AddPlot(Brushes.Black, "Range Expansion Ratio");
			}
			else if (State == State.Configure)
			{
				periodSize = Lookback/2;
			}
			else if(State == State.DataLoaded)
			{
				atrIndicator = ATR(periodSize);	 
			}
		}

		protected override void OnBarUpdate()
		{
			if(CurrentBar < Lookback)
				return;
			
			
			double rangeExpansionScore = atrIndicator[0]/atrIndicator[periodSize];
			
			Values[0][0] = rangeExpansionScore;
		}

		#region Properties
		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="Lookback", Order=1, GroupName="Parameters")]
		public int Lookback
		{ get; set; }
		#endregion

	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private RangeExpansion[] cacheRangeExpansion;
		public RangeExpansion RangeExpansion(int lookback)
		{
			return RangeExpansion(Input, lookback);
		}

		public RangeExpansion RangeExpansion(ISeries<double> input, int lookback)
		{
			if (cacheRangeExpansion != null)
				for (int idx = 0; idx < cacheRangeExpansion.Length; idx++)
					if (cacheRangeExpansion[idx] != null && cacheRangeExpansion[idx].Lookback == lookback && cacheRangeExpansion[idx].EqualsInput(input))
						return cacheRangeExpansion[idx];
			return CacheIndicator<RangeExpansion>(new RangeExpansion(){ Lookback = lookback }, input, ref cacheRangeExpansion);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.RangeExpansion RangeExpansion(int lookback)
		{
			return indicator.RangeExpansion(Input, lookback);
		}

		public Indicators.RangeExpansion RangeExpansion(ISeries<double> input , int lookback)
		{
			return indicator.RangeExpansion(input, lookback);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.RangeExpansion RangeExpansion(int lookback)
		{
			return indicator.RangeExpansion(Input, lookback);
		}

		public Indicators.RangeExpansion RangeExpansion(ISeries<double> input , int lookback)
		{
			return indicator.RangeExpansion(input, lookback);
		}
	}
}

#endregion
