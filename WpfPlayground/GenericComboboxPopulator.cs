using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPlayground
{
    public class GenericComboboxPopulator
    {
        private void TestGenericMethod()
        {
            SecuritiesDS securitiesDS = new SecuritiesDS();
            SecuritiesDS.StockRow sRow = securitiesDS.Stock.NewStockRow();
            sRow.Stock_Gid = Guid.NewGuid();
            sRow.Symbol = "AAPL";
            sRow.Price = 100;
            securitiesDS.Stock.AddStockRow(sRow);

            sRow = securitiesDS.Stock.NewStockRow();
            sRow.Stock_Gid = Guid.NewGuid();
            sRow.Symbol = "JPM";
            sRow.Price = 45;
            securitiesDS.Stock.AddStockRow(sRow);

            sRow = securitiesDS.Stock.NewStockRow();
            sRow.Stock_Gid = Guid.NewGuid();
            sRow.Symbol = "BNY";
            sRow.Price = 50;
            securitiesDS.Stock.AddStockRow(sRow);
            var v = securitiesDS.Stock.Select().ToList();
            var bl = GetbindingItems<SecuritiesDS.StockRow, Guid>(securitiesDS.Stock.Rows.Cast<SecuritiesDS.StockRow>().ToList(), x => x.Symbol, y => y.Stock_Gid);
        }

        private List<BindingItem<TValue>> GetbindingItems<TSource, TValue>(List<TSource> dataRows, Func<TSource, string> displayMemberSelector, Func<TSource, TValue> valueMemberSelector)
        {
            var items = new List<BindingItem<TValue>>();
            dataRows.ForEach(x => items.Add(new BindingItem<TValue>(displayMemberSelector(x), valueMemberSelector(x))));
            return items;
        }
    }

    public class BindingItem<TValue>
    {
        public string DisplayMember { get; set; }
        public TValue ValueMember { get; set; }
        public BindingItem(string displayMember, TValue valueMember)
        {
            DisplayMember = displayMember;
            ValueMember = valueMember;
        }
    }
}
