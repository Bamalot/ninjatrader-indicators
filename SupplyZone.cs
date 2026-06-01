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
	public class SupplyZone : Indicator
	{
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Highlights the most recent region of supprt based on parameters";
				Name										= "SupplyZone";
				Calculate									= Calculate.OnBarClose;
				IsOverlay									= true;
				DisplayInDataBox							= true;
				DrawOnPricePanel							= true;
				DrawHorizontalGridLines						= true;
				DrawVerticalGridLines						= true;
				PaintPriceMarkers							= true;
				ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				//Disable this property if your indicator requires custom values that cumulate with each new market data event. 
				//See Help Guide for additional information.
				IsSuspendedWhileInactive					= true;
				Lookback					= 50;
				PivotBars					= 2;
				MaxRegionSize               = 10.0;  
				RegionPadding				= 0.25;
			}
			else if (State == State.Configure)
			{
			}
		}

		protected override void OnBarUpdate()
		{
			if(CurrentBar < Lookback)
				return;
			
			double regionLow = Low[0];
			double regionHigh = Low[0];
			double lowestLow = Low[LowestBar(Low, Lookback)];
			// This loop goes through each bar to find the pivot points
			for(int i = PivotBars;i < Lookback-PivotBars;i++)
			{
				bool pivotFound = true;
				// Checks if this bar is a pivot point
				for(int l = i-PivotBars;l <= i+PivotBars;l++)
				{
					if(Low[i] > Low[l])
					{
						pivotFound = false;
						break;
					}
				}
				// Expand the region if necessary
				if(pivotFound && Math.Abs(regionLow - Low[i]) <= MaxRegionSize)
				{
					regionLow = Math.Min(regionLow, Low[i]);
					regionHigh = Math.Max(regionHigh, Low[i]);
				}

			}
			// Add padding to the top and bottom of the region
			regionHigh += RegionPadding;
			regionLow -= RegionPadding;
			
			Draw.Rectangle(this, "Supply Zone", Time[0], regionHigh, Time[Lookback-1], regionLow, Brushes.Green);
		}

		#region Properties
		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="Lookback Period", Order=1, GroupName="Parameters")]
		public int Lookback
		{ get; set; }

		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="Pivot Bars", Order=2, GroupName="Parameters")]
		public int PivotBars
		{ get; set; }
		
		[NinjaScriptProperty]
		[Range(1.0, double.MaxValue)]
		[Display(Name="Max Region Size ", Order=3, GroupName="Parameters")]
		public double MaxRegionSize
		{ get; set; }
		
		[NinjaScriptProperty]
		[Range(0.1, double.MaxValue)]
		[Display(Name="Region Padding", Order=4, GroupName="Parameters")]
		public double RegionPadding
		{ get; set; }
		#endregion

	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private SupplyZone[] cacheSupplyZone;
		public SupplyZone SupplyZone(int lookback, int pivotBars, double maxRegionSize, double regionPadding)
		{
			return SupplyZone(Input, lookback, pivotBars, maxRegionSize, regionPadding);
		}

		public SupplyZone SupplyZone(ISeries<double> input, int lookback, int pivotBars, double maxRegionSize, double regionPadding)
		{
			if (cacheSupplyZone != null)
				for (int idx = 0; idx < cacheSupplyZone.Length; idx++)
					if (cacheSupplyZone[idx] != null && cacheSupplyZone[idx].Lookback == lookback && cacheSupplyZone[idx].PivotBars == pivotBars && cacheSupplyZone[idx].MaxRegionSize == maxRegionSize && cacheSupplyZone[idx].RegionPadding == regionPadding && cacheSupplyZone[idx].EqualsInput(input))
						return cacheSupplyZone[idx];
			return CacheIndicator<SupplyZone>(new SupplyZone(){ Lookback = lookback, PivotBars = pivotBars, MaxRegionSize = maxRegionSize, RegionPadding = regionPadding }, input, ref cacheSupplyZone);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.SupplyZone SupplyZone(int lookback, int pivotBars, double maxRegionSize, double regionPadding)
		{
			return indicator.SupplyZone(Input, lookback, pivotBars, maxRegionSize, regionPadding);
		}

		public Indicators.SupplyZone SupplyZone(ISeries<double> input , int lookback, int pivotBars, double maxRegionSize, double regionPadding)
		{
			return indicator.SupplyZone(input, lookback, pivotBars, maxRegionSize, regionPadding);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.SupplyZone SupplyZone(int lookback, int pivotBars, double maxRegionSize, double regionPadding)
		{
			return indicator.SupplyZone(Input, lookback, pivotBars, maxRegionSize, regionPadding);
		}

		public Indicators.SupplyZone SupplyZone(ISeries<double> input , int lookback, int pivotBars, double maxRegionSize, double regionPadding)
		{
			return indicator.SupplyZone(input, lookback, pivotBars, maxRegionSize, regionPadding);
		}
	}
}

#endregion
