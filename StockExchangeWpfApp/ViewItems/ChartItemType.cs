using StockExchangeComon.Wrapers;
using StockExchangeDataModel.Models;
using System.Collections.Generic;

namespace StockExchangeWpfApp.ViewItems
{

    public class ChartItemType 
    {
        public ChartItemType()
        {
            ChartTypes = new List<ChartType> 
            { 
                ChartType.Column, 
                ChartType.Line, 
                ChartType.Scatter 
            };
        }

        public IEnumerable<ChartType> ChartTypes { get; }
    }
}
